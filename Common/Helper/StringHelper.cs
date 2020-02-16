using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Common.Helpers
{
    public class StringHelper
    {
        public static string randomString(int length = 7)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";//0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string toTitleCase(string title){
            return title != null ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower()) : null;
        }
    }
}
