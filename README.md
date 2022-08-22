# OT.Postman

You need the source URL to receive pending emails.

Example: https://your.source.url/mails/

***PREPARE JSON ON***

?fetch=ok&secret=[SECRET] 

***SUCCESS ENDPOINT***

?ok=1&id=[Id]&secret=[SECRET]

***ERROR ENDPOINT***

?error=1&id=[Id]&secret=[SECRET]&error_message=houston!wehaveaproblem...

JSON Example:

```json

{
  "AllMailCount": 100,
  "MailCount": 2,
  "WaitForNextFetch": "15",
  "FromMail": "no-reply@blabla.net",
  "FromName": "BlaBla Mail",
  "SmtpHost": "mail.blabla.bla",
  "SmtpPort": "587",
  "data": [
    {
      "Id": 45,
      "ToName": "Name45",
      "ToMail": "to45@mail.bla",
      "Subject": "Email Subject",
      "Content": "Email Content"
    },{
      "Id": 46,
      "ToName": "Name46",
      "ToMail": "to46@mail.bla",
      "Subject": "Email Subject",
      "Content": "Email Content"
    }
  ]
}

```

