using System;
using System.Threading.Tasks;

using GlitchedPolygons.GlitchedEpistle.Client.Models;
using GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Users
{
    /// <summary>
    /// Service interface implementation for creating new users on the backend.
    /// </summary>
    public interface IRegistrationService
    {
        /// <summary>
        /// Submits a user registration request to the Epistle backend and returns the resulting status code.<para> </para>
        /// If the user creation succeeded, the created user's data is applied to the currently active session <see cref="User"/>.
        /// The meaning of the returned status codes is as follows:<para> </para>
        /// 0 = Success! The user was created and the related data was loaded into session <see cref="User"/>.<para> </para>
        /// 1 = Connection to the Epistle server could not be established.<para> </para>
        /// 2 = RSA Key generation failed/incomplete.<para> </para>
        /// 3 = User registration failed server-side.
        /// 4 = User registration failed client-side.
        /// </summary>
        /// <param name="password">The user's password (NOT the SHA512!)</param>
        /// <param name="userCreationSecret">The backend's user creation secret.</param>
        /// <returns>A tuple containing the resulting status code and (eventually) the <see cref="UserCreationResponseDto"/></returns>
        Task<Tuple<int, UserCreationResponseDto>> CreateUser(string password, string userCreationSecret);
    }
}
