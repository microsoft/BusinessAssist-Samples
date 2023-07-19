
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using System.Diagnostics;
using BusinessAssistAPIClient.Models;
using BusinessAssistAPIClient.Services;
using BusinessAssistAPIClient.Utils;
using Azure;

namespace BusinessAssistAPIClient.Controllers
{
    [AuthorizeForScopes(ScopeKeySection = "BusinessAssist:BAServiceScope")]
    public class BusinessAssistController : Controller
    {
        private IBAService _baService;

        public BusinessAssistController(IBAService baService)
        {
            _baService = baService;
        }

        public IActionResult Index()
        {
            TempData["SignedInUser"] = User.GetDisplayName();
            string signedInUser = User.GetDisplayName();
            Debug.WriteLine($"BusinessAssistController:Index() called by {signedInUser}");
            return View();
        }

        public async Task<IActionResult> ValidateTenant()
        {
            var signedInUser = HttpContext.User.GetDisplayName();
            Debug.WriteLine($"BusinessAssistController:ValidateTenant() called by {signedInUser}");
            try
            {
                BAValidateTenantModel result = (await _baService.GetValidateTenantAsync());
                return View(result);
            }
            catch (WebApiMsalUiRequiredException ex)
            {
                return Redirect(ex.Message);
            }
        }
        public Task<IActionResult> SelfHelp()
        {
            var signedInUser = HttpContext.User.GetDisplayName();
            Debug.WriteLine($"BusinessAssistController:SelfHelp() called by {signedInUser}");

            try
            {
                return Task.FromResult<IActionResult>(View());
            }
            catch (WebApiMsalUiRequiredException ex)
            {
                return Task.FromResult<IActionResult>(Redirect(ex.Message));
            }
        }

        // POST: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SelfHelp([Bind("QueryText")] BASelfHelpModel selfHelp)
        {
            var signedInUser = HttpContext.User.GetDisplayName();
            Debug.WriteLine($"BusinessAssistController:SelfHelp(BASelfHelp) called by {signedInUser}");
            
            try
            {
                BASelfHelpModel result = (await _baService.GetSelfHelpAsync(selfHelp));
                return View(result);
            }
            catch (WebApiMsalUiRequiredException ex)
            {
                return Redirect(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetForecastData()
        {
            var signedInUser = HttpContext.User.GetDisplayName();
            Debug.WriteLine($"BusinessAssistController:GetForecastData() called by {signedInUser}");

            BAForecastDataModel forecastData = new BAForecastDataModel();

            try
            {
                BAForecastDataModel result = (await _baService.GetForecastDataAsync(forecastData));
                return View(result);
            }
            catch (WebApiMsalUiRequiredException ex)
            {
                return Redirect(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="forecastData"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetForecastData([Bind("QueryText")] BAForecastDataModel forecastData)
        {
            var signedInUser = HttpContext.User.GetDisplayName();
            Debug.WriteLine($"BusinessAssistController:GetForecastData() called by {signedInUser}");
            try
            {
                BAForecastDataModel result = (await _baService.GetForecastDataAsync(forecastData));
                return View(result);
            }
            catch (WebApiMsalUiRequiredException ex)
            {
                return Redirect(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetForecastOpStatus()
        {
            var signedInUser = HttpContext.User.GetDisplayName();
            Debug.WriteLine($"BusinessAssistController:GetForecastOpStatus() called by {signedInUser}");

            BAForecastDataModel forecastData = new BAForecastDataModel();

            try
            {
                BAForecastDataModel result = (await _baService.GetForecastOpStatusAsync(forecastData));
                return View(result);
            }
            catch (WebApiMsalUiRequiredException ex)
            {
                return Redirect(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="forecastData"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetForecastOpStatus([Bind("QueryText")] BAForecastDataModel forecastData)
        {
            var signedInUser = HttpContext.User.GetDisplayName();
            Debug.WriteLine($"BusinessAssistController:GetForecastOpStatus() called by {signedInUser}");
            try
            {
                BAForecastDataModel result = (await _baService.GetForecastOpStatusAsync(forecastData));
                return View(result);
            }
            catch (WebApiMsalUiRequiredException ex)
            {
                return Redirect(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> CreateForecast()
        {
            var signedInUser = HttpContext.User.GetDisplayName();
            Debug.WriteLine($"BusinessAssistController:GetForecastOpStatus() called by {signedInUser}");

            BAForecastDataModel forecastData = new BAForecastDataModel();

            try
            {
                BAForecastDataModel result = (await _baService.CreateForecastAsync(forecastData));
                return View(result);
            }
            catch (WebApiMsalUiRequiredException ex)
            {
                return Redirect(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="forecastData"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateForecast([Bind("Name, inputDataJson, DateTimeColumnName, VolumeColumnName, Seasonality, EndDateTime")] BAForecastDataModel forecastData)
        {
            var signedInUser = HttpContext.User.GetDisplayName();
            Debug.WriteLine($"BusinessAssistController:GetForecastOpStatus() called by {signedInUser}");
            try
            {
                BAForecastDataModel result = (await _baService.CreateForecastAsync(forecastData));
                return View(result);
            }
            catch (WebApiMsalUiRequiredException ex)
            {
                return Redirect(ex.Message);
            }
        }
    }
}
