using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Utilities.HelperExtensions
{
    public static class StringExtensions
    {

        public static string FirstLetterToUpper(this string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }

        public static string ToTitleCase(this string str)
        {
            if (str == null)
                return null;

            string[] title = str.Split(' ');
            string returnStr = "";

            for (int i = 0; i < title.Length; i++)
            {
                returnStr += title[i].FirstLetterToUpper();

                if (i != title.Length)
                    returnStr += " ";
            }

            return returnStr;
        }

    }
}
