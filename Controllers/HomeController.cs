using System;
using System.Web.Mvc;
using WebT.Data;
using WebT.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using PagedList;

namespace WebT.Controllers
{
    public class HomeController : Controller
    {
        UserRelContext db = new UserRelContext();
        public ActionResult Index(int? page)
        {
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            List<User> users = db.Users.Include("Orders").ToList();
            return View(users.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult UserCreate()
        {
            return PartialView("_UserCreate");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserCreate(User user)
        {
            db.Users.Add(user);

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult UserEdit(int id)
        {
            User user = db.Users.Find(id);
            if (user != null)
            {
                return PartialView("_UserEdit", user);
            }
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserEdit(User user)
        {
            
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult UserDetails(int id)
        {
            User user = db.Users.Find(id);
            if (user != null)
            {
                return PartialView("_UserDetails", user);
            }
            return View("Index");
        }

        public ActionResult UserDelete(int id)
        {
            User user = db.Users.Find(id);
            if (user != null)
            {
                return PartialView("_UserDelete", user);
            }
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("UserDelete")]
        public ActionResult UsDelete(int id)
        {
            User user = db.Users.Include("Orders").FirstOrDefault(us => us.Id == id);

            if (user != null)
            {
                db.Users.Remove(user);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult OrderDetails(int id)
        {
            Order order = db.Orders.Find(id);
            if (order != null)
            {
                return PartialView("_OrderDetails", order);
            }
            return View("Index");
        }

        public ActionResult OrderCreate(int userId)
        {
            User user = db.Users.Find(userId);

            if (user != null)
               return PartialView("_OrderCreate", new OrderViewModel { UserId = user.Id });

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrderCreate(OrderViewModel order)
        {
            User user = db.Users.Find(order.UserId);

            user.Orders.Add(new Order { Name = order.Name, Sum = order.Sum, User = user });

            db.SaveChanges();

            CalculateUserTotalSum(user);

            return RedirectToAction("Index");
        }

        public ActionResult OrderEdit(int id)
        {
            Order order = db.Orders.Include("User").FirstOrDefault(ord => ord.Id == id);
            if (order != null)
            {
                return PartialView("_OrderEdit", order);
            }
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrderEdit(Order order)
        {
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();

            CalculateUserTotalSum(order);

            return RedirectToAction("Index");
        }

        public ActionResult OrderDelete(int id)
        {
            Order order = db.Orders.Find(id);
            if (order != null)
            {
                return PartialView("_OrderDelete", order);
            }
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("OrderDelete")]
        public ActionResult OrdDelete(int id)
        {
            Order order = db.Orders.Include("User").FirstOrDefault(ord => ord.Id == id);

            if (order != null)
            {
                User user = order.User;
                db.Orders.Remove(order);
                db.SaveChanges();

                CalculateUserTotalSum(user);
            }
            return RedirectToAction("Index");
        }

        private void CalculateUserTotalSum(Order order)
        {
            User user = db.Users.Include("Orders").FirstOrDefault(us => us.Orders.Any(ord => ord.Id == order.Id));
            user.TotalSum = user.Orders.Sum(ord => ord.Sum);

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
        }

        private void CalculateUserTotalSum(User user)
        {
            user = db.Users.Include("Orders").FirstOrDefault(us => us.Id == user.Id);
            user.TotalSum = user.Orders.Sum(ord => ord.Sum);

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}