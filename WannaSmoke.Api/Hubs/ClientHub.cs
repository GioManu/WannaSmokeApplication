using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WannaSmoke.Api.Hubs
{
    public interface IClientHub
    {
        public Task NotifyClients(string message);
    }
    public class ClientHub : Hub
    {
        public async Task Notify()
        {
            await Clients.All.SendAsync("Receive","Hello");
        }
        public async Task SendNudes()
        {
            await Clients.All.SendAsync("Receive","Hello");
        }
    }
}
