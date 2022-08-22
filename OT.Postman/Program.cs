﻿
using Newtonsoft.Json;
using System.Threading;

using OT.Postman;
using RestSharp;
using System;

IEmailSender iEmailSender = new EmailService();

bool InfinityLoop = true;
bool FirstFetch = false;
int WaitForNextFetch = 10;
int WaitForNextMail = 500;

Constants provider = new Constants();
Helper.Header(provider);

bool TargetUrl = false;

while (!TargetUrl)
{
    Console.WriteLine("Source URL:");
    var newUrl = Console.ReadLine();
    if (!string.IsNullOrEmpty(newUrl) && newUrl.Length > 10)
    {
        if (!newUrl.EndsWith("/")) newUrl = newUrl + "/";
        provider.TargetUrl = newUrl;
        TargetUrl = true;
    }
}


Console.WriteLine("SMTP Password:");
string newSmtpPassword = Helper.ReadPassword();

Console.WriteLine("Secret Key: (if needed)");
string secretKey = Helper.ReadPassword();

if (!string.IsNullOrEmpty(newSmtpPassword) && newSmtpPassword.Length > 3) provider.EmailPassword = newSmtpPassword.ToString();
if (!string.IsNullOrEmpty(secretKey) && secretKey.Length > 1) provider.Secret = secretKey.ToString();

while (InfinityLoop)
{
    int TryId = 0;
    try
    {
        if (FirstFetch == false)
        {
            Console.Clear();
            Helper.Header(provider);
            FirstFetch = true;
        }

        RestClientOptions options = new RestClientOptions(provider.TargetUrl + "?fetch=ok&secret=" + provider.Secret)
        {
            ThrowOnAnyError = true,
            MaxTimeout = 5000
        };
        var client = new RestClient(options);

        RestRequest request = new RestRequest();
        request.AddHeader("user-agent", "Mozilla /5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (HTML, like Gecko) Chrome/86.0.4240.111 Safari/537.36 OPR/72.0.3815.186");
        var response = client.Get(request);
        int responseStatusCode = (int)response.StatusCode;
        if (responseStatusCode == 200)
        {


            if (response.Content != null)
            {

                var root = JsonConvert.DeserializeObject<EmailList>(response.Content);
                if (root != null && root.data != null)
                {
                    if (!string.IsNullOrEmpty(root.WaitForNextFetch)) WaitForNextFetch = Int32.Parse(root.WaitForNextFetch);
                    if (!string.IsNullOrEmpty(root.FromMail)) provider.FromMail = root.FromMail;
                    if (!string.IsNullOrEmpty(root.FromName)) provider.FromName = root.FromName;
                    if (!string.IsNullOrEmpty(root.SmtpHost)) provider.SmtpHost = root.SmtpHost;
                    if (!string.IsNullOrEmpty(root.SmtpPort)) provider.SmtpPort = Int32.Parse(root.SmtpPort);

                    Console.Clear();
                    Helper.Header(provider);
                    Console.WriteLine("Let's Go Baby! We have some emails..\n");
                    Console.WriteLine("A total of " + root.AllMailCount + " emails are waiting to be sent.\nWe are now sending " + root.MailCount + " of them.\n");

                    foreach (EmailContent ec in root.data)
                    {
                        if (ec != null)
                        {
                            if (ec.ToName != null && ec.ToMail != null && ec.Content != null && ec.Subject != null)
                            {
                                TryId = ec.Id;
                                Console.WriteLine("Sending #" + TryId);

                                Console.WriteLine("Sender: \"" + provider.FromName + "\" <" + provider.FromMail + ">");
                                Console.WriteLine("MailTo: \"" + ec.ToMail + "\" Subject: \"" + ec.Subject + "\"");

                                Task mr = iEmailSender.SendEmailAsync(provider, ec.ToName, ec.ToMail, ec.Subject, ec.Content);

                                if (mr != null && mr.IsCompletedSuccessfully)
                                {
                                    Console.WriteLine("Completed Successfully");
                                    Helper.SendReport(provider, TryId, true, "");
                                }
                                else if (mr != null && mr.Exception != null)
                                {
                                    Console.WriteLine("Some problems found");
                                    Helper.SendReport(provider, TryId, false, mr.Exception.Message);
                                }

                                Thread.Sleep(WaitForNextMail);
                            }
                        }
                    }
                }
            }
        }
        else if (responseStatusCode == 404)
        {
            Console.WriteLine("?? Houston, we have a problem! TargetURL is not found!");
        }
    }
    catch (Exception e)
    {

        Console.WriteLine("Some problems found");
        Console.WriteLine(e.Message);
        Helper.SendReport(provider, TryId, false, e.Message);
    }

    Helper.TryAgain(WaitForNextFetch);

}
