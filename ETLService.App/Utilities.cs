using System;

namespace ETLService.App
{
    public static class Utilities
    {
        public static DateTime ToDateTime(this string value)
        {
            DateTime outDt = DateTime.MinValue;
            DateTime.TryParse(value, out outDt);
            return outDt;
        }

        public static string GetRandomLetter()
        {
            int num = new Random(Guid.NewGuid().GetHashCode()).Next(0, 26);
            char let = (char)('a' + num);
            return let.ToString().ToUpper();
        }

        public static int GetRandomNumber()
        {
            return new Random(Guid.NewGuid().GetHashCode()).Next(01, 99);
        }
    }
}
