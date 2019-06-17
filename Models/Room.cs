using System.Collections.Generic;

namespace wuziqiNomal.Models
{
    public class Room
    {
        public long RoomId { get; set; }

        public string RoomName { get; set; }

        public List<RoomUser> Users { get; set; }

        public bool Start { get; set; } = false;
    }

    public class User
    {
        public string Id { get; set; }

        public string Ip { get; set; }

        public int Sex { get; set; }

        public string RoomId { get; set; }
    }

    public class RoomUser
    {
        public string Id { get; set; }

        public bool Ready { get; set; } = false;

        public Chess Color { get; set; }
    }
    public enum Chess
    {
        Black = 1,
        Red = -1,
        Look = 0
    }
}
