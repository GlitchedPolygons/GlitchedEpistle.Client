#region
using System.Threading.Tasks;
#endregion

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Coupons
{
    /// <summary>
    /// Service interface for redeeming subscription coupons.
    /// </summary>
    public interface ICouponService
    {
        /// <summary>
        /// Redeems a coupon code to extend a user's Epistle account membership.
        /// </summary>
        /// <param name="code">The coupon code.</param>
        /// <param name="userId">The user identifier to which the coupon should be applied.</param>
        /// <param name="auth">The jwt auth token.</param>
        /// <returns>Whether the coupon code was redeemed successfully or not.</returns>
        Task<bool> UseCoupon(string code, string userId, string auth);
    }
}
