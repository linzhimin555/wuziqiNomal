using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wuziqiNomal.Models;

namespace wuziqiNomal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var userIds = HttpContext.Request.Cookies.Get("userId");
            var userId = "";
            if (userIds == null)
            {
                userId = Guid.NewGuid().ToString();
                HttpCookie cookie = new HttpCookie("userId", userId);
                HttpContext.Response.Cookies.Add(cookie);
            }
            else
            {
                userId = userIds.Value;
            }
            var users = ChatHub.users;
            if (users == null)
            {
                users = new List<User>();
            }
            users.Add(new User { Id = userId });

            var RoomList = new List<string>();
            for (var i = 0; i < 50; i++)
            {
                RoomList.Add($"Room{i}");
            }

            return View(RoomList);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Room(string roomid)
        {
            var roomId = roomid;

            var userIds = HttpContext.Request.Cookies.Get("userId");
            var userId = userIds.Value;

            var users = ChatHub.users;

            var user = users.FirstOrDefault(o => o.Id == userId);

            var RoomList = ChatHub.RoomList;

            var Room = RoomList.FirstOrDefault(o => o.RoomName == roomId);

            var RoomUser1 = Room.Users.FirstOrDefault(o => o.Id == userId);

            var RoomUser = Room.Users.Where(o => o.Id != userId).ToList();

            var Start = Room.Start;

            ViewBag.RoomUser = RoomUser;
            ViewBag.RoomUser1 = RoomUser1;

            ViewBag.RoomId = roomId;

            ViewBag.Start = Start;


            return View();
        }
    }
}