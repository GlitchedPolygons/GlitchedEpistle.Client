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
using System.Collections.Generic;

using GlitchedPolygons.GlitchedEpistle.Client.Models;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Convos
{
    /// <summary>
    /// Fetcher service that continuously pulls newest messages from a <see cref="Convo"/>.
    /// </summary>
    public interface IMessageFetcher
    {
        /// <summary>
        /// Starts fetching messages from an Epistle convo continuously.<para> </para>
        /// The retrieved <see cref="Message"/>s are raw, as they were returned from the backend (thus encrypted using the <see cref="MessageFetcher.user"/>'s private key).
        /// </summary>
        /// <param name="convoId">The <see cref="Convo.Id"/>.</param>
        /// <param name="tailId">The <see cref="Message.Id"/> from which to start fetching onwards (previous messages will be ignored).</param>
        /// <param name="callback">The message fetching success callback. This is invoked whenever &gt;=1 messages has been fetched.</param>
        /// <param name="cancellationCallback">Invoked if and when the auto-fetching routine is cancelled via <c>CancellationTokenSource.Cancel()</c>.</param>
        /// <param name="fetchTimeoutMilliseconds">The time in milliseconds between fetch executions.</param>
        /// <returns>A message fetch routine handle in the form of a <see cref="CancellationTokenSource"/>, which provides the ability to stop auto-fetching via <c>CancellationTokenSource.Cancel()</c> OR <see cref="StopAutoFetchingMessages"/>. DO NOT FORGET to dispose the <see cref="CancellationTokenSource"/> via either a <c>using</c> block or by calling <c>CancellationTokenSource.Dispose()</c> manually!</returns>
        CancellationTokenSource StartAutoFetchingMessages(string convoId, long tailId, Action<IEnumerable<Message>> callback, Action cancellationCallback = null, int fetchTimeoutMilliseconds = 314);

        /// <summary>
        /// Stops continuously fetching a convo's messages via the <see cref="CancellationTokenSource"/> that was returned by <see cref="StartAutoFetchingMessages"/>.<para> </para>
        /// Basically does the same as calling the <c>.Cancel()</c> method on the <paramref name="cancellationTokenSource"/>.
        /// </summary>
        /// <param name="cancellationTokenSource">The auto-fetch handle (<see cref="CancellationTokenSource"/> that was returned by <see cref="StartAutoFetchingMessages"/>).</param>
        /// <returns>Whether the auto-fetch cycle could be stopped successfully or not (hint: if it was already stopped once and is currently in the process of cancellation, this returns <c>false</c>).</returns>
        bool StopAutoFetchingMessages(CancellationTokenSource cancellationTokenSource);
    }
}