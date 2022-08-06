using Microsoft.AspNetCore.SignalR;

namespace DeviceApi.Hubs
{
    public class DeviceStatusHub : Hub 
    {
        private List<string> OnlineDevices {get;set;}

        public DeviceStatusHub() {
            OnlineDevices = new List<string>();
        }

        public async Task SendPing(string displayid) 
        {
            Console.WriteLine(displayid + " is online!");
            OnlineDevices.Add(displayid);
            await Clients.All.SendAsync("online", displayid);
        }

        public async Task RemoveDisplay(string displayid)
        {
            Console.WriteLine(displayid + " is not online anymore!");
            OnlineDevices.Remove(displayid);
            await Clients.All.SendAsync("offline", displayid);
        }

    }    
}