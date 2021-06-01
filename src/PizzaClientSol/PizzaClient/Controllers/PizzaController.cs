//using System.Runtime.InteropServices;

//// In SDK-style projects such as this one, several assembly attributes that were historically
//// defined in this file are now automatically added during build and populated with
//// values defined in project properties. For details of which attributes are included
//// and how to customise this process see: https://aka.ms/assembly-info-properties


//// Setting ComVisible to false makes the types in this assembly not visible to COM
//// components.  If you need to access a type in this assembly from COM, set the ComVisible
//// attribute to true on that type.

//[assembly: ComVisible(false)]

//// The following GUID is for the ID of the typelib if this project is exposed to COM.

//[assembly: Guid("0bc8b8e4-8596-4654-9c01-274ebb47cad7")]

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PizzaClient.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace PizzaClient.Controllers
{
    public class PizzaController : Controller
    {
        public async Task<IActionResult> HomePage()
        {
            string BaseUrl = "http://localhost:30297/";
            var PizzaInfo = new List<Pizza>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/Pizzas");
                if (Res.IsSuccessStatusCode)
                {
                    var PizzaResponse = Res.Content.ReadAsStringAsync().Result;
                    PizzaInfo = JsonConvert.DeserializeObject<List<Pizza>>(PizzaResponse);
                }
                return View(PizzaInfo);

            }
        }
        [HttpGet]
        public ActionResult OrderDetails(int id, string name, decimal price)
        {
            ViewBag.Price = price;
            ViewBag.Name = name;
            ViewBag.Id = id;
            return View();


        }
        [HttpPost]
        public async Task<ActionResult> OrderDetails(OrderDetails Od)
        {
            
                using (var client = new HttpClient())
                {

                    StringContent content = new StringContent(JsonConvert.SerializeObject(Od), Encoding.UTF8, "application/json");
                    using (var response = await client.PostAsync("http://localhost:30297/api/OrderDetails", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var obj= JsonConvert.DeserializeObject<OrderDetails>(apiResponse);
                        TempData["Order_id"] = obj.OrderId;
                    }
                }
                return RedirectToAction("Order");         

        }





        public async Task<ActionResult> Order(int id)
        {
            int Orderid = Convert.ToInt32(TempData.Peek("Order_id"));
            OrderDetails od = new OrderDetails();
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync("http://localhost:30297/api/OrderDetails/" + Orderid))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    od = JsonConvert.DeserializeObject<OrderDetails>(apiResponse);
                }
                return View(od);

            }
        }

        public IActionResult Success()
        {
            return View();
        }



    } 
}

