using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HomeBudgetManagement.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace HomeBudgetManagement.Controllers
{
    public class HomeController : Controller
    {
        //HttpClient is intended to be instantiated once and reused throughout the life of an application.The following conditions can result in SocketException errors:
        //1.Creating a new HttpClient instance per request.
        //2.Server under heavy load.
        //Creating a new HttpClient instance per request can exhaust the available sockets.
        static HttpClient client = new HttpClient() { BaseAddress = new Uri("http://localhost:63052/") };

        public async Task<ActionResult> Index()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            List<ExpenseViewModel> list = new List<ExpenseViewModel>();
            HttpResponseMessage response =  await client.GetAsync("api/Expense/List");

            if (response.IsSuccessStatusCode)
            {
                list = JsonConvert.DeserializeObject<List<ExpenseViewModel>>(await response.Content.ReadAsStringAsync());
            }
           
            return View(list);
        }
 
        
        // GET: Home
        public async Task<ActionResult> GetExpensesUsingApiAsync()
        {
            HttpClient x = new HttpClient();

            string BaseAddress = "http://localhost:63052/api/Expense/List";

            var response = await x.GetAsync(BaseAddress);
            if (response.IsSuccessStatusCode)
            {
                List<Expense> expenses = await response.Content.ReadAsAsync<List<Expense>>();
                return View(expenses);
            }

            return HttpNotFound(); 
        }



        // GET: Home/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
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

        // GET: Home/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Home/Edit/5
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

        // GET: Home/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Home/Delete/5
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
