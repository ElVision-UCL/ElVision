using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ElVisionLibrary.Models.ElPris
{
    public class ProductData
    {
        [JsonProperty("distributionAreaGridArea")]
        public string DistributionAreaGridArea { get; set; }
        [JsonProperty("distributionAreaName")]
        public string DistributionAreaName { get; set; }
        [JsonProperty("products")]
        public List<Product> Products { get; set; }
    }
    public class Branding
    {
        [JsonProperty("brandings")]
        public List<object> Brandings { get; set; }
    }

    public class Climate
    {
        [JsonProperty("indication")]
        public int Indication { get; set; }

        [JsonProperty("energySources")]
        public EnergySources EnergySources { get; set; }

        [JsonProperty("extraClimateActionDescription")]
        public string ExtraClimateActionDescription { get; set; }

        [JsonProperty("sustainableEnergyLevel")]
        public double SustainableEnergyLevel { get; set; }
    }

    public class EnergySources
    {
        [JsonProperty("Vandkraft")]
        public double Vandkraft { get; set; }

        [JsonProperty("Vindkraft")]
        public double? Vindkraft { get; set; }

        [JsonProperty("Generel_deklaration")]
        public double? GenerelDeklaration { get; set; }

        [JsonProperty("Biomasse")]
        public double? Biomasse { get; set; }

        [JsonProperty("Solenergi")]
        public double? Solenergi { get; set; }

        [JsonProperty("Biogas")]
        public double? Biogas { get; set; }

        [JsonProperty("Hydrotermisk_energi")]
        public double? HydrotermiskEnergi { get; set; }
    }

    public class Fee
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("amountText")]
        public string AmountText { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("tooltipKey")]
        public string TooltipKey { get; set; }
    }

    public class Matrix
    {
        [JsonProperty("hoursFrom")]
        public int HoursFrom { get; set; }

        [JsonProperty("hoursTo")]
        public int HoursTo { get; set; }

        [JsonProperty("volumeFrom")]
        public int VolumeFrom { get; set; }

        [JsonProperty("volumeTo")]
        public int VolumeTo { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("monday")]
        public bool Monday { get; set; }

        [JsonProperty("tuesday")]
        public bool Tuesday { get; set; }

        [JsonProperty("wednesday")]
        public bool Wednesday { get; set; }

        [JsonProperty("thursday")]
        public bool Thursday { get; set; }

        [JsonProperty("friday")]
        public bool Friday { get; set; }

        [JsonProperty("saturday")]
        public bool Saturday { get; set; }

        [JsonProperty("sunday")]
        public bool Sunday { get; set; }
    }

    public class NextProduct
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("productId")]
        public int ProductId { get; set; }

        [JsonProperty("productVariantId")]
        public int ProductVariantId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("supplier")]
        public Supplier Supplier { get; set; }

        [JsonProperty("productType")]
        public string ProductType { get; set; }

        [JsonProperty("private1")]
        public bool Private1 { get; set; }

        [JsonProperty("business")]
        public bool Business { get; set; }

        [JsonProperty("billingType")]
        public string BillingType { get; set; }

        [JsonProperty("fixBilling")]
        public bool FixBilling { get; set; }

        [JsonProperty("flexBilling")]
        public bool FlexBilling { get; set; }

        [JsonProperty("offeredService")]
        public string OfferedService { get; set; }

        [JsonProperty("orderUrl")]
        public string OrderUrl { get; set; }

        [JsonProperty("readmoreUrl")]
        public string ReadMoreUrl { get; set; }

        [JsonProperty("branding")]
        public Branding Branding { get; set; }

        [JsonProperty("climate")]
        public Climate Climate { get; set; }

        [JsonProperty("service")]
        public Service Service { get; set; }

        [JsonProperty("productInformation")]
        public ProductInformation ProductInformation { get; set; }

        [JsonProperty("deliveryStart")]
        public string DeliveryStart { get; set; }

        [JsonProperty("deliveryEnd")]
        public string DeliveryEnd { get; set; }

        [JsonProperty("deliveryPeriodDays")]
        public int DeliveryPeriodDays { get; set; }

        [JsonProperty("totalDeliveryPeriodDays")]
        public int TotalDeliveryPeriodDays { get; set; }

        [JsonProperty("totalDeliveryPeriodDaysFromTommorow")]
        public int TotalDeliveryPeriodDaysFromTomorrow { get; set; }

        [JsonProperty("bindingPeriod")]
        public string BindingPeriod { get; set; }

        [JsonProperty("termination")]
        public string Termination { get; set; }

        [JsonProperty("numberOfPayments")]
        public int NumberOfPayments { get; set; }

        [JsonProperty("paymentDescription")]
        public string PaymentDescription { get; set; }

        [JsonProperty("paymentType")]
        public string PaymentType { get; set; }

        [JsonProperty("paymentMethods")]
        public List<string> PaymentMethods { get; set; }

        [JsonProperty("deposit")]
        public double Deposit { get; set; }

        [JsonProperty("fees")]
        public List<Fee> Fees { get; set; }

        [JsonProperty("otherFees")]
        public List<string> OtherFees { get; set; }

        [JsonProperty("productPrices")]
        public List<ProductPrice> ProductPrices { get; set; }

        [JsonProperty("introOffer")]
        public bool IntroOffer { get; set; }

        [JsonProperty("offeredProduct")]
        public string OfferedProduct { get; set; }

        [JsonProperty("productPriceUpdateInterval")]
        public ProductPriceUpdateInterval ProductPriceUpdateInterval { get; set; }
    }

    public class OtherDay
    {
        [JsonProperty("days")]
        public List<int> Days { get; set; }

        [JsonProperty("matrix")]
        public List<Matrix> Matrix { get; set; }
    }

    public class Product
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("productId")]
        public int ProductId { get; set; }

        [JsonProperty("productVariantId")]
        public int ProductVariantId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("supplier")]
        public Supplier Supplier { get; set; }

        [JsonProperty("productType")]
        public ProductType ProductType { get; set; }

        [JsonProperty("private1")]
        public bool Private1 { get; set; }

        [JsonProperty("business")]
        public bool Business { get; set; }

        [JsonProperty("billingType")]
        public string BillingType { get; set; }

        [JsonProperty("fixBilling")]
        public bool FixBilling { get; set; }

        [JsonProperty("flexBilling")]
        public bool FlexBilling { get; set; }

        [JsonProperty("offeredService")]
        public string OfferedService { get; set; }

        [JsonProperty("orderUrl")]
        public string OrderUrl { get; set; }

        [JsonProperty("readmoreUrl")]
        public string ReadMoreUrl { get; set; }

        [JsonProperty("branding")]
        public Branding Branding { get; set; }

        [JsonProperty("climate")]
        public Climate Climate { get; set; }

        [JsonProperty("service")]
        public Service Service { get; set; }

        [JsonProperty("productInformation")]
        public ProductInformation ProductInformation { get; set; }

        [JsonProperty("deliveryStart")]
        public string DeliveryStart { get; set; }

        [JsonProperty("deliveryEnd")]
        public string DeliveryEnd { get; set; }

        [JsonProperty("deliveryPeriodDays")]
        public int DeliveryPeriodDays { get; set; }

        [JsonProperty("totalDeliveryPeriodDays")]
        public int TotalDeliveryPeriodDays { get; set; }

        [JsonProperty("totalDeliveryPeriodDaysFromTommorow")]
        public int TotalDeliveryPeriodDaysFromTomorrow { get; set; }

        [JsonProperty("bindingPeriod")]
        public string BindingPeriod { get; set; }

        [JsonProperty("termination")]
        public string Termination { get; set; }

        [JsonProperty("numberOfPayments")]
        public int NumberOfPayments { get; set; }

        [JsonProperty("paymentDescription")]
        public string PaymentDescription { get; set; }

        [JsonProperty("paymentType")]
        public string PaymentType { get; set; }

        [JsonProperty("paymentMethods")]
        public List<string> PaymentMethods { get; set; }

        [JsonProperty("deposit")]
        public double Deposit { get; set; }

        [JsonProperty("fees")]
        public List<Fee> Fees { get; set; }

        [JsonProperty("otherFees")]
        public List<string> OtherFees { get; set; }

        [JsonProperty("productPrices")]
        public List<ProductPrice> ProductPrices { get; set; }

        [JsonProperty("introOffer")]
        public bool IntroOffer { get; set; }

        [JsonProperty("nextProduct")]
        public NextProduct NextProduct { get; set; }

        [JsonProperty("offeredProduct")]
        public string OfferedProduct { get; set; }

        [JsonProperty("productPriceUpdateInterval")]
        public ProductPriceUpdateInterval ProductPriceUpdateInterval { get; set; }
    }

    public class ProductInformation
    {
        [JsonProperty("subscriptionBased")]
        public string SubscriptionBased { get; set; }

        [JsonProperty("binding")]
        public string Binding { get; set; }

        [JsonProperty("priceTypeName")]
        public string PriceTypeName { get; set; }

        [JsonProperty("priceUpdateIntervalName")]
        public string PriceUpdateIntervalName { get; set; }
    }

    public class ProductPrice
    {
        [JsonProperty("subscription")]
        public double Subscription { get; set; }

        [JsonProperty("containsNetworkTariff")]
        public bool ContainsNetworkTariff { get; set; }

        [JsonProperty("additionalNordPoolSpot")]
        public bool AdditionalNordPoolSpot { get; set; }

        [JsonProperty("minimumConsumption")]
        public int MinimumConsumption { get; set; }

        [JsonProperty("validFrom")]
        public string ValidFrom { get; set; }

        [JsonProperty("validTo")]
        public string ValidTo { get; set; }

        [JsonProperty("lastUpdate")]
        public string LastUpdate { get; set; }

        [JsonProperty("matrix")]
        public List<Matrix> Matrix { get; set; }

        [JsonProperty("otherDays")]
        public List<OtherDay> OtherDays { get; set; }

        [JsonProperty("maximumConsumption")]
        public int? MaximumConsumption { get; set; }
    }

    public class ProductPriceUpdateInterval
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class ProductsWithNoPrice
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("productId")]
        public int ProductId { get; set; }

        [JsonProperty("productVariantId")]
        public int ProductVariantId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("supplier")]
        public Supplier Supplier { get; set; }

        [JsonProperty("productType")]
        public ProductType ProductType { get; set; }

        [JsonProperty("private1")]
        public bool Private1 { get; set; }

        [JsonProperty("business")]
        public bool Business { get; set; }

        [JsonProperty("orderUrl")]
        public string OrderUrl { get; set; }

        [JsonProperty("readmoreUrl")]
        public string ReadMoreUrl { get; set; }

        [JsonProperty("branding")]
        public Branding Branding { get; set; }

        [JsonProperty("climate")]
        public Climate Climate { get; set; }

        [JsonProperty("service")]
        public Service Service { get; set; }

        [JsonProperty("productInformation")]
        public ProductInformation ProductInformation { get; set; }
    }

    public class Service
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public class Supplier
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class ProductType
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
