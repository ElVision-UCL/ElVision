using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ElVisionLibrary.Models.ElPris
{
    public class ProductPriceDetails
    {
        public double TotalPrice { get; set; }
        public double TotalTax { get; set; }
        public double SupplierPrice { get; set; }
        public double GridCompanyPrice { get; set; }
        public double StatePrice { get; set; }
        public double ProductPriceAmount { get; set; }
        public double ProductSubscription { get; set; }
        public double ProductFee { get; set; }
        public double DistributionTariff { get; set; }
        public double DistributionSubscription { get; set; }
        public double DistributionFee { get; set; }
        public double TransmissionTariff { get; set; }
        public double SystemTariff { get; set; }
        public double ElAfgift { get; set; }
        public bool IsNetworkTariffIncluded { get; set; }
        public string TotalMonthlySubscription { get; set; }
        public string MonthlyProductSubscription { get; set; }
        public string MonthlyDistributionSubscription { get; set; }
    }

    public class PriceFactors
    {
        public double Price { get; set; }
        public double Subscription { get; set; }
        public double Fee { get; set; }
        public double FeeEl { get; set; }
    }

    public class NetworkFactors
    {
        public double Distribution { get; set; }
        public double Subscription { get; set; }
        public double Fee { get; set; }
        public double Transmission { get; set; }
        public double System { get; set; }
    }

    public class PriceDetail
    {
        public string Name { get; set; }
        public string AmountText { get; set; }
        public List<SubPrice> SubPrices { get; set; }
        public string TooltipKey { get; set; }
    }

    // Represents a sub-price detail
    public class SubPrice
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AmountText { get; set; }
        public string TooltipKey { get; set; }
    }

    // Main class for calculated prices
    public class ProductDetails
    {
        public string Supplier { get; set; }
        public string Name { get; set; }
        public string OrderUrl { get; set; }
        public bool ProductIsExpiring { get; set; }
        public double TotalPriceSortable { get; set; }
        public string TotalPrice { get; set; }
        public string DeliveryDaysPrice { get; set; }
        public string SupplierPriceIncludingTax { get; set; }
        public string SupplierPrice { get; set; }
        public string GridCompanyPrice { get; set; }
        public string StatePrice { get; set; }
        public string SystemTariff {  get; set; }
        public string TransmissionTariff { get; set; }
        public string Fee { get; set; }
        public List<PriceDetail> Prices { get; set; } = new List<PriceDetail>();
        public Product Product { get; set; }
        public bool ShowDetails { get; set; } = false;
        public string DropDownIcon { get; set; }


    }
}
