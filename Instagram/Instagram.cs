using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Instagram.Responses;

namespace Instagram
{
    public class Instagram
    {
        private string redirectUri = "http://localhost:50002";
        private string clientId = "563001bc245249ea947e19adc8a9ecf1";
        private string clientSecret = "43676101507a4423ab4b59f6720b9a4f";
        private string accessToken;

        public Instagram(string clientId, string clientSecret, string redirectUri)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.redirectUri = redirectUri;
        }

        public async Task Authorize()
        {
            await RetrieveAccessToken(await RetrieveCode());
        }

        private async Task<string> RetrieveCode()
        {
            using (var httpListener = new HttpListener()
            {
                Prefixes = { redirectUri + "/" }
            })
            {
                httpListener.Start();
                var browserProcess = Process.Start($"https://api.instagram.com/oauth/authorize/?client_id={clientId}&redirect_uri={redirectUri}&response_type=code");

                var context = await httpListener.GetContextAsync();
                var code = context.Request.QueryString["code"];
                context.Response.Redirect("https://instagram.com");
                context.Response.Close();
                return code;
            }
        }

        private async Task RetrieveAccessToken(string code)
        {
            using (var httpClient = new HttpClient())
            {
                var tokenResponse = await httpClient.PostAsync("https://api.instagram.com/oauth/access_token", new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["client_id"] = clientId,
                    ["client_secret"] = clientSecret,
                    ["grant_type"] = "authorization_code",
                    ["redirect_uri"] = redirectUri,
                    ["code"] = code
                }));

                var deserializedResponse = JsonConvert.DeserializeObject<TokenResponse>(await tokenResponse.Content.ReadAsStringAsync());
                accessToken = deserializedResponse.AccessToken;
            }
        }

        public async Task<IEnumerable<string>> GetLatestMedia(int count = 20)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync($"https://api.instagram.com/v1/users/self/media/recent/?access_token={accessToken}&count={count}");
            var res = JsonConvert.DeserializeObject<MediaResponse>(response);
            return res.Data.Select(item => item.Link);
        }
    }
}
