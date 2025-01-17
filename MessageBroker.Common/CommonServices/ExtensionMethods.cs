using System.Web;

namespace MessageBroker.Common.CommonServices
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Convert an Instance of Objects to Query String
        /// </summary>
        /// <param name="obj">Instance of Object</param>
        /// <returns></returns>
        public static string ToQueryString(this object obj)
        {
            List<string> keyValuePairs = [];
            var properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {
                var name = property.Name;
                var value = property.GetValue(obj);
                if (value != null)
                    keyValuePairs.Add($"{HttpUtility.UrlEncode(name)}={HttpUtility.UrlEncode(value.ToString())}");
            }
            return "?" +string.Join("&", keyValuePairs.ToArray());
        }
    }
}
