using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Postman
{
    public class Helper
    {
        public static void SendReport(Constants provider, int id, bool Ok, string ErrorMessage)
        {
            var url = provider.SourceUrl;
            if (Ok)
            {
                url = url + "?ok=1&secret=" + provider.Secret + "&ReturnKey=" + provider.ReturnKey + "&id=" + id;
            }
            else
            {
                url = url + "?error=1&secret=" + provider.Secret + "&ReturnKey=" + provider.ReturnKey + "&id=" + id + "&error_message=" + ErrorMessage;
            }
            var options = new RestClientOptions(url)
            {
                ThrowOnAnyError = true,
                MaxTimeout = 5000
            };
            var client = new RestClient(options);
            RestRequest r = new RestRequest();
            r.AddHeader("user-agent", "Mozilla /5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (HTML, like Gecko) Chrome/86.0.4240.111 Safari/537.36 OPR/72.0.3815.186");
            var resp = client.Get(r);
            int respStatusCode = (int)resp.StatusCode;
            if (respStatusCode == 200)
            {
                Console.WriteLine("Mail #" + id + " reporting...");
                Console.WriteLine();
            }
        }

        public static void TryAgain(int WaitForNextFetch)
        {
            string tryAgainText = "Try again after " + WaitForNextFetch + " seconds [";
            Console.Write(tryAgainText);
            for (int t = 0; t < WaitForNextFetch; t++) Console.Write(" ");
            Console.Write("]");
            for (int t = 0; t < WaitForNextFetch + 1; t++) Console.Write("\b");

            for (int t = 0; t < WaitForNextFetch; t++)
            {
                Thread.Sleep(500);
                Console.Write("■");
                Thread.Sleep(500);
            }

            for (int t = 0; t < WaitForNextFetch + tryAgainText.Length; t++)
            {
                Console.Write("\b");
            }
        }

        public static void Header(Constants provider)
        {
            Console.WriteLine("OT.Postman V1.0");
            Console.WriteLine();
            if (provider.SourceUrl != "") Console.WriteLine("Source URL (" + provider.SourceUrl + ")");
            if (provider.SmtpHost != "") Console.WriteLine("SMTP: " + provider.SmtpHost);
            if (provider.SmtpPort > 0) Console.WriteLine("Port: " + provider.SmtpPort);
            if (provider.FromMail != "") Console.WriteLine("From: \"" + provider.FromName + "\" <" + provider.FromMail + ">");
            Console.WriteLine("\n");
        }


        public static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    Console.Write("■");
                    password += info.KeyChar;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        password = password.Substring(0, password.Length - 1);
                        int pos = Console.CursorLeft;
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }
                }
                info = Console.ReadKey(true);
            }

            Console.WriteLine();
            return password;
        }

    }
}


