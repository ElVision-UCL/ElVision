using ElVisionLibrary.Models.ElPris;
using MudBlazor;
using System.Runtime.CompilerServices;

namespace ElVision.Extensions
{
    public static class ProductDetailsExtensions
    {
        // Modify the list in-place (no reassigning)
        public static void FilterListBySearch(this List<ProductDetails> productDetailsList, string searchString)
        {
            var filteredList = productDetailsList.Where(element =>
            {
                if (string.IsNullOrWhiteSpace(searchString))
                    return true;
                if (element.Supplier.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;
                if (element.TotalPrice.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;
                if (element.Prices[0].AmountText.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;
                if (element.Prices[1].AmountText.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;
                if (element.SupplierPrice.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;
                return false;
            }).ToList();

            // Modify the original list
            productDetailsList.Clear();
            productDetailsList.AddRange(filteredList);
        }

        // Modify the list in-place (no reassigning)
        public static void SortListByColumn(this List<ProductDetails> productDetailsList, TableState state)
        {
            // Sorting based on state.SortDirection (ascending/descending)
            switch (state.SortLabel)
            {
                case "supplier_field":
                    productDetailsList.Sort((x, y) =>
                        state.SortDirection == SortDirection.Ascending
                            ? string.Compare(x.Supplier, y.Supplier, StringComparison.OrdinalIgnoreCase)
                            : string.Compare(y.Supplier, x.Supplier, StringComparison.OrdinalIgnoreCase));
                    break;

                case "price_field":
                    productDetailsList.Sort((x, y) =>
                        state.SortDirection == SortDirection.Ascending
                            ? x.TotalPriceSortable.CompareTo(y.TotalPriceSortable)
                            : y.TotalPriceSortable.CompareTo(x.TotalPriceSortable));
                    break;

                case "type_field":
                    productDetailsList.Sort((x, y) =>
                        state.SortDirection == SortDirection.Ascending
                            ? string.Compare(x.Product.ProductInformation.PriceTypeName, y.Product.ProductInformation.PriceTypeName, StringComparison.OrdinalIgnoreCase)
                            : string.Compare(y.Product.ProductInformation.PriceTypeName, x.Product.ProductInformation.PriceTypeName, StringComparison.OrdinalIgnoreCase));
                    break;

                case "binding_field":
                    productDetailsList.Sort((x, y) =>
                        state.SortDirection == SortDirection.Ascending
                            ? x.Product.BindingPeriod.CompareTo(y.Product.BindingPeriod)
                            : y.Product.BindingPeriod.CompareTo(x.Product.BindingPeriod));
                    break;
            }
        }
    }
}
