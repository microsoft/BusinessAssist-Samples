using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BusinessAssistAPIClient.Models
{


    //    {
    //    "OperationId": "525de32f-2984-4c6c-b97d-02f6f0d2f8ef",
    //    "Name": "forecast_sample",
    //    "Status": 0,
    //    "Diagnostics": null
    //}

    public class ForecastOpStatus
    {
        public string OperationId { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public string? Diagnostics { get; set; }
    }
public class BACreateForecastDataModel
    {
        public string Name { get; set; }
        public string inputDataJson { get; set; }
        public string DateTimeColumnName { get; set; }
        public string VolumeColumnName { get; set; }
        public string Seasonality { get; set; }
        public string EndDateTime { get; set; }

        public ForecastOpStatus? OperationStatus { get; set; }
                
    }
    public class BAForecastDataModel
    {
        public string QueryText { get; set; }
        public DateTime Requested { get; set; }
        public string Status { get; set; }
        public string Response { get; set; }
        public Guid OperationId { get; set; }
        //public string OperationStatus { get; set; }
        public string ForecastInputData { get; set; }
        public string Name { get; set; }
        public string inputDataJson { get; set; } = "[{\"Date\":\"2015-01-01\",\"Ticket\":\"195\"},{\"Date\":\"2015-01-02\",\"Ticket\":\"774\"},{\"Date\":\"2015-01-03\",\"Ticket\":\"134\"},{\"Date\":\"2015-01-04\",\"Ticket\":\"100\"},{\"Date\":\"2015-01-05\",\"Ticket\":\"975\"},{\"Date\":\"2015-01-06\",\"Ticket\":\"64\"},{\"Date\":\"2015-01-07\",\"Ticket\":\"748\"},{\"Date\":\"2015-01-08\",\"Ticket\":\"74\"}]";
        public string DateTimeColumnName { get; set; } = "Date";
        public string VolumeColumnName { get; set; } = "Ticket";
        public string Seasonality { get; set; } = "2";
        public string EndDateTime { get; set; } = "07/30/2022";

        public BACreateForecastDataModel GetForecastInputData() { 
            BACreateForecastDataModel model = new BACreateForecastDataModel();
            model.Name = this.Name;
            model.inputDataJson = inputDataJson;
            model.DateTimeColumnName = this.DateTimeColumnName;
            model.VolumeColumnName = this.VolumeColumnName;
            model.Seasonality = this.Seasonality;
            model.EndDateTime = this.EndDateTime;

            return model;
        }


        public ForecastDataResults? ForecastData { get; set; }
        public ForecastOpStatus? OperationStatus { get; set; }

    }
}
