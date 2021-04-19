using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TaxLab.B2C.TestClient.Models;
using TaxLab.B2C.TestClient.ViewModels;

namespace TaxLab.B2C.TestClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly HttpClient _httpClient;
        private readonly AppSettings _config;

        public HomeController(IWebHostEnvironment environment, HttpClient httpClient, IOptions<AppSettings> config)
        {
            _environment = environment;
            _httpClient = httpClient;
            _config = config.Value;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> CallApi()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _config.SubscriptionKey);

            try
            {
                var content = await _httpClient.GetStringAsync(_config.ApiUrl);
                var response = JsonConvert.DeserializeObject<DataFeedResponse>(content);
                var documents = JsonConvert.SerializeObject(response.Documents);
                ViewBag.Json = JArray.Parse(documents).ToString();
                return View("json");
            }
            catch (HttpRequestException e)
            {
                ViewBag.Json = e.Message;
                return View("json");
            }

        }
    }
}
