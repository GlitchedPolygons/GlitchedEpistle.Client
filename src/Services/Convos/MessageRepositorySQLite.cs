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
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Linq.Expressions;

using Dapper;

using GlitchedPolygons.ExtensionMethods;
using GlitchedPolygons.GlitchedEpistle.Client.Models;
using GlitchedPolygons.GlitchedEpistle.Client.Extensions;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Convos
{
    /// <summary>
    /// SQLite repository class for accessing a <see cref="Convo"/>'s messages.<para> </para>
    /// </summary>
    public class MessageRepositorySQLite : IMessageRepository
    {
        private readonly string tableName;
        private readonly string connectionString;

        /// <summary>
        /// Creates an instance of the <see cref="MessageRepositorySQLite"/> class that will provide 
        /// functionality for accessing an epistle <see cref="Message"/> storage database using SQLite.<para> </para>
        /// If the provided connection string points to a file that doesn't exist or a db that does not contain 
        /// the messages table, a correct SQLite database + table will be created at that path.
        /// </summary>
        /// <param name="connectionString">Connection string containing the SQLite db file path.</param>
        public MessageRepositorySQLite(string connectionString)
        {
            this.tableName = nameof(Message);
            this.connectionString = connectionString;
            string sql = $"CREATE TABLE IF NOT EXISTS \"{tableName}\" (\"Id\" INTEGER NOT NULL, \"SenderId\" TEXT NOT NULL, \"SenderName\" TEXT, \"TimestampUTC\" INTEGER, \"Body\" TEXT, PRIMARY KEY(\"Id\"))";
            using (var sqlc = OpenConnection())
            {
                sqlc.Execute(sql);
            }
        }

        /// <summary>
        /// Opens a <see cref="IDbConnection"/> to the SQLite database. <para> </para>
        /// Does not dispose automatically: make sure to wrap your usage into a <c>using</c> block.
        /// </summary>
        /// <returns>The opened <see cref="IDbConnection"/> (remember to dispose of it asap after you're done!).</returns>
        private IDbConnection OpenConnection()
        {
            var sqlConnection = new SQLiteConnection(connectionString);
            sqlConnection.Open();
            return sqlConnection;
        }

        /// <summary>
        /// Gets a <see cref="Message"/> synchronously by its unique identifier.
        /// </summary>
        /// <param name="id">The <see cref="Message"/>'s unique identifier.</param>
        /// <returns>The first found <see cref="Message"/>; <c>null</c> if nothing was found.</returns>
        public Message this[long id]
        {
            get
            {
                using (var sqlc = new SQLiteConnection(connectionString))
                {
                    sqlc.Open();

                    using (var cmd = new SQLiteCommand($"SELECT * FROM \"{tableName}\" WHERE \"Id\" = '{id}'", sqlc))
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            return null;
                        }

                        reader.Read();
                        return new Message
                        {
                            Id = reader.GetInt64(0),
                            SenderId = reader.GetString(1),
                            SenderName = reader.GetString(2),
                            TimestampUTC = DateTimeExtensions.FromUnixTimeMilliseconds(reader.GetInt64(3)),
                            Body = reader.GetString(4)
                        };
                    }
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="Message.Id"/> from the most recent <see cref="Message"/> in the repository.
        /// </summary>
        public async Task<string> GetLastMessageId()
        {
            using (var sqlc = OpenConnection())
            {
                string id = await sqlc.QueryFirstOrDefaultAsync<string>($"SELECT \"Id\" FROM \"{tableName}\" ORDER BY \"TimestampUTC\" DESC LIMIT 1");
                return id;
            }
        }

        /// <summary>
        /// Gets a <see cref="Message"/> asynchronously by its unique identifier.
        /// </summary>
        /// <param name="id">The <see cref="Message"/>'s unique identifier.</param>
        /// <returns>The first found <see cref="Message"/>; <c>null</c> if nothing was found.</returns>
        public async Task<Message> Get(long id)
        {
            using (var sqlc = new SQLiteConnection(connectionString))
            {
                await sqlc.OpenAsync();

                using (var cmd = new SQLiteCommand($"SELECT * FROM \"{tableName}\" WHERE \"Id\" = '{id}'", sqlc))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (!reader.HasRows)
                    {
                        return null;
                    }

                    await reader.ReadAsync();
                    return new Message
                    {
                        Id = reader.GetInt64(0),
                        SenderId = reader.GetString(1),
                        SenderName = reader.GetString(2),
                        TimestampUTC = DateTimeExtensions.FromUnixTimeMilliseconds(reader.GetInt64(3)),
                        Body = reader.GetString(4)
                    };
                }
            }
        }

        /// <summary>
        /// Gets all <see cref="Message"/>s from the repository.
        /// </summary>
        /// <returns>All <see cref="Message"/>s inside the repo.</returns>
        public async Task<IEnumerable<Message>> GetAll()
        {
            var messages = new List<Message>(8);
            using (var sqlc = new SQLiteConnection(connectionString))
            {
                await sqlc.OpenAsync();

                using (var cmd = new SQLiteCommand($"SELECT * FROM \"{tableName}\" ORDER BY \"TimestampUTC\" ASC", sqlc))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (!reader.HasRows)
                    {
                        return Array.Empty<Message>();
                    }

                    while (await reader.ReadAsync())
                    {
                        messages.Add(new Message
                        {
                            Id = reader.GetInt64(0),
                            SenderId = reader.GetString(1),
                            SenderName = reader.GetString(2),
                            TimestampUTC = DateTimeExtensions.FromUnixTimeMilliseconds(reader.GetInt64(3)),
                            Body = reader.GetString(4)
                        });
                    }
                }
            }
            return messages;
        }

        /// <summary>
        /// Gets the n latest <see cref="Message"/>s from the repo.
        /// </summary>
        /// <param name="n">The amount of messages to retrieve.</param>
        /// <param name="offset">How many entries to skip before starting to gather messages.</param>
        public async Task<IEnumerable<Message>> GetLastMessages(int n, int offset = 0)
        {
            n = Math.Abs(n);
            offset = Math.Abs(offset);

            IList<Message> messages = new List<Message>(n);

            string sql = $"SELECT * FROM \"{tableName}\" ORDER BY \"TimestampUTC\" DESC LIMIT {n}";
            if (offset > 0)
            {
                sql += " OFFSET " + offset;
            }

            using (var sqlc = new SQLiteConnection(connectionString))
            {
                await sqlc.OpenAsync();

                using (var cmd = new SQLiteCommand(sql, sqlc))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (!reader.HasRows)
                    {
                        return Array.Empty<Message>();
                    }

                    while (await reader.ReadAsync())
                    {
                        messages.Add(new Message
                        {
                            Id = reader.GetInt64(0),
                            SenderId = reader.GetString(1),
                            SenderName = reader.GetString(2),
                            TimestampUTC = DateTimeExtensions.FromUnixTimeMilliseconds(reader.GetInt64(3)),
                            Body = reader.GetString(4)
                        });
                    }
                }
            }
            return messages.Reverse();
        }

        /// <summary>
        /// Gets a single <see cref="Message"/> from the repo according to the specified predicate condition.<para> </para>
        /// If 0 or &gt;1 entities are found, <c>null</c> is returned.<para> </para>
        /// </summary>
        /// <param name="predicate">The search predicate.</param>
        /// <returns>Single found <see cref="Message"/> or <c>null</c>.</returns>
        public async Task<Message> SingleOrDefault(Expression<Func<Message, bool>> predicate)
        {
            try
            {
                Message result = (await GetAll())?.SingleOrDefault(predicate.Compile());
                return result;
            }
            catch (Exception)
            {
                return default;
            }
        }

        /// <summary>
        /// Finds all <see cref="Message"/>s according to the specified predicate <see cref="T:System.Linq.Expressions.Expression" />.<para> </para>
        /// </summary>
        /// <param name="predicate">The search predicate (all <see cref="Message"/>s that match the provided conditions will be added to the query's result).</param>
        /// <returns>The found <see cref="Message"/>s.</returns>
        public async Task<IEnumerable<Message>> Find(Expression<Func<Message, bool>> predicate)
        {
            try
            {
                IEnumerable<Message> result = (await GetAll()).Where(predicate.Compile());
                return result;
            }
            catch (Exception)
            {
                return Array.Empty<Message>();
            }
        }

        /// <summary>
        /// Adds a message to the repository.
        /// </summary>
        /// <param name="message">The <see cref="Message"/> to add.</param>
        /// <returns>Whether the operation was successful or not.</returns>
        public async Task<bool> Add(Message message)
        {
            if (message is null)
            {
                return false;
            }

            bool success = false;
            string sql = $"INSERT OR IGNORE INTO \"{tableName}\" VALUES (@Id, @SenderId, @SenderName, @TimestampUTC, @Body)";
            
            using (var dbcon = OpenConnection())
            {
                success = await dbcon.ExecuteAsync(sql, new
                {
                    Id = message.Id,
                    SenderId = message.SenderId,
                    SenderName = message.SenderName,
                    TimestampUTC = message.TimestampUTC.ToUnixTimeMilliseconds(),
                    Body = message.Body,
                }) > 0;
            }
            
            return success;
        }

        /// <summary>
        /// Adds multiple <see cref="Message"/>s in bulk to the repository (SQLite db).
        /// </summary>
        /// <param name="messages">The <see cref="Message"/>s to add.</param>
        /// <returns>Whether the operation was successful or not.</returns>
        public async Task<bool> AddRange(IEnumerable<Message> messages)
        {
            if (messages is null)
            {
                return false;
            }

            bool success = false;
            string sql = $"INSERT OR IGNORE INTO \"{tableName}\" VALUES (@Id, @SenderId, @SenderName, @TimestampUTC, @Body)";

            using (var dbcon = OpenConnection())
            using (var t = dbcon.BeginTransaction())
            {
                success = await dbcon.ExecuteAsync(sql, messages.Select(m => new
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    SenderName = m.SenderName,
                    TimestampUTC = m.TimestampUTC.ToUnixTimeMilliseconds(),
                    Body = m.Body,
                }), t) > 0;

                if (success)
                {
                    t.Commit();
                }
            }

            return success;
        }

        /// <summary>
        /// Updates an existing <see cref="Message"/> record inside the db.
        /// </summary>
        /// <param name="message">The new (updated) <see cref="Message"/> instance.</param>
        /// <returns>Whether the operation was successful or not.</returns>
        public async Task<bool> Update(Message message)
        {
            var sql = new StringBuilder(256)
                .Append($"UPDATE \"{tableName}\" SET ")
                .Append("\"SenderId\" = @SenderId, ")
                .Append("\"SenderName\" = @SenderName, ")
                .Append("\"TimestampUTC\" = @TimestampUTC, ")
                .Append("\"Body\" = @Body ")
                .Append("WHERE \"Id\" = @Id");

            using (var dbcon = OpenConnection())
            {
                int result = await dbcon.ExecuteAsync(sql.ToString(), new
                {
                    Id = message.Id,
                    SenderId = message.SenderId,
                    SenderName = message.SenderName,
                    TimestampUTC = message.TimestampUTC.ToUnixTimeMilliseconds(),
                    Body = message.Body
                });

                return result > 0;
            }
        }

        /// <summary>
        /// Removes the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="message">The <see cref="Message"/> to remove.</param>
        /// <returns>Whether the <see cref="Message"/> could be removed successfully or not.</returns>
        public Task<bool> Remove(Message message)
        {
            return Remove(message.Id);
        }

        /// <summary>
        /// Removes the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="id">The unique id of the <see cref="Message"/> to remove.</param>
        /// <returns>Whether the <see cref="Message"/> could be removed successfully or not.</returns>
        public async Task<bool> Remove(long id)
        {
            bool result = false;
            IDbConnection sqlc = null;
            try
            {
                sqlc = OpenConnection();
                result = await sqlc.ExecuteAsync($"DELETE FROM \"{tableName}\" WHERE \"Id\" = @Id", new { Id = id }) > 0;
            }
            catch (Exception)
            {
                result = false;
            }
            finally
            {
                sqlc?.Dispose();
            }
            return result;
        }

        /// <summary>
        /// Removes all <see cref="Message"/>s at once from the repository.
        /// </summary>
        /// <returns>Whether the <see cref="Message"/>s were removed successfully or not. If the repository was already empty, <c>false</c> is returned (because nothing was actually &lt;&lt;removed&gt;&gt; ).</returns>
        public async Task<bool> RemoveAll()
        {
            bool result = false;
            IDbConnection sqlc = null;
            try
            {
                sqlc = OpenConnection();
                result = await sqlc.ExecuteAsync($"DELETE FROM \"{tableName}\"") > 0;
            }
            catch (Exception)
            {
                result = false;
            }
            finally
            {
                sqlc?.Dispose();
            }
            return result;
        }

        /// <summary>
        /// Removes all <see cref="Message"/>s that match the specified conditions.<para> </para>
        /// </summary>
        /// <param name="predicate">The predicate <see cref="Expression"/> that defines which <see cref="Message"/>s should be removed.</param>
        /// <returns>Whether the <see cref="Message"/>s were removed successfully or not.</returns>
        public async Task<bool> RemoveRange(Expression<Func<Message, bool>> predicate)
        {
            return await RemoveRange(await Find(predicate));
        }

        /// <summary>
        /// Removes the range of <see cref="Message"/>s from the repository.
        /// </summary>
        /// <param name="messages">The <see cref="Message"/>s to remove.</param>
        /// <returns>Whether all <see cref="Message"/>s were removed successfully or not.</returns>
        public Task<bool> RemoveRange(IEnumerable<Message> messages)
        {
            return RemoveRange(messages.Select(e => e.Id));
        }

        /// <summary>
        /// Removes the range of <see cref="Message"/>s from the repository.
        /// </summary>
        /// <param name="ids">The unique ids of the <see cref="Message"/>s to remove.</param>
        /// <returns>Whether all <see cref="Message"/>s were removed successfully or not.</returns>
        public async Task<bool> RemoveRange(IEnumerable<long> ids)
        {
            bool result = false;
            IDbConnection sqlc = null;
            try
            {
                var sql = new StringBuilder(256)
                    .Append("DELETE FROM ")
                    .Append('\"')
                    .Append(tableName)
                    .Append('\"')
                    .Append(" WHERE \"Id\" IN (");

                foreach (long id in ids)
                {
                    sql.Append('\'').Append(id).Append('\'').Append(", ");
                }

                string sqlString = sql.ToString().TrimEnd(',', ' ') + ");";

                sqlc = OpenConnection();
                result = await sqlc.ExecuteAsync(sqlString) > 0;
            }
            catch (Exception)
            {
                result = false;
            }
            finally
            {
                sqlc?.Dispose();
            }
            return result;
        }
    }
}
