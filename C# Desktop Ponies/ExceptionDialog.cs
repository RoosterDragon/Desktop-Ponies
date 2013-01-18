namespace CsDesktopPonies
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Reflection;
    using System.Windows.Forms;

    /// <summary>
    /// A user dialog designed to allow reporting of unexpected exceptions.
    /// </summary>
    public partial class ExceptionDialog : Form
    {
        /// <summary>
        /// Email address from which errors reports should be sent.
        /// </summary>
        private const string SenderEmailAddress = "something@gmail.com";
        /// <summary>
        /// Password for the sender email account.
        /// </summary>
        private const string SenderPassword = "OriginalPasswordDoNotSteal11234567890";
        /// <summary>
        /// SMTP host that will handle sending the email.
        /// </summary>
        private const string SenderSmtpHost = "smtp.gmail.com";
        /// <summary>
        /// Port for connecting to the SMTP server.
        /// </summary>
        private const int SenderSmtpPort = 587;

        /// <summary>
        /// The exception being reported on.
        /// </summary>
        private Exception exception;
        /// <summary>
        /// Email address to which error reports should be sent.
        /// </summary>
        private string recipientEmail;
        /// <summary>
        /// The assembly that created the dialog, and wishes to report on the error.
        /// </summary>
        private Assembly sourceAssembly;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsDesktopPonies.ExceptionDialog"/> class.
        /// </summary>
        /// <param name="ex">The exception on which information should be displayed.</param>
        /// <param name="destinationEmailAddress">The e-mail address where an error report should be sent, or null to prevent reporting.
        /// </param>
        public ExceptionDialog(Exception ex, string destinationEmailAddress)
        {
            if (ex == null)
                throw new ArgumentNullException("ex");

            exception = ex;
            recipientEmail = destinationEmailAddress;
            sourceAssembly = Assembly.GetCallingAssembly();

            InitializeComponent();
            EmailCommand.Visible = recipientEmail != null;
            ExceptionInformation.Text = string.Format(CultureInfo.CurrentCulture,
                "Exception: {0}\nMessage-\n{1}", ex.GetType().FullName, ex.Message);
        }

        /// <summary>
        /// Raised when an error report should be emailed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Data about the event.</param>
        private void EmailCommand_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this,
                "This will send an e-mail containing detailed error information to '" + recipientEmail +
                "'. Do you want to send this message?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                MailMessage message = null;
                SmtpClient client = null;
                bool success = true;
                try
                {
                    string exceptionTypeName = exception.GetType().FullName;
                    string exceptionToString = exception.ToString();
                    string subject = "ERROR: " + exception.Source + " " + exceptionTypeName;
                    AppDomain domain = AppDomain.CurrentDomain;
                    string body = string.Format(CultureInfo.CurrentCulture,
                        "Exception Source: {0}\n" +
                        "AppDomain Name: {1}\n" +
                        "Assembly Name: {2}\n" +
                        "Exception Type: {3}\n" +
                        "Exception Message-\n{4}\n" +
                        "Exception Trace-\n{5}\n" +
                        "Loaded Assemblies-\n{6}",
                        exception.Source,
                        domain.FriendlyName,
                        sourceAssembly.FullName,
                        exceptionTypeName,
                        exception.Message,
                        exception.StackTrace,
                        string.Join("\n", domain.GetAssemblies().Select(assembly => assembly.FullName)));
                    MessageBox.Show(body);
                    Environment.Exit(1);
                    message = new MailMessage(SenderEmailAddress, recipientEmail, subject, body);
                    client = new SmtpClient(SenderSmtpHost, SenderSmtpPort)
                    {
                        EnableSsl = true,
                        Credentials = new NetworkCredential(SenderEmailAddress, SenderPassword),
                        Timeout = 5000
                    };
                    client.Send(message);
                }
                catch (FormatException)
                {
                    MessageBox.Show(this, "Could not send message. Recipient address '" + recipientEmail + "' is not valid.",
                        "Invalid Address");
                    success = false;
                }
                catch (SmtpFailedRecipientException ex)
                {
                    MessageBox.Show(this, "Message could not be sent to its recipient(s).\n\n" + ex.Message, "Message Not Sent");
                    success = false;
                }
                catch (SmtpException ex)
                {
                    MessageBox.Show(this, "Error attempting to send the message.\n\n" + ex.Message, "Error Sending Message");
                    success = false;
                }
                finally
                {
                    if (message != null)
                        message.Dispose();
                    if (client != null)
                        client.Dispose();
                }

                if (success)
                {
                    MessageBox.Show(this, "Message sent successfully.", "Message Sent");
                    EmailCommand.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Raised when the dialog should be closed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Data about the event.</param>
        private void CloseCommand_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
