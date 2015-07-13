using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace ASPMVCProducts
{
    public class ProductsHub : Hub
    {
        public ProductsHub()
        {
        }
        public void NotifyEvent_All(string aEventName, object aEventData)
        {
            Clients.All.OnServerEvent(aEventName, aEventData);
        }
        public void NotifyEvent_Others(string aEventName, object aEventData)
        {
            Clients.Others.OnServerEvent(aEventName, aEventData);
        }
    }
}