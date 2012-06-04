﻿using System.Web.Mvc;
using Infrastructure.Events;
using YourApp.Models;
using YourDomain.Commands;

namespace YourApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ICommandSender commandSender;

        public AccountController(ICommandSender commandSender)
        {
            this.commandSender = commandSender;
        }

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Account/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Account/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Account/Create

        [HttpPost]
        public ActionResult Create(AccountCreationModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var command = new CreateAccount(model.Email, model.FirstName, model.LastName, model.Password);
                    commandSender.Send(command);

                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Account/Edit/5
 
        
    }
}
