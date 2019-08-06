﻿using System.Collections.Generic;
using GlitchedPolygons.GlitchedEpistle.Client.Extensions;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Convos
{
    /// <summary>
    /// Default implementation (thread safe through usage of standard locks) of the <see cref="IConvoPasswordProvider"/> interface.
    /// </summary>
    /// <seealso cref="IConvoPasswordProvider" />
    public class ConvoPasswordProvider : IConvoPasswordProvider
    {
        private readonly Dictionary<string, string> dictionary = new Dictionary<string, string>(8);

        /// <summary>
        /// Gets a conversation's password SHA512 from the session's password provider.<para></para>
        /// Returns <c>null</c> if the user has never accessed the convo during the session.
        /// </summary>
        /// <param name="convoId">The convo identifier.</param>
        /// <returns>The convo's password SHA512 <c>string</c>; <c>null</c> if the password was</returns>
        public string GetPasswordSHA512(string convoId)
        {
            lock (dictionary)
            return dictionary.TryGetValue(convoId, out string pwSHA512) ? pwSHA512 : null;
        }

        /// <summary>
        /// Saves a convo's password SHA512 for the current app session for easy access.
        /// </summary>
        /// <param name="convoId">The convo identifier.</param>
        /// <param name="passwordSHA512">The password's SHA512.</param>
        public void SetPasswordSHA512(string convoId, string passwordSHA512)
        {
            if (convoId.NotNullNotEmpty() && passwordSHA512.NotNullNotEmpty())
            {
                lock (dictionary)
                dictionary[convoId] = passwordSHA512;
            }
        }

        /// <summary>
        /// Removes a convo password SHA512 from the session's cache.
        /// </summary>
        /// <param name="convoId">The convo identifier.</param>
        public void RemovePasswordSHA512(string convoId)
        {
            lock (dictionary)
            {
                if (dictionary.ContainsKey(convoId))
                {
                    dictionary.Remove(convoId);
                }
            }
        }

        /// <summary>
        /// Clears all session-stored passwords.
        /// </summary>
        public void Clear()
        {
            lock (dictionary)
            dictionary.Clear();
        }
    }
}