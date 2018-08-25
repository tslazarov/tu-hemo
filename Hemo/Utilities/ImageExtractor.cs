using Hemo.Models.Users;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hemo.Utilities
{
    public class ImageExtractor : IImageExtractor
    {
        public async Task<string> GetImageAsBase64Url(string accessToken)
        {
            string userId = await GetUserId(accessToken);
            string imageUrl = await GetImageUrl(userId, accessToken);
            string base64Image = GetImage(imageUrl);

            return base64Image;
        }

        private async Task<string> GetUserId(string accessToken)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.FacebookGraphAPIBaseUrl);

                HttpResponseMessage response = client.GetAsync(Constants.FacebookGraphAPIMeEndpoint + accessToken).Result;

                string content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    FacebookModel contentResponse = JsonConvert.DeserializeObject<FacebookModel>(content);

                    return contentResponse.Id;
                }

                return string.Empty;
            }
        }

        private async Task<string> GetImageUrl(string userId, string accessToken)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.FacebookGraphAPIBaseUrl);

                HttpResponseMessage response = client.GetAsync(userId + "/" + Constants.FacebookGraphAPIPictureEndpoint + accessToken).Result;
                string content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    FacebookPictureModel contentResponse = JsonConvert.DeserializeObject<FacebookPictureModel>(content);

                    return contentResponse.Data.Url;
                }

                return string.Empty;
            }
        }

        private string GetImage(string imageUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                byte[] bytes = client.GetByteArrayAsync(imageUrl).Result;
                return Convert.ToBase64String(bytes);
            }
        }
    }
}