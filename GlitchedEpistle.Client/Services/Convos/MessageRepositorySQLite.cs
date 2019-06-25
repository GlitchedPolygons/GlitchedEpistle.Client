using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Linq.Expressions;

using Dapper;

using GlitchedPolygons.RepositoryPattern;
using GlitchedPolygons.GlitchedEpistle.Client.Models;
using GlitchedPolygons.GlitchedEpistle.Client.Extensions;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Convos
{
    /// <summary>
    /// SQLite repository class for accessing a <see cref="Convo"/>'s messages.<para> </para>
    /// </summary>
    public class MessageRepositorySQLite : IRepository<Message, string>
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
            string sql = $"CREATE TABLE IF NOT EXISTS \"{tableName}\" (\"Id\" TEXT NOT NULL, \"SenderId\" TEXT NOT NULL, \"SenderName\" TEXT, \"TimestampUTC\" INTEGER, \"Body\" TEXT, PRIMARY KEY(\"Id\"))";
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
        public Message this[string id]
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
                            Id = reader.GetString(0),
                            SenderId = reader.GetString(1),
                            SenderName = reader.GetString(2),
                            TimestampUTC = DateTimeExtensions.FromUnixTimeSeconds(reader.GetInt64(3)),
                            Body = reader.GetString(4)
                        };
                    }
                }
            }
        }

        /// <summary>
        /// Gets a <see cref="Message"/> asynchronously by its unique identifier.
        /// </summary>
        /// <param name="id">The <see cref="Message"/>'s unique identifier.</param>
        /// <returns>The first found <see cref="Message"/>; <c>null</c> if nothing was found.</returns>
        public async Task<Message> Get(string id)
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
                        Id = reader.GetString(0),
                        SenderId = reader.GetString(1),
                        SenderName = reader.GetString(2),
                        TimestampUTC = DateTimeExtensions.FromUnixTimeSeconds(reader.GetInt64(3)),
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

                using (var cmd = new SQLiteCommand($"SELECT * FROM \"{tableName}\"", sqlc))
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
                            Id = reader.GetString(0),
                            SenderId = reader.GetString(1),
                            SenderName = reader.GetString(2),
                            TimestampUTC = DateTimeExtensions.FromUnixTimeSeconds(reader.GetInt64(3)),
                            Body = reader.GetString(4)
                        });
                    }
                }
            }
            return messages;
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

            if (message.Id.NullOrEmpty())
            {
                throw new ArgumentException($"{nameof(MessageRepositorySQLite)}::{nameof(Add)}: The {nameof(message)}'s Id property is null or empty. Very bad! Messages should be added to the local sqlite db using their backend unique id as primary key.");
            }

            bool success = false;
            string sql = $"INSERT INTO \"{tableName}\" VALUES (@Id, @SenderId, @SenderName, @TimestampUTC, @Body)";
            
            using (var dbcon = OpenConnection())
            {
                success = await dbcon.ExecuteAsync(sql, new
                {
                    message.Id,
                    message.SenderId,
                    message.SenderName,
                    TimestampUTC = message.TimestampUTC.ToUnixTimeSeconds(),
                    message.Body,
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
            var sql = new StringBuilder($"INSERT INTO \"{tableName}\" VALUES ", 512);

            foreach (var message in messages)
            {
                if (message.Id.NullOrEmpty())
                {
                    throw new ArgumentException($"{nameof(MessageRepositorySQLite)}::{nameof(AddRange)}: One or more {nameof(messages)} Id member is null or empty. Very bad! Messages should be added to the local sqlite db using their backend unique id as primary key.");
                }

                sql.Append("('")
                    .Append(message.Id).Append("', '")
                    .Append(message.SenderId).Append("', '")
                    .Append(message.SenderName).Append("', ")
                    .Append(message.TimestampUTC.ToUnixTimeSeconds()).Append(", '")
                    .Append(message.Body)
                    .Append("'),");
            }

            using (var dbcon = OpenConnection())
            {
                return await dbcon.ExecuteAsync(sql.ToString().TrimEnd(',')) > 0;
            }
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
                    TimestampUTC = message.TimestampUTC.ToUnixTimeSeconds(),
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
        public async Task<bool> Remove(string id)
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
        public async Task<bool> RemoveRange(IEnumerable<string> ids)
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

                foreach (string id in ids)
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
