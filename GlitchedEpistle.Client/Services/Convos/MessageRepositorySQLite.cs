using System;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Generic;

using Dapper;
using GlitchedPolygons.RepositoryPattern.SQLite;
using GlitchedPolygons.GlitchedEpistle.Client.Models;
using GlitchedPolygons.GlitchedEpistle.Client.Extensions;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Convos
{
    /// <summary>
    /// SQLite repository class for accessing a <see cref="Convo"/>'s messages.<para> </para>
    /// </summary>
    public class MessageRepositorySQLite : SQLiteRepository<Message, string>
    {
        private const string TIMESTAMP_FORMAT = "yyyy-MM-dd HH:mm:ss.fffZ";

        /// <summary>
        /// Creates an instance of the <see cref="MessageRepositorySQLite"/> class that will provide 
        /// functionality for accessing an epistle <see cref="Message"/> storage database using SQLite.<para> </para>
        /// If the provided connection string points to a file that doesn't exist or a db that does not contain 
        /// the messages table, a correct SQLite database + table will be created at that path.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="tableName"></param>
        public MessageRepositorySQLite(string connectionString, string tableName = null) : base(connectionString, tableName)
        {
            string sql = $"CREATE TABLE IF NOT EXISTS \"{TableName}\" (\"Id\" TEXT NOT NULL, \"SenderId\" TEXT NOT NULL, \"SenderName\" TEXT, \"TimestampUTC\" TIMESTAMP, \"Body\" TEXT, PRIMARY KEY(\"Id\"))";
            using (var sqlc = OpenConnection())
            {
                sqlc.Execute(sql);
            }
        }

        /// <summary>
        /// Adds a message to the repository.
        /// </summary>
        /// <param name="message">The <see cref="Message"/> to add.</param>
        /// <returns>Whether the operation was successful or not.</returns>
        public override async Task<bool> Add(Message message)
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
            string sql = $"INSERT INTO \"{TableName}\" VALUES (@Id, @SenderId, @SenderName, @TimestampUTC, @Body)";
            
            using (var dbcon = OpenConnection())
            {
                success = await dbcon.ExecuteAsync(sql, new
                {
                    message.Id,
                    message.SenderId,
                    message.SenderName,
                    TimestampUTC = message.TimestampUTC.ToString(TIMESTAMP_FORMAT, CultureInfo.InvariantCulture),
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
        public override async Task<bool> AddRange(IEnumerable<Message> messages)
        {
            var sql = new StringBuilder($"INSERT INTO \"{TableName}\" VALUES ", 512);

            foreach (var message in messages)
            {
                if (message.Id.NullOrEmpty())
                {
                    throw new ArgumentException($"{nameof(MessageRepositorySQLite)}::{nameof(AddRange)}: One or more {nameof(messages)} Id member is null or empty. Very bad! Messages should be added to the local sqlite db using their backend unique id as primary key.");
                }

                sql.Append("('")
                    .Append(message.Id).Append("', '")
                    .Append(message.SenderId).Append("', '")
                    .Append(message.SenderName).Append("', '")
                    .Append(message.TimestampUTC.ToString(TIMESTAMP_FORMAT, CultureInfo.InvariantCulture)).Append("', '")
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
        public override async Task<bool> Update(Message message)
        {
            var sql = new StringBuilder(256)
                .Append($"UPDATE \"{TableName}\" SET ")
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
                    TimestampUTC = message.TimestampUTC.ToString(TIMESTAMP_FORMAT, CultureInfo.InvariantCulture),
                    Body = message.Body
                });

                return result > 0;
            }
        }
    }
}
