using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using UnityEngine;
using TMPro;
using System.IO;

public class EmailRecordings : MonoBehaviour
{
    public GameManager gameManager;

    public string senderEmail;
    public string senderPassword;
    public string defaultRecipientEmail;

    public string recipient1;
    public string recipient2;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }

   
    public void SendEmails(string audioFilePath)
    {
        Send(defaultRecipientEmail, audioFilePath);
        Send(recipient1, audioFilePath);
        Send(recipient2, audioFilePath);
    }

    private void Send(string recipientEmail, string attachmentFilePath)
    {
        if (string.IsNullOrEmpty(recipientEmail))
        {
            Debug.LogWarning("Recipient email is empty, skipping send.");
            return;
        }

        MailMessage mail = new MailMessage
        {
            From = new MailAddress(senderEmail),
            Subject = "Audio Recording- Thermal Recall Task",
            Body = "Please find the attached audio recording."
        };

        mail.To.Add(recipientEmail);

        // Attach the .wav file if the path is valid
        if (!string.IsNullOrEmpty(attachmentFilePath) && File.Exists(attachmentFilePath))
        {
            Attachment attachment = new Attachment(attachmentFilePath);
            mail.Attachments.Add(attachment);
        }
        else
        {
            Debug.LogError("Invalid or missing file path for attachment: " + attachmentFilePath);
        }

        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(senderEmail, senderPassword) as ICredentialsByHost,
            EnableSsl = true
        };

        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

        try
        {
            smtpServer.Send(mail);
            Debug.Log("Email sent successfully with attachment");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to send email: " + ex.Message);
        }
    }
}
