using newbtc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricePredictML.Livedata
{
    public  class Priceprocessor
    {
        public  async Task<BtcModel> Loadbtcinformation()
        {
            string Url = "https://api.kucoin.com/api/v1/market/stats?symbol=BTC-USDT";
            using (HttpResponseMessage response = await ApiHelper.Apiclient.GetAsync(Url))
            {
                if (response.IsSuccessStatusCode)
                {
                    BtcResultModel result = await response.Content.ReadAsAsync<BtcResultModel>();
                    return result.data;
                }
                else
                    throw new Exception(response.ReasonPhrase);
            }

            
        }
    }
}
