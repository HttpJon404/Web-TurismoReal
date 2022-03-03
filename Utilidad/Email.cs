using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Utilidad
{
    public class Email
    {
        private static string Server;
        private static string From;
        private static string Psw;
        private static string MailJetPublicKey;
        private static string MailJetSecretKey;

        /// <summary>
        /// Función que envía un email en formato html
        /// </summary>
        /// <param name="subject">Asunto</param>
        /// <param name="to">correos destinatarios separados por ;</param>
        /// <param name="cc">correos con copia separados por ;</param>
        /// <param name="bcc">correos con copia oculta separados por ;</param>
        /// <param name="body">cuerpo en HTML</param>
        /// <returns>true si es existoso</returns>
        public static bool SendHtmlEmail(string subject, string to, string cc, string bcc, string body, string[] files = null, string textBody = "")
        {
            try
            {
                if (Server == null)
                {
                    Server = ConfigurationManager.AppSettings["EmailSMTPServer"];
                }
                if (From == null)
                {
                    From = ConfigurationManager.AppSettings["EmailEnvio"];
                    Psw = ConfigurationManager.AppSettings["EmailPass"];
                }
                SmtpClient client = new SmtpClient(Server, 587);
                var v_username = ConfigurationManager.AppSettings["EmailSMTPUsr"];
                var v_password = ConfigurationManager.AppSettings["EmailSMTPPwd"];
                client.Credentials = new NetworkCredential(v_username, v_password);
                client.EnableSsl = true;

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(From);
                msg.IsBodyHtml = false;
                if (!string.IsNullOrEmpty(to))
                {
                    to = to.Replace(";", ",");
                    msg.To.Add(to);
                }
                if (!string.IsNullOrEmpty(cc))
                {
                    cc = cc.Replace(";", ",");
                    msg.CC.Add(cc);
                }
                if (!string.IsNullOrEmpty(bcc))
                {
                    bcc = bcc.Replace(";", ",");
                    msg.Bcc.Add(bcc);
                }
                //------------------------------------------
                msg.Subject = subject;
                msg.Body = textBody;
                msg.Priority = MailPriority.Normal;

                ContentType mimeType = new System.Net.Mime.ContentType("text/html");
                // Add the alternate body to the message.

                AlternateView alternate = AlternateView.CreateAlternateViewFromString(body, mimeType);
                msg.AlternateViews.Add(alternate);

                
                // PROCESAR ARCHIVOS ADJUNTOS
                if (files != null)
                {
                    if (files.GetType().IsArray)
                    {
                        foreach (var item in files)
                        {
                            System.Net.Mail.Attachment dataAttach = new System.Net.Mail.Attachment(item, MediaTypeNames.Application.Octet);
                            msg.Attachments.Add(dataAttach);
                        }

                    }
                }
                //------------------------------------------

                client.Send(msg);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static void SendHtmlEmailJet(string subject, string to, string cc, string bcc, string body, string[] fileContents = null, string[] fileNames = null, string[] filesMimeTypes = null, string textBody = null)
        {
            if (string.IsNullOrEmpty(to) && string.IsNullOrEmpty(cc))
            {
                return;
            }

            EliminaDuplicados(ref to, ref cc, ref bcc);

            string[] files = null;
            var snd = SendHtmlEmail(subject, to, cc, bcc, body, fileNames, textBody);
            return;
        }

        public static void EliminaDuplicados(ref string to, ref string cc, ref string bcc)
        {
            to = to.ToLower().Replace(";", ",");
            cc = cc.ToLower().Replace(";", ",");
            bcc = bcc.ToLower().Replace(";", ",");

            var arrTo = to.Split(',');
            var arrCc = cc.Split(',');
            var arrBcc = bcc.Split(',');

            arrTo = arrTo.Distinct().ToArray();
            arrCc = arrCc.Distinct().ToArray();
            arrBcc = arrBcc.Distinct().ToArray();

            arrCc = arrCc.Except(arrTo).ToArray();
            arrBcc = arrBcc.Except(arrTo).ToArray();
            arrCc = arrCc.Except(arrBcc).ToArray();

            to = string.Join(",", arrTo);
            cc = string.Join(",", arrCc);
            bcc = string.Join(",", arrBcc);
        }
    }
}
