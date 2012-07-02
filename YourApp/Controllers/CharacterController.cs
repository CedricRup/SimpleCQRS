using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raven.Client;
using Services.Queries;
using YourDomain;
using Raven.Client.Linq;

namespace YourApp.Controllers
{
    public class CharacterController : Controller
    {
        private readonly IDocumentStore store;

        public CharacterController(IDocumentStore store)
        {
            this.store = store;
        }

        public ActionResult Index(string term)
        {
            using (var session = store.OpenSession())
            {

                return
                    View(
                        session.Query<CharacterByPopularity.Result,CharacterByPopularity>() // on cherche des Result dans l'index CharacterByPopularity
                        .As<CharacterByPopularity.CharacterWithPopalurity>() //mais les résultats ressembleront à des objets CharacterWithPopalurity
                        .OrderByDescending(c => c.Popularity).ToList()); // et on trie
            }
            
        }

        //
        // GET: /Character/Details/5


        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Character/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Character/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Character/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Character/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Character/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Character/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
