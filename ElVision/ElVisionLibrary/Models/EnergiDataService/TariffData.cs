using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElVisionLibrary.Models.EnergiDataService
{
    public class Tariff
    {
        public string ChargeOwner { get; set; }
        public string GLN_Number { get; set; }
        public string ChargeType { get; set; }
        public string ChargeTypeCode { get; set; }
        public string Note { get; set; }
        public string Description { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string VATClass { get; set; }
        public double Price1 { get; set; }
        public object Price2 { get; set; }
        public object Price3 { get; set; }
        public object Price4 { get; set; }
        public object Price5 { get; set; }
        public object Price6 { get; set; }
        public object Price7 { get; set; }
        public object Price8 { get; set; }
        public object Price9 { get; set; }
        public object Price10 { get; set; }
        public object Price11 { get; set; }
        public object Price12 { get; set; }
        public object Price13 { get; set; }
        public object Price14 { get; set; }
        public object Price15 { get; set; }
        public object Price16 { get; set; }
        public object Price17 { get; set; }
        public object Price18 { get; set; }
        public object Price19 { get; set; }
        public object Price20 { get; set; }
        public object Price21 { get; set; }
        public object Price22 { get; set; }
        public object Price23 { get; set; }
        public object Price24 { get; set; }
        public int TransparentInvoicing { get; set; }
        public int TaxIndicator { get; set; }
        public string ResolutionDuration { get; set; }
    }

    public class TariffData
    {
        public int total { get; set; }
        public string filters { get; set; }
        public int limit { get; set; }
        public string dataset { get; set; }
        [JsonProperty("records")]
        public List<Tariff> Tariffs { get; set; }
    }
}
