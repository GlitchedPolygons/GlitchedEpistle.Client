using System;
using System.Net;
using System.Threading.Tasks;

using RestSharp;
using GlitchedPolygons.GlitchedEpistle.Client.Constants;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Coupons
{
    /// <summary>
    /// Service class for redeeming subscription coupons.
    /// </summary>
    public class CouponService : ICouponService
    {
        private readonly RestClient restClient = new RestClient(URLs.EPISTLE_API);

        /// <summary>
        /// Redeems a coupon code to extend a user's Epistle account membership.
        /// </summary>
        /// <param name="code">The coupon code.</param>
        /// <param name="userId">The user identifier to which the coupon should be applied.</param>
        /// <param name="auth">The jwt auth token.</param>
        /// <returns>Whether the coupon code was redeemed successfully or not.</returns>
        public async Task<bool> UseCoupon(string code, string userId, string auth)
        {
            var request = new RestRequest(
                method: Method.PUT,
                resource: new Uri($"coupons/{code}", UriKind.Relative)
            );
            request.AddQueryParameter(nameof(userId), userId);
            request.AddQueryParameter(nameof(auth), auth);

            var response = await restClient.ExecuteTaskAsync(request);
            return response.StatusCode == HttpStatusCode.OK;
        }
    }
}
