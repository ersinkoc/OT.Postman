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


Sample MySQL Table for E-Mails

```sql
CREATE TABLE `send_emails` (
  `id` int NOT NULL AUTO_INCREMENT,
  `email` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
  `title` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `content` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `is_send` tinyint(1) NOT NULL DEFAULT '0',
  `record_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `send_date` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  `trying` tinyint(1) NOT NULL DEFAULT '0',
  `next_try_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `error` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci ROW_FORMAT=DYNAMIC;

```

