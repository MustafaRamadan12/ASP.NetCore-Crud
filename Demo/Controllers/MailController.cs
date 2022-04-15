using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using Demo.BL.Models;
using Demo.BL.Helper;
using Microsoft.AspNetCore.Authorization;

namespace Demo.Controllers
{
    [Authorize]
    public class MailController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Index(MailVM model)
        {
            try
            {
                TempData["Msg"] = MailSender.SendMail(model);
                return View();
            }catch(Exception ex)
            {
                TempData["Msg"] = MailSender.SendMail(model);
                return View();
            }
        }
    }
}
