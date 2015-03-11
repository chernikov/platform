﻿using System.Collections.Generic;

namespace platformAthletic.Tools
{
    public class Translit
    {

        private static Dictionary<string, string> translateTable { get; set; }

        static Translit()
        {
            translateTable = new Dictionary<string, string>();
            translateTable.Add("а", "a");
            translateTable.Add("б", "b");
            translateTable.Add("в", "v");
            translateTable.Add("г", "g");
            translateTable.Add("д", "d");
            translateTable.Add("е", "e");
            translateTable.Add("ё", "yo");
            translateTable.Add("ж", "zh");
            translateTable.Add("з", "z");
            translateTable.Add("и", "i");
            translateTable.Add("й", "j");
            translateTable.Add("к", "k");
            translateTable.Add("л", "l");
            translateTable.Add("м", "m");
            translateTable.Add("н", "n");
            translateTable.Add("о", "o");
            translateTable.Add("п", "p");
            translateTable.Add("р", "r");
            translateTable.Add("с", "s");
            translateTable.Add("т", "t");
            translateTable.Add("у", "u");
            translateTable.Add("ф", "f");
            translateTable.Add("х", "x");
            translateTable.Add("ц", "cz");
            translateTable.Add("ч", "ch");
            translateTable.Add("ш", "sh");
            translateTable.Add("щ", "shh");
            translateTable.Add("ь", "");
            translateTable.Add("ы", "y");
            translateTable.Add("ъ", "");
            translateTable.Add("э", "eh");
            translateTable.Add("ю", "yu");
            translateTable.Add("я", "ya");
        }

        /// <summary>
        /// Cyrillic to latin
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Translate(string source)
        {
            if (source == null)
                return string.Empty;
            var prepared = source.ToLower();

            foreach (var item in translateTable.Keys)
            {
                prepared = prepared.Replace(item, translateTable[item]);
            }
            prepared = prepared.Replace(" ", "-");
            prepared = prepared.Replace(",", "-");
            prepared = prepared.Replace("--", "-");
            return prepared;
        }
    }
}