using ElVision.Extensions;
using ElVisionLibrary.Models.ElPris;
using ElVisionLibrary.Models.EnergiDataService;
using MudBlazor;

namespace ElVision.Utilities
{
    public static class ProductUtilities
    {
        public static ProductPriceDetails CalculatePriceDetail(
            int deliveryPeriodDays,
            bool calculateTax,
            double annualConsumption,
            bool electricalHeating,
            ConsumptionProfile consumptionProfile,
            Product product,
            DistributionAreaChargeData distributionAreaCharge,
            List<AverageElspotPrice> averageDailyPrices,
            List<NationalTariff> nationalTariffs)
        {
            var isFixBilling = product.BillingType == "fix";

            var consumptionProfilePercentage = BuildConsumptionProfilePercentage(consumptionProfile);

            ProductPrice validProductPrice = GetValidProductPrice(product, annualConsumption);
            if (validProductPrice == null)
            {
                return null;
            }

            var isNetworkTariffIncluded = validProductPrice.ContainsNetworkTariff;
            var monthlyProductSubscription = validProductPrice.Subscription;
            var annualProductSubscription = monthlyProductSubscription * 12;

            // fix return null
            var annualProductPrice = CalculateAnnualProductPrice(validProductPrice, annualConsumption, isFixBilling, consumptionProfilePercentage, averageDailyPrices);

            double annualProductFee = CalculateAnnualProductFee(product);

            CalculateNetworkTariffDistributionCharges(isNetworkTariffIncluded, annualConsumption, consumptionProfilePercentage, distributionAreaCharge,
                 out double monthlyDistributionSubscription, out double annualDistributionTariff, out double annualDistributionFee, out double distributionChargeAreaTariff);

            double annualDistributionSubscription = monthlyDistributionSubscription * 12;

            ProcessNationalCharges(nationalTariffs,
                out double transmissionTariff, out double systemTariff, out double elAfgift);

            double annualTransmissionTariff = 0, annualSystemTariff = 0;

            if (!isNetworkTariffIncluded)
            {
                annualTransmissionTariff = CalculateAnnualTariff(transmissionTariff, annualConsumption);
                annualSystemTariff = CalculateAnnualTariff(systemTariff, annualConsumption);
            }

            CalculateElAfgift(electricalHeating, annualConsumption, elAfgift,
                out double annualElAfgift);

            var calculatedSupplierPrice = CalculateSupplierPrice(annualProductPrice, annualProductSubscription, annualProductFee);
            var calculatedGridCompanyPrice = CalculateGridCompanyPrice(annualSystemTariff, annualDistributionTariff, annualTransmissionTariff, annualDistributionSubscription, annualDistributionFee);
            var calculatedStatePrice = CalculateStatePrice(annualElAfgift);

            var totalPrice = calculatedSupplierPrice + calculatedGridCompanyPrice + calculatedStatePrice;

            double totalTax = 0;
            if (calculateTax)
            {
                totalTax = totalPrice * 0.25;
            }

            double totalMonthlySubscription = CalculateMonthlySubscription(monthlyProductSubscription, monthlyDistributionSubscription, isNetworkTariffIncluded, calculateTax);

            ProductPriceDetails productPriceDetails = new ProductPriceDetails
            {
                TotalMonthlySubscription = FormatPrice(totalMonthlySubscription, calculateTax),
                MonthlyProductSubscription = FormatPrice(monthlyProductSubscription, calculateTax),
                MonthlyDistributionSubscription = FormatPrice(monthlyDistributionSubscription, calculateTax),
                IsNetworkTariffIncluded = isNetworkTariffIncluded,
                TotalPrice = totalPrice,
                TotalTax = totalTax,
                SupplierPrice = calculatedSupplierPrice,
                GridCompanyPrice = calculatedGridCompanyPrice,
                StatePrice = calculatedStatePrice,
                ProductPriceAmount = annualProductPrice,
                ProductSubscription = annualProductSubscription * 100,
                ProductFee = annualProductFee * 100,
                DistributionTariff = annualDistributionTariff * 100,
                DistributionSubscription = annualDistributionSubscription * 100,
                DistributionFee = annualDistributionFee * 100,
                TransmissionTariff = annualTransmissionTariff * 100,
                SystemTariff = annualSystemTariff * 100,
                ElAfgift = annualElAfgift * 100,
            };

            return productPriceDetails;
        }

