using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Hemo.Extensions
{
    public static class OwinRequestExtensions
    {
        /// <summary>
        /// Gets the combined request parameters from the form body, query string, and request headers.
        /// </summary>
        /// <param name="request">Owin request.</param>
        /// <returns>Dictionary of combined form body, query string, and request headers.</returns>
        public static Dictionary<string, string> GetRequestParameters(this IOwinRequest request)
        {
            Dictionary<string,string> bodyParameters = request.GetBodyParameters();
            Dictionary<string, string> queryParameters = request.GetQueryParameters();
            Dictionary<string, string> headerParameters = request.GetHeaderParameters();

            bodyParameters.Merge(queryParameters);

            bodyParameters.Merge(headerParameters);

            return bodyParameters;
        }

        /// <summary>
        /// Gets the query string request parameters.
        /// </summary>
        /// <param name="request">Owin Request.</param>
        /// <returns>Dictionary of query string parameters.</returns>
        public static Dictionary<string, string> GetQueryParameters(this IOwinRequest request)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

            foreach (KeyValuePair<string, string[]> pair in request.Query)
            {
                string value = GetJoinedValue(pair.Value);

                dictionary.Add(pair.Key, value);
            }

            return dictionary;
        }

        /// <summary>
        /// Gets the form body request parameters.
        /// </summary>
        /// <param name="request">Owin Request.</param>
        /// <returns>Dictionary of form body parameters.</returns>
        public static Dictionary<string, string> GetBodyParameters(this IOwinRequest request)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

            Task<IFormCollection> formCollectionTask = request.ReadFormAsync();

            formCollectionTask.Wait();

            foreach (KeyValuePair<string,string[]> pair in formCollectionTask.Result)
            {
                string value = GetJoinedValue(pair.Value);

                dictionary.Add(pair.Key, value);
            }

            return dictionary;
        }

        /// <summary>
        /// Gets the header request parameters.
        /// </summary>
        /// <param name="request">Owin Request.</param>
        /// <returns>Dictionary of header parameters.</returns>
        public static Dictionary<string, string> GetHeaderParameters(this IOwinRequest request)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

            foreach (KeyValuePair<string, string[]> pair in request.Headers)
            {
                string value = GetJoinedValue(pair.Value);

                dictionary.Add(pair.Key, value);
            }

            return dictionary;
        }

        private static string GetJoinedValue(string[] value)
        {
            if (value != null)
                return string.Join(",", value);

            return null;
        }

        public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> to, IDictionary<TKey, TValue> data)
        {
            foreach (KeyValuePair<TKey, TValue> item in data)
            {
                if (to.ContainsKey(item.Key) == false)
                {
                    to.Add(item.Key, item.Value);
                }
            }
        }
    }
}