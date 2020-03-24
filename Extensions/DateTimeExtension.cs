using System;
using System.Runtime.CompilerServices;
namespace PicScapeAPI.Extensions
{
    public static class DateTimeExtension
    {
        public static string ToSwiftString(this DateTime date)
        {
            var resultString = date.ToString();
            return resultString;
        }

        public static DateTime ToDateTime(this string date)
        {
            var returnDate = new DateTime();
            if(DateTime.TryParse(date, out returnDate) == false)
                return new DateTime();
            return returnDate;
        } 
    }
}