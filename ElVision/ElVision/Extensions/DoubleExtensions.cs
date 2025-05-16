namespace ElVision.Extensions
{
    public static class DoubleExtensions
    {
        public static double ToTwoDecimalPlaces(this double value)
        {
            return Math.Round(value, 2);
        }
    }
}
