using ElVisionLibrary.Models;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;
using ElVisionLibrary.Models.EnergiDataService;

namespace ElVision.Utilities
{
    public static class TariffUtilities
    {
        public static List<ChargeOwnerTariffForHour> ExtractChargeOwnerTariffForHour(TariffData tariffData)
        {
            List<ChargeOwnerTariffForHour> chargeOwnerTariffForHourList = new();
            foreach (var entry in tariffData.Tariffs)
            {
                if (EntryInRange(entry, DateTime.UtcNow.ToString("yyyy-MM-dd")))
                {
                    double basePrice = entry.Price1;

                    PropertyInfo[] properties = typeof(Tariff).GetProperties();
                    foreach (var property in properties)
                    {
                        if (property.Name.StartsWith("Price"))
                        {
                            int hour = int.Parse(property.Name.Substring(5)) - 1;
                            // Get the value of the property dynamically using reflection
                            object propertyValue = property.GetValue(entry);

                            // Ensure the value is not null, otherwise use base price
                            double currentValue = propertyValue != null
                                ? Convert.ToDouble(propertyValue)
                                : basePrice;

                            var chargeOwnerTariff = new ChargeOwnerTariffForHour
                            {
                                Hour = hour.ToString(),
                                Price = currentValue
                            };
                            // Store the tariff value for the specific hour
                            chargeOwnerTariffForHourList.Add(chargeOwnerTariff);
                        }
                    }
                }
            }

            return chargeOwnerTariffForHourList;
        }

        public static List<NationalTariff> ExtractNationalTariffs(TariffData tariffData)
        {
            List<NationalTariff> nationalTariffs = new();
            foreach (var entry in tariffData.Tariffs)
            {
                if (EntryInRange(entry, DateTime.UtcNow.ToString("yyyy-MM-dd")))
                {
                    switch (entry.Note)
                    {
                        case "Transmissions nettarif":
                            nationalTariffs.Add(new NationalTariff
                            {
                                Type = NationalTariffType.TransmissionsNettarif,
                                Price = entry.Price1
                            });
                            break;
                        case "Elafgift":
                            nationalTariffs.Add(new NationalTariff
                            {
                                Type = NationalTariffType.Elafgift,
                                Price = entry.Price1
                            });
                            break;
                        case "Systemtarif":
                            nationalTariffs.Add(new NationalTariff
                            {
                                Type = NationalTariffType.Systemtarif,
                                Price = entry.Price1
                            });
                            break;
                        default:
                            break;
                    }
                }
            }
            return nationalTariffs;
        }

        private static bool EntryInRange(Tariff entry, string checkDate)
        {
            return entry.ValidFrom <= DateTime.Parse(checkDate) && (entry.ValidTo == null || entry.ValidTo > DateTime.Parse(checkDate));
        }

        private static string Slugify(string value)
        {
            // Normalize the string to decompose special characters
            var normalizedString = value.Normalize(NormalizationForm.FormD);

            // Build the string with ASCII characters only
            var stringBuilder = new StringBuilder();
            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            // Convert to string and remove diacritics
            var slug = stringBuilder.ToString().Normalize(NormalizationForm.FormC);

            // Replace all non-alphanumeric characters and spaces with hyphens
            slug = Regex.Replace(slug, @"[^a-zA-Z0-9\s-]", "");
            slug = Regex.Replace(slug, @"\s+", "-").Trim('-');

            // Convert to lowercase
            return slug.ToLower();
        }
    }
}
