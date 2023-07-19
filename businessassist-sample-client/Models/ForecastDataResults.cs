namespace BusinessAssistAPIClient.Models
{
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
    }
}
