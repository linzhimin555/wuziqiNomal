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
            var userIds =  HttpContext.Request.Cookies.Get("userId");
            var userId = userIds.ToString();
            if (userId == null)
            {
                userId = Guid.NewGuid().ToString();
                HttpCookie cookie = new HttpCookie("userId", userId);
                HttpContext.Response.Cookies.Add(cookie);
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
        public ActionResult Room()
        {
            var userIds = HttpContext.Request.Cookies.Get("userId");
            var userId = userIds.ToString();

            var users = ChatHub.users;

            var user = users.FirstOrDefault(o => o.Id == userId);


            _memoryCache.TryGetValue<Room>(RoomId, out var Room);

            RoomUser1 = Room.Users.FirstOrDefault(o => o.Id == userId);

            RoomUser = Room.Users.Where(o => o.Id != userId).ToList();

            Start = Room.Start;
            return View();
        }
    }
}