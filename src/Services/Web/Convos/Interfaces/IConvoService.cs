﻿/*
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

using System;
using System.Threading.Tasks;
using GlitchedPolygons.GlitchedEpistle.Client.Models;
using GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Convos
{
    /// <summary>
    /// Service interface responsible for accessing convos on the web API (remote).
    /// </summary>
    public interface IConvoService : IDisposable
    {
        /// <summary>
        /// Gets the server's maximum convo duration setting value (in days).
        /// </summary>
        /// <returns>The server's preferred maximum convo lifetime. A value below 0 means that there is no maximum convo duration limit on the specified server.</returns>
        Task<int> GetMaximumConvoDurationDays();
        
        /// <summary>
        /// Creates a new convo on the server.<para> </para>
        /// "<paramref name="requestBody.Body"/>" should be the <see cref="ConvoCreationRequestDto"/> serialized into JSON and compressed.
        /// </summary>
        /// <param name="requestBody">Request body containing the parameters (auth, etc...).</param>
        /// <returns><c>null</c> if creation failed; the created <see cref="Convo"/>'s unique id.</returns>
        Task<string> CreateConvo(EpistleRequestBody requestBody);

        /// <summary>
        /// Deletes a convo server-side.
        /// </summary>
        /// <param name="requestBody">Request body containing the convo deletion parameters (auth, etc...).</param>
        /// <returns>Whether deletion was successful or not.</returns>
        Task<bool> DeleteConvo(EpistleRequestBody requestBody);

        /// <summary>
        /// Posts a message to a <see cref="Convo" />.
        /// </summary>
        /// <param name="requestBody">Request body containing the message post parameters.</param>
        /// <returns>Whether the message was posted successfully or not.</returns>
        Task<bool> PostMessage(EpistleRequestBody requestBody);

        /// <summary>
        /// Gets a convo's metadata (description, timestamp, etc...).
        /// </summary>
        /// <param name="convoId">The convo's identifier.</param>
        /// <param name="convoPasswordSHA512">The convo's password hash.</param>
        /// <param name="userId">The user identifier (needs to be a participant of the convo).</param>
        /// <param name="auth">The authentication token.</param>
        /// <returns>The convo's metadata wrapped into a DTO (<c>null</c> if something failed).</returns>
        Task<ConvoMetadataDto> GetConvoMetadata(string convoId, string convoPasswordSHA512, string userId, string auth);

        /// <summary>
        /// Changes a convo's metadata (description, title, etc...).<para> </para>
        /// The user making the request needs to be the <see cref="Convo"/>'s admin (Creator).<para> </para>
        /// If you're assigning a new admin, he needs to be a participant of the <see cref="Convo"/>, else you'll get a bad request returned from the web api.
        /// </summary>
        /// <param name="requestBody">Request body containing the authentication parameters + the data that needs to be changed (<c>null</c> fields will be ignored; fields with values will be updated and persisted into the server's db)..</param>
        /// <returns>Whether the convo's metadata was changed successfully or not.</returns>
        Task<bool> ChangeConvoMetadata(EpistleRequestBody requestBody);

        /// <summary>
        /// Gets the last convo messages since a specific message's <see cref="Message.Id"/>.
        /// </summary>
        /// <param name="convoId">The convo's identifier.</param>
        /// <param name="convoPasswordSHA512">The convo's password hash.</param>
        /// <param name="userId">The user identifier (needs to be a convo participant).</param>
        /// <param name="auth">The request authentication token.</param>
        /// <param name="tailId">The id of the tail message from which to start retrieving subsequent messages (e.g. starting from message id that evaluates to index 4 will not include <c>convo.Messages[4]</c>). Here you would pass the id of the last message the client already has. If this is null or empty, all messages will be retrieved!</param>
        /// <returns>The retrieved <see cref="Message" />s (<c>null</c> if everything is up to date or if something failed).</returns>
        Task<Message[]> GetConvoMessagesSinceTailId(string convoId, string convoPasswordSHA512, string userId, string auth, long tailId = 0);

        /// <summary>
        /// Gets the latest and greatest messages from a convo!
        /// </summary>
        /// <param name="convoId">The convo's identifier.</param>
        /// <param name="convoPasswordSHA512">The convo's password hash.</param>
        /// <param name="userId">The user identifier (needs to be a convo participant).</param>
        /// <param name="auth">The request authentication token.</param>
        /// <param name="n">How many messages to retrieve?</param>
        /// <returns>The retrieved <see cref="Message" />s (<c>null</c> if everything is up to date or if something failed).</returns>
        Task<Message[]> GetLastConvoMessages(string convoId, string convoPasswordSHA512, string userId, string auth, long n);

        /// <summary>
        /// Gets a specific range of messages from the db, sorted by descending timestamp.<para> </para>
        /// Both the <paramref name="fromId"/> and <paramref name="toId"/> arguments are INCLUSIVE!
        /// </summary>
        /// <param name="convoId">The convo id whose latest and greatest messages you want to retrieve.</param>
        /// <param name="userId">The user id for whom to retrieve the messages.</param>
        /// <param name="convoPasswordSHA512">The convo's password SHA512.</param>
        /// <param name="fromId">The <see cref="Message.Id"/> from which to start looking for messages onwards (inclusive).</param>
        /// <param name="toId">The <see cref="Message.Id"/> until which to retrieve messages (inclusive).</param>
        /// <param name="auth">Request authorization token.</param>
        /// <returns>The retrieved messages (or an empty array if nothing was found). <c>null</c> if the request failed altogether.</returns>
        Task<Message[]> GetConvoMessagesFromRange(string convoId, string convoPasswordSHA512, long fromId, long toId, string userId, string auth);
        
        /// <summary>
        /// Gets the previous messages from a convo starting from a specific <see cref="Message.Id"/>.<para> </para>
        /// The <paramref name="fromId"/> is EXCLUSIVE!
        /// </summary>
        /// <param name="convoId">The convo's identifier.</param>
        /// <param name="convoPasswordSHA512">The convo's password hash.</param>
        /// <param name="userId">The user identifier (needs to be a convo participant).</param>
        /// <param name="auth">The request authentication token.</param>
        /// <param name="fromId">The message id from which to start looking for previous message backwards (EXCLUSIVE!).</param>
        /// <param name="n">How many messages to retrieve?</param>
        /// <returns>The retrieved <see cref="Message" />s (<c>null</c> if there are no previous messages or if something failed).</returns>
        Task<Message[]> GetPreviousMessages(string convoId, string convoPasswordSHA512, string userId, string auth, long fromId, long n);
        
        /// <summary>
        /// Join a <see cref="Convo" />.
        /// </summary>
        /// <param name="requestBody">Request body containing the convo join parameters.</param>
        /// <returns>Whether the <see cref="Convo" /> was joined successfully or not.</returns>
        Task<bool> JoinConvo(EpistleRequestBody requestBody);

        /// <summary>
        /// Leave a <see cref="Convo"/>.
        /// </summary>
        /// <param name="requestBody">The request parameters.</param>
        /// <returns>Whether the <see cref="Convo"/> was left successfully or not.</returns>
        Task<bool> LeaveConvo(EpistleRequestBody requestBody);

        /// <summary>
        /// Kick a user from a conversation.
        /// </summary>
        /// <param name="requestBody">Request parameters for kicking a <see cref="User"/> out of a <see cref="Convo"/>.</param>
        /// <returns>Whether the user was kicked out successfully or not.</returns>
        Task<bool> KickUser(EpistleRequestBody requestBody);
    }
}
