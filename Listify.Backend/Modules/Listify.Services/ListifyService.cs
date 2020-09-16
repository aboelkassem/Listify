using Listify.Lib.Responses;
using Listify.Paths;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Listify.Services
{
    public class ListifyService : IListifyService
    {
        public virtual async Task<bool> IsContentValid(string content)
        {
            try
            {
                var response = await PostToNeutrinoAsync(content);
                return !response.Isbad;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        public virtual async Task<string> CleanContent(string content)
        {
            try
            {
                var response = await PostToNeutrinoAsync(content);

                return !response.Isbad ? content : response.Censoredcontent;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return string.Empty;
        }

        private async Task<NeutrinoBadWordResponse> PostToNeutrinoAsync(string content)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var req = new List<KeyValuePair<string, string>>();
                    req.Add(new KeyValuePair<string, string>("user-id", Globals.NEUTRINOAPI_BAD_WORD_FILTER_USER_ID));
                    req.Add(new KeyValuePair<string, string>("api-key", Globals.NEUTRINO_API_KEY));
                    req.Add(new KeyValuePair<string, string>("censor-character", "*"));
                    req.Add(new KeyValuePair<string, string>("content", content));

                    var contentEncoded = new FormUrlEncodedContent(req);
                    var response = await httpClient.PostAsync(Globals.NEUTRINOAPI_BAD_WORD_FILTER_URL, contentEncoded);

                    var serializedResponse = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<NeutrinoBadWordResponse>(serializedResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}