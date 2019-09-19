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

using System;
using System.Threading.Tasks;
using GlitchedPolygons.GlitchedEpistle.Client.Models;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Users
{
    /// <summary>
    /// Comfortable service interface for submitting user password change requests to an Epistle server backend.
    /// </summary>
    public interface IPasswordChanger
    {
        /// <summary>
        /// Changes an Epistle <see cref="User"/>'s password.
        /// </summary>
        /// <param name="oldPw">The old <see cref="User"/> password (NOT its SHA512!).</param>
        /// <param name="newPw">The new, better and safer password (NOT its SHA512!).</param>
        /// <param name="totp">Request authentication token.</param>
        /// <returns>Whether the password change request was successful or not.</returns>
        /// <exception cref="ApplicationException">Thrown if the <see cref="User.PrivateKeyPem"/> is <c>null</c> or empty.</exception>
        Task<bool> ChangePassword(string oldPw, string newPw, string totp);
    }
}