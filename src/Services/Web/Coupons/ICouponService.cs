#region
using System.Threading.Tasks;

using GlitchedPolygons.GlitchedEpistle.Client.Models;
#endregion

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Coupons
{
    /// <summary>
    /// Service interface for redeeming subscription coupons.
    /// </summary>
    public interface ICouponService
    {
        /// <summary>
        /// Redeems a coupon code to extend a user's Epistle account membership.
        /// </summary>
        /// <param name="requestBody">Request body containing the coupon redeeming parameters (auth, etc...).</param>
        /// <returns>Whether the coupon code was redeemed successfully or not.</returns>
        Task<bool> UseCoupon(EpistleRequestBody requestBody);
    }
}
