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
using System.Collections.Generic;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Users
{
    /// <summary>
    /// Service interface for getting and uploading user profile pictures.
    /// </summary>
    public interface IProfilePictureChanger
    {
        /// <summary>
        /// Gets one or more user's profile picture from the server.
        /// </summary>
        /// <param name="userIds">The userIds whose profile picture you want to retrieve (comma-separated if more than one; should contain NO whitespaces).</param>
        /// <returns>The deserialized JSON of userId-profilePictureBase64 tuples. E.g.: { "userId1": "base64pic1", "userId2": "base64pic2" }</returns>
        Task<IDictionary<string, string>> GetUserProfilePictures(string userIds);

        /// <summary>
        /// Uploads a new profile picture to the server. 
        /// </summary>
        /// <param name="totp">2FA Token.</param>
        /// <param name="pic">The profile picture (base64-encoded). Can be <c>null</c>.</param>
        /// <returns>Whether the update was accepted or failed.</returns>
        Task<bool> UpdateUserProfilePicture(string totp, string pic);
    }
}