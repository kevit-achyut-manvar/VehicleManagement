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

        private void addToken(HttpClient httpClient, string token)
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
        }

        // GET: HomeController/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: HomeController/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(VehicleOwnerRegister ownerRegister)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseAddress);

                    HttpResponseMessage getData = await client.PostAsJsonAsync("api/Auth/Register", ownerRegister);

                    if (getData.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Login));
                    }
                    else
                    {
                        Console.WriteLine("\nError.......");
                        return View();
                    }
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: HomeController/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(VehicleOwnerLogin ownerLogin)
        {
            var data = new ServiceResponse<string>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseAddress);

                    HttpResponseMessage getData = await client.PostAsJsonAsync("api/Auth/Login", ownerLogin);

                    if (getData.IsSuccessStatusCode)
                    {
                        string results = getData.Content.ReadAsStringAsync().Result;
                        data = JsonConvert.DeserializeObject<ServiceResponse<string>>(results);
                        TokenManagement.userToken = data.Data;
                        addToken(client, TokenManagement.userToken);
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        Console.WriteLine("\nError.......");
                        return View();
                    }
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController
        public async Task<IActionResult> Index()
        {
            var data = new ServiceResponse<List<GetVehicleDto>>();

            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                addToken(client, TokenManagement.userToken);

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
                addToken(client, TokenManagement.userToken);

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
                    addToken(client, TokenManagement.userToken);

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
                    addToken(client, TokenManagement.userToken);

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
            var data = new ServiceResponse<UpdateVehicleDto>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                addToken(client, TokenManagement.userToken);

                HttpResponseMessage getData = await client.GetAsync("api/Vehicle/" + id);

                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<ServiceResponse<UpdateVehicleDto>>(results);
                }
                else
                {
                    Console.WriteLine("Error in consuming web API.");
                }
            }
            return View(data);
        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ServiceResponse<UpdateVehicleDto> updatedVehicle)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseAddress);
                    addToken(client, TokenManagement.userToken);

                    HttpResponseMessage getData = await client.PutAsJsonAsync("api/Vehicle/" + id, updatedVehicle);

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
    }

    public class TokenManagement
    {
        public static string userToken;
    }
}
