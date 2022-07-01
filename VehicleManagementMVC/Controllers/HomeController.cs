using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VehicleManagementMVC.Models;

namespace VehicleManagementMVC.Controllers
{
    public class HomeController : Controller
    {
        string baseAddress = "https://localhost:7191/";
        string userToken = "bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI1IiwibmFtZSI6IkFjaHl1dCIsIm5iZiI6MTY1NjY1MTA2OCwiZXhwIjoxNjU2NzM3NDY4LCJpYXQiOjE2NTY2NTEwNjh9.SVBCGMimRebSCWKdOBCicAnO813gE-oxqZDLQTnzGkmXu006SBltW7EtAaQEGNPw9UwJlsBzGy5kL1MZR8Lu9A";

        // GET: HomeController
        public async Task<IActionResult> Index()
        {
            var data = new ServiceResponse<List<GetVehicleDto>>();

            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Add("Authorization", userToken);

                HttpResponseMessage getData = await client.GetAsync("api/Vehicle/GetAll");

                if(getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<ServiceResponse<List<GetVehicleDto>>>(results);
                }
                else
                {
                    Console.WriteLine("Error in consuming web API.");
                }
                ViewData.Model = data;
            }
            return View();
        }

        // GET: HomeController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var data = new ServiceResponse<GetVehicleDto>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Add("Authorization", userToken);

                HttpResponseMessage getData = await client.GetAsync("api/Vehicle/" + id);

                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<ServiceResponse<GetVehicleDto>>(results);
                }
                else
                {
                    Console.WriteLine("Error in consuming web API.");
                }
                ViewData.Model = data;
            }
            return View();
        }

        // GET: HomeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AddVehicleDto addVehicle)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseAddress);
                    client.DefaultRequestHeaders.Add("Authorization", userToken);

                    HttpResponseMessage getData = await client.PostAsJsonAsync<AddVehicleDto>("api/Vehicle/", addVehicle);

                    if (getData.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        Console.WriteLine("Error........");
                        return View();
                    }   
                }  
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, GetVehicleDto vehicle)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseAddress);
                    client.DefaultRequestHeaders.Add("Authorization", userToken);

                    HttpResponseMessage getData = await client.DeleteAsync("api/Vehicle/" + id);

                    if (getData.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        Console.WriteLine("Error........");
                        return View();
                    }
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            return View();
        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, UpdateVehicleDto updatedVehicle)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseAddress);
                    client.DefaultRequestHeaders.Add("Authorization", userToken);

                    HttpResponseMessage getData = await client.PutAsJsonAsync("api/Vehicle/" + id, updatedVehicle);

                    if (getData.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Details));
                    }
                    else
                    {
                        Console.WriteLine("Error........");
                        return View();
                    }
                }
            }
            catch
            {
                return View();
            }
        }
    }
}
