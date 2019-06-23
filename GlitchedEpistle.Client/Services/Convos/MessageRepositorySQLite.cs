using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;

using Dapper;

using GlitchedPolygons.RepositoryPattern.SQLite;
using GlitchedPolygons.GlitchedEpistle.Client.Models;
using GlitchedPolygons.GlitchedEpistle.Client.Extensions;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Convos
{
    public class MessageRepositorySQLite : SQLiteRepository<Message, string>
    {
        public MessageRepositorySQLite(string connectionString, string tableName = null) : base(connectionString, tableName)
        {
            string sql = $"CREATE TABLE IF NOT EXISTS \"{TableName}\" (\"Id\" TEXT NOT NULL, \"SenderId\" TEXT NOT NULL, \"SenderName\" TEXT, \"TimestampUTC\" TIMESTAMP, \"Body\" TEXT, PRIMARY KEY(\"Id\"))";
            using (var sqlc = OpenConnection())
            {
                sqlc.Execute(sql);
            }
        }

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
                    message.TimestampUTC,
                    message.Body,
                }) > 0;
            }
            
            return success;
        }

        public override Task<bool> AddRange(IEnumerable<Message> entities)
        {
            throw new System.NotImplementedException();
        }

        public override Task<bool> Update(Message entity)
        {
            throw new System.NotImplementedException();
        }
    }
}