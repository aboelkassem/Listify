using Listify.DAL;
using Listify.Lib.Requests;
using Listify.Lib.Responses;
using Listify.Paths;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Listify.Services
{
    public class ListifyService : IListifyService
    {
        private readonly IListifyDAL _dal;

        public ListifyService(IListifyDAL dal)
        {
            _dal = dal;
        }

        public virtual async Task<bool> IsContentValid(string content)
        {
            try
            {
                var response = await GetFromNeutrinoAsync(content);
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
                var response = await GetFromPurgoMalumAsync(content);

                return response.Result;
                //var response = await GetToNeutrinoAsync(content);

                //return !response.Isbad ? content : response.Censoredcontent;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return string.Empty;
        }
        public virtual async Task<bool> ExcecutePaypalTransaction(PurchaseCreateRequest purchase)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var request = new PaypalPurchaseRequest
                    {
                        payer_id = purchase.PayerId,
                    };

                    var accessToken = await GetPaypalAccessTokenAsync();

                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                    var response = await httpClient.PostAsync($"https://api.sandbox.paypal.com/v1/payments/payment/{purchase.PaymentId}/execute",
                        new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

                    var content = await response.Content.ReadAsStringAsync();
                    var deserializedContent = JsonConvert.DeserializeObject<PaypalPurchaseResponse>(content);

                    return deserializedContent.state.Trim().ToLower() == "approved" ? true : false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public virtual async Task<PaypalPaymentCreateResponse> CreatePaypalPayment(PaypalPaymentCreateRequest request)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var accessToken = await GetPaypalAccessTokenAsync();

                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                    httpClient.DefaultRequestHeaders
                      .Accept
                      .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

                    var response = await httpClient.PostAsync("https://api.sandbox.paypal.com/v1/payments/payment",
                        new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

                    var content = await response.Content.ReadAsStringAsync();
                    var deserializedContent = JsonConvert.DeserializeObject<PaypalPaymentCreateResponse>(content);

                    return deserializedContent;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        // Two Api bad word filter 
        private async Task<NeutrinoBadWordResponse> GetFromNeutrinoAsync(string content)
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
        private async Task<PurgoMalumResponse> GetFromPurgoMalumAsync(string content)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"{Globals.PURGOMALUM_FILTER_URL}?text={content}");

                    var serializedResponse = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<PurgoMalumResponse>(serializedResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        private async Task<string> GetPaypalAccessTokenAsync()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                    httpClient.DefaultRequestHeaders.Add("Accept-Language", "en_US");

                    var authTokenBase64String = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{Globals.PAYPAL_SANDBOX_CLIENT_ID}:{Globals.PAYPAL_SANDBOX_SECRET}"));

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authTokenBase64String);

                    var req = new List<KeyValuePair<string, string>>();
                    req.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));

                    var contentEncoded = new FormUrlEncodedContent(req);

                    var response = await httpClient.PostAsync("https://api.sandbox.paypal.com/v1/oauth2/token", contentEncoded);

                    var content = await response.Content.ReadAsStringAsync();
                    var deserializedContent = JsonConvert.DeserializeObject<PaypalAccessTokenResponse>(content);
                    return deserializedContent.access_token;
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