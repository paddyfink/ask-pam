using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace AskPam.Crm.Hubs
{
    public class BaseHub : Hub
    {
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddAsync(Context.ConnectionId, groupName);            
        }       
       
    }
}
