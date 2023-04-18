using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace PricePredictML.Livedata
{
    public class ApiHelper
    {
        public static HttpClient Apiclient { get; set; }
        public static void InitializeClient()
        {
            Apiclient = new HttpClient();
            Apiclient.DefaultRequestHeaders.Accept.Clear();
            Apiclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
