using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Enceladus.Api.Helpers
{
    public class HttpClientHelper
    {
        internal static async Task<HttpResponseMessage> Get(string Uri, AuthenticationType authType = AuthenticationType.None, string userName = null, string password = null)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(Uri)
            };
            int _TimeoutSec = 90;
            client.Timeout = new TimeSpan(0, 0, _TimeoutSec);
            string _ContentType = "application/json";
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_ContentType));

            byte[] bytes = Encoding.UTF8.GetBytes(userName + ":" + password);
            var uNamePwd64 = Convert.ToBase64String(bytes);

            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse("Basic " + uNamePwd64);
            HttpResponseMessage response = await client.GetAsync(Uri);

            // either this - or check the status to retrieve more information
            response.EnsureSuccessStatusCode();
            //// var obj = "";
            ////  using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(response.Content.ReadAsStringAsync().Result)))
            //// {
            ////     DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Array));
            ////     obj = ser.ReadObject(ms);
            ////  }
            //// return obj;
            ////var content = response.Content.ReadAsStringAsync().Result;

            return response;
        }
        internal static async Task<HttpResponseMessage> Put(string Uri, string Body)
        {
            var cl = new HttpClient();
            cl.BaseAddress = new Uri(Uri);
            int _TimeoutSec = 90;
            cl.Timeout = new TimeSpan(0, 0, _TimeoutSec);
            string _ContentType = "application/json";
            cl.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_ContentType));

            // Construct an HttpContent from a StringContent
            HttpContent _Body = new StringContent(Body);
            // and add the header to this object instance
            // optional: add a formatter option to it as well
            _Body.Headers.ContentType = new MediaTypeHeaderValue(_ContentType);
            // synchronous request without the need for .ContinueWith() or await
            var response = await cl.PutAsync(Uri, _Body);

            // either this - or check the status to retrieve more information
            response.EnsureSuccessStatusCode();
            // var obj = "";
            //  using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(response.Content.ReadAsStringAsync().Result)))
            // {
            //     DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Array));
            //     obj = ser.ReadObject(ms);
            //  }
            // return obj;
            //var content = response.Content.ReadAsStringAsync().Result;

            return response;
        }
        internal static async Task<HttpResponseMessage> Post(string Uri, string Body)
        {
            var cl = new HttpClient
            {
                BaseAddress = new Uri(Uri)
            };
            int _TimeoutSec = 90;
            cl.Timeout = new TimeSpan(0, 0, _TimeoutSec);
            string _ContentType = "application/json";
            cl.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_ContentType));

            // Construct an HttpContent from a StringContent
            var _Body = new StringContent(Body);
            // and add the header to this object instance
            // optional: add a formatter option to it as well
            _Body.Headers.ContentType = new MediaTypeHeaderValue(_ContentType);
            // synchronous request without the need for .ContinueWith() or await
            var response = await cl.PostAsync(Uri, _Body);
            response.EnsureSuccessStatusCode();

            return response;
        }
        internal static async Task<HttpResponseMessage> Delete(string Uri)
        {
            var cl = new HttpClient();
            cl.BaseAddress = new Uri(Uri);
            int _TimeoutSec = 90;
            cl.Timeout = new TimeSpan(0, 0, _TimeoutSec);
            string _ContentType = "application/json";
            cl.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_ContentType));
            HttpResponseMessage response = await cl.DeleteAsync(Uri);
            response.EnsureSuccessStatusCode();

            return response;
        }
    }
    public enum AuthenticationType
    {
        None,
        Basic,
        Token
    }
}
