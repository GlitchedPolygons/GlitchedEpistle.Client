/*
    Glitched Epistle - Client
    Copyright (C) 2019  Raphael Beck

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

#region
using System;
using System.Net;
using System.Threading.Tasks;

using GlitchedPolygons.GlitchedEpistle.Client.Models;
using GlitchedPolygons.GlitchedEpistle.Client.Utilities;

using Newtonsoft.Json;

using RestSharp;
#endregion

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Coupons
{
    /// <summary>
    /// Service class for redeeming subscription coupons.
    /// </summary>
    public class CouponService : EpistleWebApiService, ICouponService
    {
        private readonly RestClient restClient = new RestClient(UrlUtility.EpistleAPI_v1);

        /// <summary>
        /// Redeems a coupon code to extend a user's Epistle account membership.
        /// </summary>
        /// <param name="requestBody">Request body containing the coupon redeeming parameters (auth, etc...).</param>
        /// <returns>Whether the coupon code was redeemed successfully or not.</returns>
        public async Task<bool> UseCoupon(EpistleRequestBody requestBody)
        {
            var request = EpistleRequest(requestBody, "coupons/redeem");
            IRestResponse response = await restClient.ExecuteTaskAsync(request);
            return response.StatusCode == HttpStatusCode.OK;
        }
    }
}
