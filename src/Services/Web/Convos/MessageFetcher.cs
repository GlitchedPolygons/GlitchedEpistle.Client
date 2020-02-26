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

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using GlitchedPolygons.ExtensionMethods;
using GlitchedPolygons.GlitchedEpistle.Client.Models;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Convos
{
    /// <summary>
    /// Fetcher service that continuously pulls newest messages from a <see cref="Convo"/>.
    /// </summary>
    public class MessageFetcher : IMessageFetcher
    {
        private readonly User user;
        private readonly IConvoService convoService;

#pragma warning disable 1591
        public MessageFetcher(IConvoService convoService, User user)
        {
            this.user = user;
            this.convoService = convoService;
        }
#pragma warning restore 1591

        /// <summary>
        /// Starts fetching messages from an Epistle convo continuously.<para> </para>
        /// The retrieved <see cref="Message"/>s are raw, as they were returned from the backend (thus encrypted using the <see cref="MessageFetcher.user"/>'s private key).
        /// </summary>
        /// <param name="convoId">The <see cref="Convo.Id"/>.</param>
        /// <param name="convoPasswordSHA512">The convo's password SHA512.'</param>
        /// <param name="tailId">The <see cref="Message.Id"/> from which to start fetching onwards (previous messages will be ignored).</param>
        /// <param name="callback">The message fetching success callback. This is invoked whenever &gt;=1 messages has been fetched.</param>
        /// <param name="cancellationCallback">Invoked if and when the auto-fetching routine is cancelled via <c>CancellationTokenSource.Cancel()</c>.</param>
        /// <param name="fetchTimeoutMilliseconds">The time in milliseconds between fetch executions.</param>
        /// <returns>A message fetch routine handle in the form of a <see cref="CancellationTokenSource"/>, which provides the ability to stop auto-fetching via <c>CancellationTokenSource.Cancel()</c> OR <see cref="StopAutoFetchingMessages"/>. DO NOT FORGET to dispose the <see cref="CancellationTokenSource"/> via either a <c>using</c> block or by calling <c>CancellationTokenSource.Dispose()</c> manually!</returns>
        public CancellationTokenSource StartAutoFetchingMessages(string convoId, string convoPasswordSHA512, long tailId, Action<IEnumerable<Message>> callback, Action cancellationCallback = null, int fetchTimeoutMilliseconds = 314)
        {
            if (fetchTimeoutMilliseconds < 0)
                fetchTimeoutMilliseconds *= -1;

            var ct = new CancellationTokenSource();

            Task.Run(async () =>
            {
                long currentTailId = tailId;
                do
                {
                    Message[] retrievedMessages = await convoService.GetConvoMessagesSinceTailId(
                        convoId: convoId,
                        convoPasswordSHA512: convoPasswordSHA512,
                        userId: user.Id,
                        auth: user.Token.Item2,
                        tailId: currentTailId
                    ).ConfigureAwait(false);

                    if (retrievedMessages.NotNullNotEmpty())
                    {
                        for (int i = retrievedMessages.Length - 1; i >= 0; i--)
                        {
                            if (retrievedMessages[i]?.Id > currentTailId)
                            {
                                currentTailId = retrievedMessages[i].Id;
                            }
                        }
                        if (!ct.IsCancellationRequested)
                        {
                            callback?.Invoke(retrievedMessages);
                        }
                    }

                    await Task.Delay(fetchTimeoutMilliseconds, ct.Token).ConfigureAwait(false);
                } while (!ct.IsCancellationRequested);

                cancellationCallback?.Invoke();
            }, ct.Token);

            return ct;
        }

        /// <summary>
        /// Stops continuously fetching a convo's messages via the <see cref="CancellationTokenSource"/> that was returned by <see cref="IMessageFetcher.StartAutoFetchingMessages"/>.<para> </para>
        /// Basically does the same as calling the <c>.Cancel()</c> method on the <paramref name="cancellationTokenSource"/>.
        /// </summary>
        /// <param name="cancellationTokenSource">The auto-fetch handle (<see cref="CancellationTokenSource"/> that was returned by <see cref="IMessageFetcher.StartAutoFetchingMessages"/>).</param>
        /// <returns>Whether the auto-fetch cycle could be stopped successfully or not (hint: if it was already stopped once and is currently in the process of cancellation, this returns <c>false</c>).</returns>
        public bool StopAutoFetchingMessages(CancellationTokenSource cancellationTokenSource)
        {
            if (cancellationTokenSource is null || cancellationTokenSource.IsCancellationRequested)
            {
                return false;
            }
            try
            {
                cancellationTokenSource.Cancel();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
