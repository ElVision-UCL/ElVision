using ElVisionLibrary.Models.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElVisionLibrary.Models.ElOverblik.TimeSeries
{
    public class MarketEvaluationPoint
    {
        [JsonProperty("mRID")]
        public MRID MRID { get; set; }
    }

    public class MRID
    {
        [JsonProperty("codingScheme")]
        public string CodingScheme { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class MyEnergyDataMarketDocument
    {
        [JsonProperty("mRID")]
        public string MRID { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("sender_MarketParticipant.name")]
        public string SenderMarketParticipantName { get; set; }

        [JsonProperty("sender_MarketParticipant.mRID")]
        public SenderMarketParticipantMRID SenderMarketParticipantMRID { get; set; }

        [JsonProperty("period.timeInterval")]
        public PeriodTimeInterval PeriodTimeInterval { get; set; }

        [JsonProperty("TimeSeries")]
        public List<TimeSeries> TimeSeries { get; set; }
    }

    public class Period
    {
        [JsonProperty("resolution")]
        public string Resolution { get; set; }

        [JsonProperty("timeInterval")]
        public TimeInterval TimeInterval { get; set; }

        [JsonProperty("Point")]
        public List<Point> Point { get; set; }
    }

    public class PeriodTimeInterval
    {
        [JsonProperty("start")]
        public DateTime Start { get; set; }

        [JsonProperty("end")]
        public DateTime End { get; set; }
    }

    public class Point
    {
        [JsonProperty("position")]
        public string Position { get; set; }

        [JsonProperty("out_Quantity.quantity")]
        public string OutQuantityQuantity { get; set; }

        [JsonProperty("out_Quantity.quality")]
        public string OutQuantityQuality { get; set; }
    }

    public class Result
    {
        [JsonProperty("MyEnergyData_MarketDocument")]
        public MyEnergyDataMarketDocument MyEnergyDataMarketDocument { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("errorCode")]
        public int ErrorCode { get; set; }

        [JsonProperty("errorText")]
        public string ErrorText { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("stackTrace")]
        public object StackTrace { get; set; }
    }

    public class TimeSeriesResponse
    {
        [JsonProperty("result")]
        public List<Result> Result { get; set; }
    }

    public class SenderMarketParticipantMRID
    {
        [JsonProperty("codingScheme")]
        public object CodingScheme { get; set; }

        [JsonProperty("name")]
        public object Name { get; set; }
    }

    public class TimeInterval
    {
        [JsonProperty("start")]
        public DateTime Start { get; set; }

        [JsonProperty("end")]
        public DateTime End { get; set; }
    }

    public class TimeSeries
    {
        [JsonProperty("mRID")]
        public string MRID { get; set; }

        [JsonProperty("businessType")]
        public string BusinessType { get; set; }

        [JsonProperty("curveType")]
        public string CurveType { get; set; }

        [JsonProperty("measurement_Unit.name")]
        public string MeasurementUnitName { get; set; }

        [JsonProperty("MarketEvaluationPoint")]
        public MarketEvaluationPoint MarketEvaluationPoint { get; set; }

        [JsonProperty("Period")]
        public List<Period> Period { get; set; }


    }

}
