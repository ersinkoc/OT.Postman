# OT.Postman

Veritabanına kaydedilen mailler için SMTP üzerinden mail gönderme console zımbırtısı

.NET 6 (Core) ile MailKit kullanılarak hazırlandı. Derlendiğinde her ortamda (win,lin,mac) çalışır.

JSON kaynağı adresiniz https://your.source.url/mails/ gibi olmalı.

Yazılım ***[SOURCEURL]?fetch=ok&secret=[SECRET]*** adresinden örnekteki gibi bir JSON alır. Json içeriğinde SMTP bilgileri (şifre hariç), toplam mail sayısı, json içindeki maillerin sayısı (tavsiye 10 mail'i geçmesin), sonraki sorgu için beklenecek süre (saniye olarak) ve maillerin id, alıcı adı, mail adresi, başlık ve içerik detayı bulunur.

JSON içeriği için örnek SQL sorgusu

```sql
SELECT id as Id, name as ToName ,email as ToMail, title as Subject ,content as Content FROM send_emails WHERE is_send = 0 AND trying < 6 AND next_try_date <= now() ORDER by next_try_date ASC LIMIT 10
```

Eğer SMTP sorunsuz bir şekilde mail gönderirse bunu ***[SOURCEURL]?ok=1&id=[Id]&secret=[SECRET]*** adresine raporlar. Mailleri gönderildi olarak işaretleyebilirsiniz.

```sql
UPDATE send_emails SET is_send = 1, send_date = now() WHERE id = '[ID]'
```

Eğer bir hata oluşursa ***[SOURCEURL]?error=1&id=[Id]&secret=[SECRET]&error_message=houston!wehaveaproblem...*** adresine rapor gönderilir. Hatalı mailler için ise şu şekilde bir SQL kullanılabilir. 


```sql
UPDATE send_emails SET send_date = '0000-00-00 00:00:00', is_send = 0, error = '[error_message]', trying = trying + 1, next_try_date = DATE_ADD(NOW() , INTERVAL 5 MINUTE) WHERE is_send = 0 AND id = '[ID]'
```

JSON Örneği:

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
  `email` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
  `name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
  `title` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
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

