#region
using System;
using System.Net;
using System.Threading.Tasks;

using GlitchedPolygons.GlitchedEpistle.Client.Models;
using GlitchedPolygons.GlitchedEpistle.Client.Constants;

using Newtonsoft.Json;

using RestSharp;
#endregion

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Coupons
{
    /// <summary>
    /// Service class for redeeming subscription coupons.
    /// </summary>
    public class CouponService : ICouponService
    {
        private readonly RestClient restClient = new RestClient(URLs.EPISTLE_API_V1);

        /// <summary>
        /// Redeems a coupon code to extend a user's Epistle account membership.
        /// </summary>
        /// <param name="requestBody">Request body containing the coupon redeeming parameters (auth, etc...).</param>
        /// <returns>Whether the coupon code was redeemed successfully or not.</returns>
        public async Task<bool> UseCoupon(EpistleRequestBody requestBody)
        {
            var request = new RestRequest(
                method: Method.POST,
                resource: new Uri("coupons/redeem", UriKind.Relative)
            );

            request.AddParameter("application/json", JsonConvert.SerializeObject(requestBody), ParameterType.RequestBody);

            IRestResponse response = await restClient.ExecuteTaskAsync(request);
            return response.StatusCode == HttpStatusCode.OK;
        }
    }
}
