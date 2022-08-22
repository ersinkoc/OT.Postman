// See https://aka.ms/new-console-template for more information

public class EmailList
{
    public int AllMailCount { get; set; }
    public int MailCount { get; set; }
    public string? WaitForNextFetch { get; set; }
    public string? SmtpHost { get; set; }
    public string? SmtpPort { get; set; }
    public string? FromName { get; set; }
    public string? FromMail { get; set; }
    public List<EmailContent>? data { get; set; }
}
