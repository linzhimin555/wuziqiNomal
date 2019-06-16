using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using Microsoft.AspNet.SignalR;
using wuziqiNomal.Models;

namespace wuziqiNomal
{
    public class ChatHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        public static List<User> users = new List<User>();

        public static List<Room> RoomList = new List<Room>();

        public string userKey = "userList";
        Cache _cache = HttpRuntime.Cache;
        public ChatHub()
        {
            
        }
        public void SendMessage(string user, string message, string room)
        {
            Clients.OthersInGroup(room).Send("ReceiveMessage", user, message);
        }

        public Room JoinRoom(string roomName)
        {
            Context.RequestCookies.TryGetValue("userId", out var userIds);
            var userId = userIds.Value;
            var room = RoomList.FirstOrDefault(o => o.RoomName == roomName);

            var user = users.FirstOrDefault(o => o.Id == userId);
            if (room == null)
                room = new Room
                {
                    RoomName = roomName,
                    Users = new List<RoomUser>(),
                };
            RoomUser u;
            if (room.Users == null || room.Users.Count == 0)
            {
                u = new RoomUser
                {
                    Id = userId,
                    Color = Chess.Black,
                };

            }
            else if (room.Users.Count == 1)
            {
                u = new RoomUser
                {
                    Id = userId,
                    Color = Chess.Red,
                };
            }
            else
            {
                u = new RoomUser
                {
                    Id = userId,
                    Color = Chess.Look,
                };
            }

            if (room.Users != null && !room.Users.Exists(o => o.Id == userId))
            {
                room.Users.Add(u);
            }

            users.FirstOrDefault(o => o.Id == userId).RoomId = roomName;
            RoomList.Add(new Room
            {
                RoomName = roomName,
                Start = false,
                Users = new List<RoomUser>
                {
                   u
                }
            }) ;

            Groups.Add(Context.ConnectionId, roomName);

            Clients.OthersInGroup(roomName).Send("OthersChange", u);
            return room;
        }

        public bool RemoveRoom(string roomName)
        {
            Context.RequestCookies.TryGetValue("userId", out var userIds);
            var userId = userIds.Value;
            var room = RoomList.FirstOrDefault(o => o.RoomName == roomName);

            if (room == null)
                return false;

            var user = room.Users.FirstOrDefault(o => o.Id == userId);
            room.Users.Remove(user);
          
            users.FirstOrDefault(o => o.Id == userId).RoomId = "";

            Clients.OthersInGroup(roomName).Send("OthersChange", user);
            Groups.Remove(Context.ConnectionId, roomName);
            return true;
        }

        public (bool, string) BeReady(string roomName)
        {
            Context.RequestCookies.TryGetValue("userId", out var userIds);
            var userId = userIds.Value;
            var room = RoomList.FirstOrDefault(o => o.RoomName == roomName);

            if (room != null)
            {
                var user = room.Users.FirstOrDefault(o => o.Id == userId);
                if (user != null)
                {
                    user.Ready = true;
                }

                if (room.Users.Count(o => o.Ready) >= 2)
                {
                    room.Start = true;
                     Clients.OthersInGroup(roomName).SendAsync("OthersChange", user);
                     Clients.OthersInGroup(roomName).SendAsync("Start", 1);
                    return (true, "two");
                }
                else
                {
                    Clients.OthersInGroup(roomName).SendAsync("OthersChange", user);
                    return (true, "one");
                }
            }
            return (false, "");
        }

        public void Start(string roomName)
        {
            Clients.OthersInGroup(roomName).SendAsync("Start", 1);
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }
    }
}