using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiServer
{
    public static class JSONConverter
    {
        public static Dictionary<string, string> FromJson(string json_string)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            string filter_str = json_string.Trim(new char[] { '{', '}', ' ', '\n', '\r', '\t'})
                                           .Replace("\"","")
                                           .Replace(" ","");


            foreach (string pair in filter_str.Split(','))
            {
                result.Add(pair.Split(':')[0], pair.Split(':')[1]);
            }


            return result;
        }

        public static string ToJson(Dictionary<string, string> dict)
        {
            string result = "{";


            foreach (KeyValuePair<string,string> item in dict)
            {
                result += string.Format("\"{0}\": \"{1}\",",item.Key,item.Value);
            }


            return result.TrimEnd(',') + '}';
        }

        public static string ToJson(string[] arr_lst)
        {
            string result = "[";


            foreach (string item in arr_lst)
            {
                result += string.Format("\"{0}\",", item);
            }


            return result.TrimEnd(',') + ']';
        }
    }
}
