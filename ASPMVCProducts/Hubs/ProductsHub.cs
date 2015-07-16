using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Collections;

namespace ASPMVCProducts
{
    [Authorize]
    public class ProductsHub : Hub
    {
        static Dictionary<string, List<string>> mConnectionsPerLogin = new Dictionary<string, List<string>>();


        public ProductsHub()
        {
        }

        public static IEnumerable<string> GetConnectionsIdsOf(string aUserName)
        {
            lock (mConnectionsPerLogin)
            {
                List<string> lConnections;
                if (!mConnectionsPerLogin.TryGetValue(aUserName, out lConnections))
                    return Enumerable.Empty<String>();

                return lConnections.ToArray();
            }
        }

        public static void ClearConnectionIdsOf(string aUserName)
        {
            lock (mConnectionsPerLogin)
            {
                mConnectionsPerLogin.Remove(aUserName);
            }
        }

        public override Task OnConnected()
        {
            _AddCurrentConnectionId();
            return base.OnConnected();
        }
        public override Task OnDisconnected()
        {
            _RemoveCurrentConnectionId();
            return base.OnDisconnected();
        }
        public override Task OnReconnected()
        {
            var user = Context.User;
            if (user.Identity.IsAuthenticated)
            {
                var lUserName = user.Identity.Name;
                lock (mConnectionsPerLogin)
                {
                    List<string> lConnections;
                    if (!mConnectionsPerLogin.TryGetValue(lUserName, out lConnections))
                    {
                        lConnections = new List<string>();
                        mConnectionsPerLogin[lUserName] = lConnections;
                    }
                    if (!lConnections.Contains(Context.ConnectionId))
                        lConnections.Add(Context.ConnectionId);
                }
            }
            return base.OnConnected();
        }

        private void _AddCurrentConnectionId()
        {
            var user = Context.User;
            if (user.Identity.IsAuthenticated)
            {
                var lUserName = user.Identity.Name;
                lock (mConnectionsPerLogin)
                {
                    List<string> lConnections;
                    if (!mConnectionsPerLogin.TryGetValue(lUserName, out lConnections))
                    {
                        lConnections = new List<string>();
                        mConnectionsPerLogin[lUserName] = lConnections;
                    }
                    if (!lConnections.Contains(Context.ConnectionId))
                        lConnections.Add(Context.ConnectionId);
                }
            }
        }
        private void _RemoveCurrentConnectionId()
        {
            var user = Context.User;
            if (user.Identity.IsAuthenticated)
            {
                var lUserName = user.Identity.Name;
                lock (mConnectionsPerLogin)
                {
                    List<string> lConnections;
                    if (mConnectionsPerLogin.TryGetValue(lUserName, out lConnections) && lConnections.Contains(Context.ConnectionId))
                    {
                        lConnections.Remove(Context.ConnectionId);
                    }
                }
            }
        }
    }
}