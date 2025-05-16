using ElVisionLibrary.Models.ElPris;
using Newtonsoft.Json;

namespace ElVision.Utilities
{
    public static class GridAreaUtilities
    {
        public static DistributionArea GetDistributionArea(ZipCodeData zipCodeData, int zipCode)
        {
            return zipCodeData.zipCodes
                .FirstOrDefault(z => z.Code == zipCode)?
                .DistributionAreas
                .FirstOrDefault();
        }
    }
}
