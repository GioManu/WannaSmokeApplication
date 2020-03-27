using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WannaSmoke.Infrastructure.Models;

namespace WannaSmoke.Client.Services.SocketService
{
    public interface ISocketService
    {
        bool TestConnection();
        void Reconnect();
        void OnConnected(Action<string> CallBackMethod);
        Task Invoke(string methodName);
        Task Connect();
        Task GoSmoke(Action<string> action, Action<string> OnErrorAction);
        Task RegisterIP(string name, string IP);
    }
    public class SocketService : ISocketService
    {
        public string Host { get; set; }
        public Config Config { get; set; }
        public String MyLocalIP { get; set; }
        public HubConnection connection { get; set; }
        public SocketService(Config config)
        {
            Config = config;
            Host = config.GetFullServer();
            Initialize();
        }
        public void Initialize()
        {
            connection = new HubConnectionBuilder()
               .WithUrl($"http://{Host}/{this.Config.Hub}")
               .WithAutomaticReconnect()
               .Build();
        }
        public async Task GoSmoke(Action<string> action,Action<string> OnErrorAction)
        {
            if(TestConnection())
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        var url = $"http://{Host}/Smoke";

                        var json = JsonConvert.SerializeObject(new GoSmokeRequest()
                        {
                            Name = Config.Name,
                            ConnectionID = connection.ConnectionId
                        });

                        var payload = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                        var res = await client.PostAsync(url, payload);
                    }

                }
                catch
                {
                    OnErrorAction("Server connection Error");
                }
            }
            else
            {
                OnErrorAction("No server connection");
            }
        }

        public void OnConnected(Action<string> CallBackMethod)
        {
            this.connection.On<string>("Receive", (name) => CallBackMethod(name));
        }

        public async Task Connect()
        {
            await this.connection.StartAsync();
        }
        public async Task Invoke(string methodName)
        {
            await this.connection.InvokeAsync(methodName);
        }
        public async Task RegisterIP(string name, string IP)
        {
            throw new NotImplementedException();
        }

        public void Reconnect()
        {
            connection = new HubConnectionBuilder()
                .WithUrl($"{this.Host}/{this.Config.Hub}")
                .WithAutomaticReconnect()
                .Build();
        }

        public bool TestConnection()
        {
            try
            {
                using (var client = new TcpClient(Config.Server,int.Parse(Config.Port)))
                    return true;
            }
            catch (SocketException ex)
            {
                return false;
            }
        }
    }
}
