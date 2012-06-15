using Xunit;

namespace Teamworks.Mailgun.Test
{
    public class MessageHandlerTest
    {
        [Fact]
        public void EnsureAfterEmailSentAStringIsReturned()
        {
            var mail = new MailMessageHandler();
            mail.Send();
        }
    }
}
