namespace Application.Utils
{
    public static class Extensions
    {
        public static string NumberString(this Enum enVal)
        {
            return enVal.ToString("D");
        }
    }
}
