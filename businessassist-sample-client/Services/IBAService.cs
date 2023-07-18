using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessAssistAPIClient.Models;

namespace BusinessAssistAPIClient.Services
{
    public interface IBAService
    {
        Task<BAValidateTenantModel> GetValidateTenantAsync();

        Task<BASelfHelpModel> GetSelfHelpAsync(BASelfHelpModel selfHelp);

        Task<BAForecastDataModel> GetForecastDataAsync(BAForecastDataModel forecastData);

        Task<BAForecastDataModel> GetForecastOpStatusAsync(BAForecastDataModel forecastData);

        Task<BAForecastDataModel> CreateForecastAsync(BAForecastDataModel forecastData);
    }
}
