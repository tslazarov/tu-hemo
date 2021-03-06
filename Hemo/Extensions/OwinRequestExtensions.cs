﻿using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hemo.Extensions
{
    public static class OwinRequestExtensions
    {
        public static Dictionary<string, string> GetRequestParameters(this IOwinRequest request)
        {
            Dictionary<string,string> bodyParameters = request.GetBodyParameters();
            Dictionary<string, string> queryParameters = request.GetQueryParameters();
            Dictionary<string, string> headerParameters = request.GetHeaderParameters();

            bodyParameters.Merge(queryParameters);

            bodyParameters.Merge(headerParameters);

            return bodyParameters;
        }

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