using Enceladus.Api.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Enceladus.Api.Models.Prices.Intrinio
{
    public class IntrinioSecurity
    {
        public string Ticker { get; set; }
        public string Figi_Ticker { get; set; }
        public string Figi { get; set; }
        public string Composite_Figi { get; set; }
        public string Composite_Figi_Ticker { get; set; }
        public string Security_Name { get; set; }
        public string Market_Sector { get; set; }
        public string Security_Type { get; set; }
        public string Stock_Exchange { get; set; }
        public string Last_Crsp_Adj_Date { get; set; }
        public string Figi_UniqueId { get; set; }
        public string Share_Class_Figi { get; set; }
        public string Figi_Exch_Cntry { get; set; }
        public string Currency { get; set; }
        public string Mic { get; set; }
        public string Exch_Symbol { get; set; }
        public bool Etf { get; set; }
        public bool Delisted_Security { get; set; }
        public bool Primary_Listing { get; set; }
        public List<PriceItem> Prices { get; set; }
    }
    public class IntrinioPriceReturnObject
    {
        public int Result_Count { get; set; }
        public int Page_Size { get; set; }
        public int Current_Page { get; set; }
        public int Total_Pages { get; set; }
        public int Api_Call_Credits { get; set; }
        public ICollection<PriceItem> Data { get; set; }
    }
    public class PriceItem
    {
        public DateTime Date { get; set; }
        public float Open { get; set; }
        public float High { get; set; }
        public float Low { get; set; }
        public float Close { get; set; }
        public long Volume { get; set; }
        public float Ex_Dividend { get; set; }
        public float Split_Ratio { get; set; }
        public float Adj_Open { get; set; }
        public float Adj_High { get; set; }
        public float Adj_Low { get; set; }
        public float Adj_Close { get; set; }
        public long Adj_Volume { get; set; }

    }

    public static class IntrinioService
    {
        private const string INTRINIO_URI = "https://api.intrinio.com";
        private const string INTRINIO_USERNAME = "8f10dc72c5fe3b7a5e41b28b247264b7";
        private const string INTRINIO_PWD = "5617ef50dd2b9fec7d4d1188f4bd6b75";
        public static async Task<IntrinioSecurity> GetTickerDetails(string ticker)
        {
            var uri = INTRINIO_URI + "/securities?identifier=" + ticker;
            var response = await HttpClientHelper.Get(uri, AuthenticationType.Basic, INTRINIO_USERNAME, INTRINIO_PWD);
            return JsonConvert.DeserializeObject<IntrinioSecurity>(response.Content.ReadAsStringAsync().Result);
        }
        public static async Task<ICollection<PriceItem>> GetTickerPrices(string ticker, DateTime? startDate = null, DateTime? endDate = null)
        {
            var uri = INTRINIO_URI + "/prices?identifier=" + ticker;

            if (startDate.HasValue)
                uri += "&start_date=" + startDate.Value.ToString("yyyy-MM-dd");
            if (endDate.HasValue)
                uri += "&end_date=" + endDate.Value.ToString("yyyy-MM-dd");

            IntrinioPriceReturnObject obj;
            var currentPage = 1;
            var Prices = new List<PriceItem>(10000);
            do
            {
                var response = await HttpClientHelper.Get(uri + "&page_number=" + currentPage, AuthenticationType.Basic, INTRINIO_USERNAME, INTRINIO_PWD);
                obj = JsonConvert.DeserializeObject<IntrinioPriceReturnObject>(response.Content.ReadAsStringAsync().Result);
                Prices.AddRange(obj.Data);
                currentPage = obj.Current_Page + 1;
                Thread.Sleep(1000);
            }
            while (currentPage <= obj.Total_Pages);
            return Prices;
        }
    }
}
