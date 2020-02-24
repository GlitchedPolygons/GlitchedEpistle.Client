/*
    Glitched Epistle - Client
    Copyright (C) 2020  Raphael Beck

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

using System.Threading.Tasks;
using GlitchedPolygons.GlitchedEpistle.Client.Models;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Users
{
    /// <summary>
    /// User login service.
    /// </summary>
    public interface ILoginService
    {
        /// <summary>
        /// Logs a <see cref="User"/> in and applies the session token + key to the local <see cref="User"/> instance.<para> </para>
        /// The resulting status code has the following meaning:<para> </para>
        /// 0 = Successful login<para> </para>
        /// 1 = Connection to the Epistle server could not be established.<para> </para>
        /// 2 = Login failed (invalid credentials or returned jwt/key).<para> </para>
        /// 3 = Login succeeded server-side, but failed client-side (e.g. response's private key could not be decrypted, auth token could not be attributed to the <see cref="User"/> instance, etc...).
        /// </summary>
        /// <param name="userId">The user login id.</param>
        /// <param name="userPassword">The user's password (NOT its SHA512!).</param>
        /// <param name="totp">The 2-Factor Authentication token.</param>
        /// <returns>The status code (as described in the summary).</returns>
        Task<int> Login(string userId, string userPassword, string totp);
    }
}