        public static ProductPriceDetails CalculatePriceDetail(
            int deliveryPeriodDays,
            bool calculateTax,
            double annualConsumption,
            bool electricalHeating,
            ConsumptionProfile consumptionProfile,
            NextProduct product,
            DistributionAreaChargeData distributionAreaCharge,
            List<AverageElspotPrice> averageDailyPrices,
            List<NationalTariff> nationalTariffs)
        {
            var isFixBilling = product.BillingType == "fix";

            var consumptionProfilePercentage = BuildConsumptionProfilePercentage(consumptionProfile);

            ProductPrice validProductPrice = GetValidProductPrice(product, annualConsumption);
            if (validProductPrice == null)
            {
                return null;
            }

            var isNetworkTariffIncluded = validProductPrice.ContainsNetworkTariff;
            var monthlyProductSubscription = validProductPrice.Subscription;
            var annualProductSubscription = monthlyProductSubscription * 12;

            var annualProductPrice = CalculateAnnualProductPrice(validProductPrice, annualConsumption, isFixBilling, consumptionProfilePercentage, averageDailyPrices);

            double annualProductFee = CalculateAnnualProductFee(product);

            CalculateNetworkTariffDistributionCharges(isNetworkTariffIncluded, annualConsumption, consumptionProfilePercentage, distributionAreaCharge,
                 out double monthlyDistributionSubscription, out double annualDistributionTariff, out double annualDistributionFee, out double distributionChargeAreaTariff);

            double annualDistributionSubscription = monthlyDistributionSubscription * 12;

            ProcessNationalCharges(nationalTariffs,
                out double transmissionTariff, out double systemTariff, out double elAfgift);

            double annualTransmissionTariff = 0, annualSystemTariff = 0;

            if (!isNetworkTariffIncluded)
            {
                annualTransmissionTariff = CalculateAnnualTariff(transmissionTariff, annualConsumption);
                annualSystemTariff = CalculateAnnualTariff(systemTariff, annualConsumption);
            }

            CalculateElAfgift(electricalHeating, annualConsumption, elAfgift,
                out double annualElAfgift);

            var calculatedSupplierPrice = CalculateSupplierPrice(annualProductPrice, annualProductSubscription, annualProductFee);
            var calculatedGridCompanyPrice = CalculateGridCompanyPrice(annualSystemTariff, annualDistributionTariff, annualTransmissionTariff, annualDistributionSubscription, annualDistributionFee);
            var calculatedStatePrice = CalculateStatePrice(annualElAfgift);

            var totalPrice = calculatedSupplierPrice + calculatedGridCompanyPrice + calculatedStatePrice;

            double totalTax = 0;
            if (calculateTax)
            {
                totalTax = totalPrice * 0.25;
            }

            double totalMonthlySubscription = CalculateMonthlySubscription(monthlyProductSubscription, monthlyDistributionSubscription, isNetworkTariffIncluded, calculateTax);

            ProductPriceDetails productPriceDetails = new ProductPriceDetails
            {
                TotalMonthlySubscription = FormatPrice(totalMonthlySubscription, calculateTax),
                MonthlyProductSubscription = FormatPrice(monthlyProductSubscription, calculateTax),
                MonthlyDistributionSubscription = FormatPrice(monthlyDistributionSubscription, calculateTax),
                IsNetworkTariffIncluded = isNetworkTariffIncluded,
                TotalPrice = totalPrice,
                TotalTax = totalTax,
                SupplierPrice = calculatedSupplierPrice,
                GridCompanyPrice = calculatedGridCompanyPrice,
                StatePrice = calculatedStatePrice,
                ProductPriceAmount = annualProductPrice,
                ProductSubscription = annualProductSubscription * 100,
                ProductFee = annualProductFee * 100,
                DistributionTariff = annualDistributionTariff * 100,
                DistributionSubscription = annualDistributionSubscription * 100,
                DistributionFee = annualDistributionFee * 100,
                TransmissionTariff = annualTransmissionTariff * 100,
                SystemTariff = annualSystemTariff * 100,
                ElAfgift = annualElAfgift * 100,
            };

            return productPriceDetails;
        }

