using Microsoft.AspNetCore.SignalR;

namespace MeetMVC.Infrastructure
{
    public class ChatIdProvider //: IUserIdProvider
    {
        private IHttpContextAccessor _contextAccessor;
        public ChatIdProvider(IHttpContextAccessor httpContextAccessor)
        {
            _contextAccessor = httpContextAccessor;
        }
        /*public string? GetUserId(HubConnectionContext connection)
        {
            
        }*/
    }
}
