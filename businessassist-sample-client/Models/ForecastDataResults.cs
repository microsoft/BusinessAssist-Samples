namespace BusinessAssistAPIClient.Models
{

    //"predictions": [
    //{
    //    "date": "2015-01-09T00:00:00+00:00",
    //    "forecast": 768.88764,
    //    "low": 113.85303,
    //    "high": 1423.9222
    //},


    public class ForecastDataItem
    {
        public string? Date { get; set; }
        public double? Forecast { get; set; }
        public double? Low { get; set; }
        public double? High { get; set; }

    }

    public class ForecastDataPredictions
    {
        public List<ForecastDataItem> Items { get; set; }

    }
    public class ForecastDataResults
    {
        public List<ForecastDataItem>? Predictions { get; set; }

        //public string? Predictions { get; set }


    }
}
