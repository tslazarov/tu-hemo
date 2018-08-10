using Hemo.Models.Users;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Hemo.Utilities
{
    public class ImageExtractor : IImageExtractor
    {
        public async Task<string> GetImageAsBase64Url(string accessToken)
        {
            string userId = await GetUserId(accessToken);
            string imageUrl = await GetImageUrl(userId, accessToken);
            string base64Image = await GetImage(imageUrl);

            return base64Image;
        }

        private async Task<string> GetUserId(string accessToken)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.FacebookGraphAPIBaseUrl);

                var response = client.GetAsync(Constants.FacebookGraphAPIMeEndpoint + accessToken).Result;

                var content = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var contentResponse = JsonConvert.DeserializeObject<FacebookModel>(content);

                    return contentResponse.Id;
                }

                return string.Empty;
            }
        }

        private async Task<string> GetImageUrl(string userId, string accessToken)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.FacebookGraphAPIBaseUrl);

                var response = client.GetAsync(userId + "/" + Constants.FacebookGraphAPIPictureEndpoint + accessToken).Result;
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var contentResponse = JsonConvert.DeserializeObject<FacebookPictureModel>(content);

                    return contentResponse.Data.Url;
                }

                return string.Empty;
            }
        }

        private async Task<string> GetImage(string imageUrl)
        {
            using (var client = new HttpClient())
            {
                var bytes = client.GetByteArrayAsync(imageUrl).Result;
                return Convert.ToBase64String(bytes);
            }
        }
    }
}