        public static double[][] BuildConsumptionProfilePercentage(ConsumptionProfile consumptionProfile)
        {
            var consumptionProfilePercentage = new double[8][];
            for (int d = 1; d <= 7; d++)
            {
                consumptionProfilePercentage[d] = new double[24];
            }

            if (consumptionProfile is not null && consumptionProfile.HourProfile is not null && consumptionProfile.HourProfile.Count == 7)
            {
                for (int dayKey = 1; dayKey <= 7; dayKey++)
                {
                    var hourProfile = consumptionProfile.HourProfile[$"{dayKey}"];
                    if (hourProfile != null && hourProfile.Count > 0)
                    {
                        for (int i = 0; i < hourProfile.Count; i++)
                        {
                            for (int h = hourProfile[i].HourFrom; h < hourProfile[i].HourTo; h++)
                            {
                                consumptionProfilePercentage[dayKey][h] = hourProfile[i].PercentagePerHour;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int d = 1; d <= 7; d++)
                {
                    for (int h = 0; h < 24; h++)
                    {
                        consumptionProfilePercentage[d][h] = 100.0 / 24.0 / 7.0;
                    }
                }
            }

            return consumptionProfilePercentage;
        }

        public static ProductPrice GetValidProductPrice(Product product, double annualConsumption)
        {
            if (product.ProductPrices != null)
            {
                foreach (var productPrice in product.ProductPrices)
                {
                    bool between = Between(annualConsumption, productPrice.MinimumConsumption, productPrice.MaximumConsumption);
                    if (between)
                    {
                        return productPrice;
                    }
                }
            }

            return null;
        }

        public static ProductPrice GetValidProductPrice(NextProduct product, double annualConsumption)
        {
            if (product.ProductPrices != null)
            {
                foreach (var productPrice in product.ProductPrices)
                {
                    bool between = Between(annualConsumption, productPrice.MinimumConsumption, productPrice.MaximumConsumption);
                    if (between)
                    {
                        return productPrice;
                    }
                }
            }

            return null;
        }

        public static double CalculateAnnualProductPrice(ProductPrice validProductPrice, double annualConsumption, bool isFixBilling, double[][] consumptionProfilePercentage, List<AverageElspotPrice> averageElspotPrices)
        {
            double[] elspotProfile = new double[24];

            if (validProductPrice.AdditionalNordPoolSpot)
            {
                // commented is old code, but keep it for now for reference in case of issues.
                //if (nordpool == null) // If the product price is based on Nord Pool prices and no Nord Pool is given, return null.
                //{
                //    return 0;
                //}

                //var nordpools = nordpool.Nordpools[0];

                //if (distributionAreaData.Area == nordpool.Nordpools[1].Area)
                //{
                //    nordpools = nordpool.Nordpools[1];
                //}

                //for (int h = 0; h < 24; h++)
                //{
                //    nordpoolProfile[h] = nordpools.HourProcent[h];
                //}

                for (int h = 0; h < 24; h++)
                {
                    elspotProfile[h] = Convert.ToDouble(averageElspotPrices[h].SpotPriceDKK);
                }
            }
            else // If no add-on of Nord Pool spot, set array to 0.
            {
                for (int h = 0; h < 24; h++)
                {
                    elspotProfile[h] = 0;
                }
            }

            double annualProductPrice = 0;

            for (int day = 1; day <= 7; day++)
            {
                var matrix = validProductPrice.Matrix;

                // Check if there are other days with custom pricing
                if (validProductPrice.OtherDays != null && validProductPrice.OtherDays.Count > 0)
                {
                    foreach (var od in validProductPrice.OtherDays)
                    {
                        if (od.Days.Contains(day))
                        {
                            matrix = od.Matrix;
                            break;
                        }
                    }
                }

                foreach (var row in matrix)
                {
                    double thisVolumeFrom = row.VolumeFrom;
                    double thisVolumeTo = row.VolumeTo;
                    int thisHoursFrom = row.HoursFrom;
                    int thisHoursTo = row.HoursTo;
                    double thisAmount = row.Amount;
                    double volume = 0;

                    // Calculate the volume to be billed
                    if (annualConsumption > thisVolumeTo)
                    {
                        volume = thisVolumeTo - thisVolumeFrom;
                    }
                    else if (annualConsumption >= thisVolumeFrom)
                    {
                        volume = annualConsumption - thisVolumeFrom;
                    }
                    else
                    {
                        continue;
                    }

                    // Calculate the annual product price based on the billing method
                    if (isFixBilling)
                    {
                        double averageDailyPricesSum = 0.0;
                        double productMatrixRowAmountSum = 0.0;
                        for (int h = thisHoursFrom; h < thisHoursTo; h++)
                        {
                            averageDailyPricesSum += elspotProfile[h];
                            productMatrixRowAmountSum += thisAmount;
                        }
                        double averageDailyPricesAverage = averageDailyPricesSum / 24 / 7;
                        double productMatrixRowAmountAverage = productMatrixRowAmountSum / 24 / 7;
                        annualProductPrice += (productMatrixRowAmountAverage + averageDailyPricesAverage) * volume;
                    }
                    else
                    {
                        for (int hour = thisHoursFrom; hour < thisHoursTo; hour++)
                        {
                            double price = (thisAmount + elspotProfile[hour]) * volume * consumptionProfilePercentage[day][hour] / 100;
                            price = Math.Round(price);
                            annualProductPrice += Math.Round(price);
                        }
                    }
                }
            }

            return annualProductPrice;
        }

        public static double CalculatePriceFactor(ProductPriceDetails p1, ProductPriceDetails p2, int deliveryDays, int restDays, double annualConsumption) =>
            ((p1.TotalPrice + p1.TotalTax) * deliveryDays / 180 + (p2.TotalPrice + p2.TotalTax) * restDays / 180) / annualConsumption;

        public static double CalculateSubscriptionFactor(ProductPriceDetails p1, ProductPriceDetails p2, int deliveryDays, int restDays, double annualConsumption) =>
            ((p1.ProductSubscription * deliveryDays / 180 + p2.ProductSubscription * restDays / 180) / annualConsumption);

        public static double CalculateFeeFactor(ProductPriceDetails p1, ProductPriceDetails p2, int deliveryDays, int restDays, double annualConsumption) =>
            ((p1.ProductFee * deliveryDays / 180 + p2.ProductFee * restDays / 180) / annualConsumption);

        public static double CalculateTariffFactor(ProductPriceDetails p1, ProductPriceDetails p2, int deliveryDays, int restDays, double annualConsumption, Func<ProductPriceDetails, ProductPriceDetails, double> tariff1, Func<ProductPriceDetails, ProductPriceDetails, double> tariff2) =>
            (tariff1(p1, p2) * deliveryDays / 180 + tariff2(p1, p2) * restDays / 180) / annualConsumption;

        public static NetworkFactors CalculateNetworkFactors(ProductPriceDetails p1, ProductPriceDetails p2, int deliveryDays, int restDays, double annualConsumption)
        {
            var networkFactors = new NetworkFactors();

            // Handle network tariffs if not included
            if (!p1.IsNetworkTariffIncluded)
            {
                networkFactors.Distribution = p1.DistributionTariff * deliveryDays / 180 / annualConsumption;
                networkFactors.Subscription = p1.DistributionSubscription * deliveryDays / 180 / annualConsumption;
                networkFactors.Fee = p1.DistributionFee * deliveryDays / 180 / annualConsumption;
                networkFactors.Transmission = p1.TransmissionTariff * deliveryDays / 180 / annualConsumption;
                networkFactors.System = p1.SystemTariff * deliveryDays / 180 / annualConsumption;
            }

            if (!p2.IsNetworkTariffIncluded)
            {
                networkFactors.Distribution += p2.DistributionTariff * restDays / 180 / annualConsumption;
                networkFactors.Subscription += p2.DistributionSubscription * restDays / 180 / annualConsumption;
                networkFactors.Fee += p2.DistributionFee * restDays / 180 / annualConsumption;
                networkFactors.Transmission += p2.TransmissionTariff * restDays / 180 / annualConsumption;
                networkFactors.System += p2.SystemTariff * restDays / 180 / annualConsumption;
            }

            return networkFactors;
        }

        public static ProductDetails CreateProductDetails(Product product, bool productIsExpiring, PriceFactors priceFactors, NetworkFactors networkFactors, bool calculateTax)
        {
            // Jeg mister noget værdifuldt data her.

            var calculatedPrice = new ProductDetails
            {
                Supplier = product.Supplier.Name,
                Name = product.Name,
                OrderUrl = product.OrderUrl,
                ProductIsExpiring = productIsExpiring,
                TotalPriceSortable = priceFactors.Price,
                TotalPrice = $"{priceFactors.Price:F2} øre/kWh",
                DeliveryDaysPrice = $"{priceFactors.Price / 100 / 2:F2} kr",  // Assuming this is the correct conversion
                SupplierPriceIncludingTax = $"{priceFactors.Price * 1.25:F2} øre",
                SupplierPrice = $"{priceFactors.Price:F2} øre",
                GridCompanyPrice = $"{networkFactors.Distribution + networkFactors.Transmission + networkFactors.System:F2} øre",
                StatePrice = $"{priceFactors.FeeEl + networkFactors.Subscription:F2} øre",
                Product = product,
                DropDownIcon = Icons.Material.Filled.ArrowDropUp,
                Fee = $"{networkFactors.Fee}",
                TransmissionTariff = $"{networkFactors.Transmission}",
                SystemTariff = $"{networkFactors.System}"
            };

            // Add prices for electricity, transport, and taxes
            AddPriceDetail(calculatedPrice, "Betaling for el", priceFactors.Price, "elBetaling", priceFactors);
            AddPriceDetail(calculatedPrice, "Betaling for transport", networkFactors.Distribution + networkFactors.Transmission + networkFactors.System, "elTransport", networkFactors);
            AddPriceDetail(calculatedPrice, "Afgifter", priceFactors.FeeEl + networkFactors.Subscription, "elGebyrer", priceFactors);

            if (calculateTax)
            {
                calculatedPrice.Prices.Add(new PriceDetail
                {
                    Name = "Moms (25,00 pct.)",
                    AmountText = $"{priceFactors.Price / 100:F2} øre/kWh",
                    TooltipKey = "elMoms"
                });
            }

            return calculatedPrice;
        }

        // this is doomed. :) jeg må lige ligge hovedet i blød her... dynamic giver ingen mening at bruge tror jeg.
        public static void AddPriceDetail(ProductDetails calculatedPrice, string name, double price, string tooltipKey, dynamic subPrices)
        {
            var priceDetails = new List<SubPrice>
            {
                new SubPrice { Id = 1, Name = name, AmountText = $"{price:F2} øre/kWh", TooltipKey = tooltipKey }
            };

            calculatedPrice.Prices.Add(new PriceDetail
            {
                Name = name,
                AmountText = $"{price:F2} øre/kWh",
                SubPrices = priceDetails,
                TooltipKey = tooltipKey
            });
        }

        public static double CalculateAnnualProductFee(Product product)
        {
            double annualProductFee = 0;

            foreach (var fee in product.Fees)
            {
                // Skip specific fees based on TooltipKey
                if (fee.TooltipKey != "feeSupplierChange" && fee.TooltipKey != "feeProductChange")
                {
                    annualProductFee += fee.Amount;
                }
            }

            return annualProductFee;
        }

        public static double CalculateAnnualProductFee(NextProduct product)
        {
            double annualProductFee = 0;

            foreach (var fee in product.Fees)
            {
                // Skip specific fees based on TooltipKey
                if (fee.TooltipKey != "feeSupplierChange" && fee.TooltipKey != "feeProductChange")
                {
                    annualProductFee += fee.Amount;
                }
            }

            return annualProductFee;
        }

        public static void CalculateNetworkTariffDistributionCharges(
            bool isNetworkTariffIncluded,
            double annualConsumption,
            double[][] consumptionProfilePercentage,
            DistributionAreaChargeData distributionAreaCharge,
            out double monthlyDistributionSubscription,
            out double annualDistributionTariff,
            out double annualDistributionFee,
            out double distributionChargeAreaTariff)
        {
            monthlyDistributionSubscription = 0;
            annualDistributionTariff = 0;
            annualDistributionFee = 0;
            distributionChargeAreaTariff = 0;
            bool networkTariffFlexExists = false;

            if (!isNetworkTariffIncluded)
            {
                // Handle flex charges and tariff calculations
                foreach (var charge in distributionAreaCharge.DistributionAreaCharges)
                {
                    if (charge.BillingType == "flex" && charge.ChargeType == "tariff")
                    {
                        networkTariffFlexExists = true;
                        foreach (var chargeHour in charge.DistributionAreaChargeHours)
                        {
                            for (int day = 1; day <= 7; day++)
                            {
                                for (int h = chargeHour.HoursFrom; h < chargeHour.HoursTo; h++)
                                {
                                    distributionChargeAreaTariff += (annualConsumption * consumptionProfilePercentage[day][h] / 100 * chargeHour.Amount);
                                }
                            }
                        }
                    }
                }
            }

            // Further calculations based on whether flex exists
            if (!isNetworkTariffIncluded)
            {
                if (networkTariffFlexExists)
                {
                    foreach (var charge in distributionAreaCharge.DistributionAreaCharges)
                    {
                        if (charge.BillingType == "flex")
                        {
                            if (charge.ChargeType == "subscription")
                            {
                                monthlyDistributionSubscription += charge.Price;
                            }
                            else if (charge.ChargeType == "tariff")
                            {
                                annualDistributionTariff = distributionChargeAreaTariff;
                            }
                            else
                            {
                                annualDistributionFee += charge.Price * annualConsumption;
                            }
                        }
                    }
                }
                else
                {
                    foreach (var charge in distributionAreaCharge.DistributionAreaCharges)
                    {
                        if (charge.BillingType == "fix")
                        {
                            if (charge.ChargeType == "subscription")
                            {
                                monthlyDistributionSubscription += charge.Price;
                            }
                            else if (charge.ChargeType == "tariff")
                            {
                                annualDistributionTariff += charge.Price * annualConsumption;
                            }
                            else
                            {
                                annualDistributionFee += charge.Price * annualConsumption;
                            }
                        }
                    }
                }
            }
        }

        public static void ProcessNationalCharges(List<NationalTariff> nationalTariffs,
            out double transmissionTariff,
            out double systemTariff,
            out double elAfgift)
        {
            // Initialize the values
            transmissionTariff = 0;
            systemTariff = 0;
            elAfgift = 0;

            foreach (var tariff in nationalTariffs)
            {
                switch (tariff.Type)
                {
                    case NationalTariffType.TransmissionsNettarif:
                        transmissionTariff = tariff.Price;
                        break;
                    case NationalTariffType.Systemtarif:
                        systemTariff = tariff.Price;
                        break;
                    case NationalTariffType.Elafgift:
                        elAfgift = tariff.Price;
                        break;
                }
            }
        }

        public static double CalculateSupplierPrice(double basePrice, double subscription, double fee)
        {
            return basePrice + 100 * (subscription + fee);
        }

        public static double CalculateGridCompanyPrice(double systemTariff, double distributionTariff, double transmissionTariff, double distributionSubscription, double distributionFee)
        {
            return 100 * (systemTariff + distributionTariff + transmissionTariff + distributionSubscription + distributionFee);
        }

        public static double CalculateStatePrice(double elAfgift)
        {
            return 100 * elAfgift;
        }

        // Method to calculate tariffs
        public static double CalculateAnnualTariff(double tariff, double consumption)
        {
            return tariff * consumption;
        }

        // Method to calculate ElAfgift for electrical heating
        public static void CalculateElAfgift(bool electricalHeating, double annualConsumption, double elAfgift, out double annualElAfgift)
        {
            if (electricalHeating)
            {
                if (annualConsumption > 4000)
                {
                    annualElAfgift = elAfgift * 4000;
                }
                else
                {
                    annualElAfgift = elAfgift * annualConsumption;
                }
            }
            else
            {
                annualElAfgift = elAfgift * annualConsumption;
            }
        }

        // Method to calculate monthly subscriptions
        public static double CalculateMonthlySubscription(double monthlyProductSubscription, double monthlyDistributionSubscription, bool isNetworkTariffIncluded, bool calculateTax)
        {
            if (calculateTax)
            {
                monthlyProductSubscription *= 1.25;
                monthlyDistributionSubscription *= 1.25;
            }

            return monthlyProductSubscription + (isNetworkTariffIncluded ? 0.00 : monthlyDistributionSubscription);
        }

        // Method to format the prices with tax or not
        public static string FormatPrice(double price, bool calculateTax)
        {
            return price + " kr. " + (calculateTax ? " (inkl. moms)" : " (ekskl. moms)");
        }

        public static bool Between(double x, double? min, double? max)
            => (min ?? x) <= x && x <= (max ?? x);

        public static ProductDetails GetProductDetails(
            double annualConsumption,
            bool calculateTax,
            ConsumptionProfile consumptionProfile,
            Product product,
            NextProduct nextProduct,
            DistributionAreaChargeData distributionAreaCharge,
            List<AverageElspotPrice> averageDailyPrices,
            List<NationalTariff> nationalTariffs)
        {
            // User input or default values
            bool electricalHeating = true;
            int deliveryDays = Math.Min(product.DeliveryPeriodDays, 180);
            bool productIsExpiring = product.DeliveryPeriodDays < 180 && nextProduct == null;

            // Product price details calculations
            var productPriceDetails1 = ProductUtilities.CalculatePriceDetail(deliveryDays, calculateTax, annualConsumption, electricalHeating, consumptionProfile, product, distributionAreaCharge, averageDailyPrices, nationalTariffs);
            if (productPriceDetails1 == null) return null;

            var restDays = 0;
            ProductPriceDetails productPriceDetails2 = new();
            if (product.DeliveryPeriodDays < 180 && nextProduct != null)
            {
                restDays = 180 - deliveryDays;
                productPriceDetails2 = ProductUtilities.CalculatePriceDetail(restDays, calculateTax, annualConsumption, electricalHeating, consumptionProfile, nextProduct, distributionAreaCharge, averageDailyPrices, nationalTariffs);
                if (productPriceDetails2 == null)
                {
                    return null;
                }
            }
            else if (product.DeliveryPeriodDays < 180 && nextProduct == null)
            {
                //Product is expiring within 180 days. Postpone so we can calculate a more accurate price
                deliveryDays = 180;
                productIsExpiring = true;
            }

            // Beregninger
            var totalIncludingTaxPerKWH = ((productPriceDetails1.TotalPrice + productPriceDetails1.TotalTax) * deliveryDays / 180
                                          + (productPriceDetails2.TotalPrice + productPriceDetails2.TotalTax) * restDays / 180) / annualConsumption;

            var totalIncludingTax = ((productPriceDetails1.TotalPrice + productPriceDetails1.TotalTax) * deliveryDays / 180
                                    + (productPriceDetails2.TotalPrice + productPriceDetails2.TotalTax) * restDays / 180) / 100 / 2; // vist i kr, beløb for 6 måneder

            var totalTax = (productPriceDetails1.TotalTax * deliveryDays / 180
                           + productPriceDetails2.TotalTax * restDays / 180) / annualConsumption;

            var supplier = (productPriceDetails1.SupplierPrice * deliveryDays / 180
                           + productPriceDetails2.SupplierPrice * restDays / 180) / annualConsumption;

            var supplierIncludingTax = supplier * 1.25;

            var gridCompany = (productPriceDetails1.GridCompanyPrice * deliveryDays / 180
                              + productPriceDetails2.GridCompanyPrice * restDays / 180) / annualConsumption;

            var state = (productPriceDetails1.StatePrice * deliveryDays / 180
                        + productPriceDetails2.StatePrice * restDays / 180) / annualConsumption;

            var productDetails = new ProductDetails
            {
                Supplier = product.Supplier.Name,
                Name = product.Name,
                OrderUrl = product.OrderUrl,
                ProductIsExpiring = productIsExpiring,
                TotalPriceSortable = totalIncludingTaxPerKWH.ToTwoDecimalPlaces(),
                TotalPrice = $"{totalIncludingTaxPerKWH.ToTwoDecimalPlaces()} øre/kWh",
                DeliveryDaysPrice = $"{totalIncludingTax.ToTwoDecimalPlaces()} kr",
                SupplierPriceIncludingTax = $"{supplierIncludingTax.ToTwoDecimalPlaces()} øre",
                SupplierPrice = $"{supplier.ToTwoDecimalPlaces()} øre",
                GridCompanyPrice = $"{gridCompany.ToTwoDecimalPlaces()} øre",
                StatePrice = $"{state.ToTwoDecimalPlaces()} øre",
                DropDownIcon = Icons.Material.Filled.ArrowDropUp,
                Product = product
            };

            if (productPriceDetails1.IsNetworkTariffIncluded || productPriceDetails2.IsNetworkTariffIncluded)
            {
                productDetails.Prices.Add(new PriceDetail
                {
                    Name = "* Priserne for el inkluderer betaling for transport",
                    AmountText = null
                });
            }

            var elPrice = (productPriceDetails1.ProductPriceAmount * deliveryDays / 180 + productPriceDetails2.ProductPriceAmount * restDays / 180) / annualConsumption;
            var elSubscription = (productPriceDetails1.ProductSubscription * deliveryDays / 180 + productPriceDetails2.ProductSubscription * restDays / 180) / annualConsumption;
            var elFee = (productPriceDetails1.ProductFee * deliveryDays / 180 + productPriceDetails2.ProductFee * restDays / 180) / annualConsumption;

            productDetails.Prices.Add(new PriceDetail
            {
                Name = "Betaling for el",
                AmountText = $"{supplier.ToTwoDecimalPlaces()} øre/kWh",
                SubPrices = new List<SubPrice>
                {
                    new SubPrice { Id = 1, Name = "El", AmountText = $"{elPrice.ToTwoDecimalPlaces()} øre/kWh", TooltipKey = "el" },
                    new SubPrice { Id = 2, Name = "El (abonnement)", AmountText = $"{elSubscription.ToTwoDecimalPlaces()} øre/kWh", TooltipKey = "elSubscription" },
                    new SubPrice { Id = 3, Name = "El (gebyr)", AmountText = $"{elFee.ToTwoDecimalPlaces()} øre/kWh", TooltipKey = "elFee" }
                }
            });

            var tariffDistribution = new
            {
                Distribution = (productPriceDetails1.DistributionTariff * deliveryDays / 180 + productPriceDetails2.DistributionTariff * restDays / 180) / annualConsumption,
                Transmission = (productPriceDetails1.TransmissionTariff * deliveryDays / 180 + productPriceDetails2.TransmissionTariff * restDays / 180) / annualConsumption,
                System = (productPriceDetails1.SystemTariff * deliveryDays / 180 + productPriceDetails2.SystemTariff * restDays / 180) / annualConsumption,
                Subscription = (productPriceDetails1.DistributionSubscription * deliveryDays / 180 + productPriceDetails2.DistributionSubscription * restDays / 180) / annualConsumption
            };

            var deliveryPrice = tariffDistribution.Distribution + tariffDistribution.Transmission + tariffDistribution.System;

            productDetails.Prices.Add(new PriceDetail
            {
                Name = "Betaling for transport",
                AmountText = $"{gridCompany.ToTwoDecimalPlaces()} øre/kWh",
                SubPrices = new List<SubPrice>
                {
                    new SubPrice
                    {
                        Id = 1,
                        Name = "El-transport",
                        AmountText = $"{deliveryPrice.ToTwoDecimalPlaces()} øre/kWh",
                        TooltipKey = "elTariffs"
                    },
                    new SubPrice
                    {
                        Id = 2,
                        Name = "  El-transport (abonnement)",
                        AmountText = $"{tariffDistribution.Subscription.ToTwoDecimalPlaces()} øre/kWh",
                        TooltipKey = "elDistributionSubscription"
                    }
                }
            });

            var feeEl = (productPriceDetails1.ElAfgift * deliveryDays / 180 + productPriceDetails2.ElAfgift * restDays / 180) / annualConsumption;

            var subPrices = new List<SubPrice>
            {
                new SubPrice { Id = 4, Name = "Beregnet el-afgift", AmountText = $"{feeEl.ToTwoDecimalPlaces()} øre/kWh", TooltipKey = "elStateFee" }
            };

            productDetails.Prices.Add(new PriceDetail
            {
                Name = "Afgifter",
                AmountText = $"{state.ToTwoDecimalPlaces()} øre/kWh",
                SubPrices = subPrices
            });

            if (calculateTax)
            {
                productDetails.Prices.Add(new PriceDetail
                {
                    Name = "Moms (25,00 pct.)",
                    AmountText = $"{totalTax.ToTwoDecimalPlaces()} øre/kWh",
                    TooltipKey = "elMoms"
                });
            }

            return productDetails;
        }
    }
}
