using Xunit;

namespace Teamworks.Mailgun.Test
{
    public class MessageHandlerTest
    {
        [Fact]
        public void EnsureAfterEmailSentAStringIsReturned()
        {
            Assert.IsType<string>(new MailMessageHandler().Send());
        }
    }
}
