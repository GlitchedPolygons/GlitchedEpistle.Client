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
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

using Dapper;

using GlitchedPolygons.ExtensionMethods;
using GlitchedPolygons.RepositoryPattern;
using GlitchedPolygons.GlitchedEpistle.Client.Models;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Web.Convos
{
    /// <summary>
    /// SQLite repository class for accessing all locally stored <see cref="Convo"/>s.<para> </para>
    /// </summary>
    public class ConvoRepositorySQLite : IRepository<Convo, string>
    {
        private readonly string tableName;
        private readonly string connectionString;
        
        /// <summary>
        /// Creates an instance of the <see cref="ConvoRepositorySQLite"/> class that will provide 
        /// functionality for accessing the local <see cref="Convo"/> storage database using SQLite.<para> </para>
        /// This db only contains the metadata for the convos stored locally on the device.<para> </para>
        /// If the provided connection string points to a file that doesn't exist or to a db that does not contain 
        /// the convos table, a correct SQLite database + table will be created at that path.
        /// </summary>
        /// <param name="connectionString">Connection string containing the SQLite db file path.</param>
        public ConvoRepositorySQLite(string connectionString)
        {
            this.tableName = nameof(Convo);
            this.connectionString = connectionString;

            using var dbcon = OpenConnection();
            dbcon.Execute($"CREATE TABLE IF NOT EXISTS \"{tableName}\" (\"Id\" TEXT NOT NULL, \"CreatorId\" TEXT NOT NULL, \"Name\" TEXT, \"Description\" TEXT, \"CreationTimestampUTC\" INTEGER, \"ExpirationUTC\" INTEGER, \"Participants\" TEXT, \"BannedUsers\" TEXT, PRIMARY KEY(\"Id\"))");
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
        /// Gets a <see cref="Convo"/> synchronously by its unique identifier.
        /// </summary>
        /// <param name="id">The <see cref="Convo"/>'s unique identifier.</param>
        /// <returns>The first found <see cref="Convo"/>; <c>null</c> if nothing was found.</returns>
        public Convo this[string id]
        {
            get
            {
                using var dbcon = new SQLiteConnection(connectionString);
                dbcon.Open();

                using var cmd = new SQLiteCommand($"SELECT * FROM \"{tableName}\" WHERE \"Id\" = '{id}'", dbcon);
                using var reader = cmd.ExecuteReader();
                
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
                
                return new Convo
                {
                    Id = reader.GetString(0),
                    CreatorId = reader.GetString(1),
                    Name = reader.GetString(2),
                    Description = reader.GetString(3),
                    CreationUTC = reader.GetInt64(4),
                    ExpirationUTC = reader.GetInt64(5),
                    Participants = reader.GetString(6).Split(',').ToList(),
                    BannedUsers = reader.GetString(7).Split(',').ToList(),
                };
            }
        }

        /// <summary>
        /// Gets a <see cref="Convo"/> asynchronously by its unique identifier.
        /// </summary>
        /// <param name="id">The <see cref="Convo"/>'s unique identifier.</param>
        /// <returns>The first found <see cref="Convo"/>; <c>null</c> if nothing was found.</returns>
        public async Task<Convo> Get(string id)
        {
            await using var dbcon = new SQLiteConnection(connectionString);
            await dbcon.OpenAsync().ConfigureAwait(false);

            await using var cmd = new SQLiteCommand($"SELECT * FROM \"{tableName}\" WHERE \"Id\" = '{id}'", dbcon);
            await using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
                
            if (!reader.HasRows)
            {
                return null;
            }

            await reader.ReadAsync().ConfigureAwait(false);
            
            return new Convo
            {
                Id = reader.GetString(0),
                CreatorId = reader.GetString(1),
                Name = reader.GetString(2),
                Description = reader.GetString(3),
                CreationUTC = reader.GetInt64(4),
                ExpirationUTC = reader.GetInt64(5),
                Participants = reader.GetString(6).Split(',').ToList(),
                BannedUsers = reader.GetString(7).Split(',').ToList(),
            };
        }

        /// <summary>
        /// Gets all <see cref="Convo"/>s from the repository.
        /// </summary>
        /// <returns>All <see cref="Convo"/>s inside the repo.</returns>
        public async Task<IEnumerable<Convo>> GetAll()
        {
            var convos = new List<Convo>(8);
            
            await using var dbcon = new SQLiteConnection(connectionString);
            await dbcon.OpenAsync().ConfigureAwait(false);

            await using var cmd = new SQLiteCommand($"SELECT * FROM \"{tableName}\" ORDER BY \"Name\"", dbcon);
            await using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            
            if (!reader.HasRows)
            {
                return Array.Empty<Convo>();
            }

            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                convos.Add(new Convo
                {
                    Id = reader.GetString(0),
                    CreatorId = reader.GetString(1),
                    Name = reader.GetString(2),
                    Description = reader.GetString(3),
                    CreationUTC = reader.GetInt64(4),
                    ExpirationUTC = reader.GetInt64(5),
                    Participants = reader.GetString(6).Split(',').ToList(),
                    BannedUsers = reader.GetString(7).Split(',').ToList(),
                });
            }
            
            return convos;
        }
        
        /// <summary>
        /// Gets a single entity from the repo according to the specified predicate condition.<para> </para>
        /// If 0 or &gt;1 entities are found, <c>null</c> is returned.<para> </para>
        /// </summary>
        /// <param name="predicate">The search predicate.</param>
        /// <returns>Single found <see cref="Convo"/> or <c>null</c>.</returns>
        public async Task<Convo> SingleOrDefault(Expression<Func<Convo, bool>> predicate)
        {
            try
            {
                Convo result = (await GetAll().ConfigureAwait(false))?.SingleOrDefault(predicate.Compile());
                return result;
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Finds all <see cref="Convo"/>s according to the specified predicate <see cref="T:System.Linq.Expressions.Expression" />.<para> </para>
        /// </summary>
        /// <param name="predicate">The search predicate (all entities that match the provided conditions will be added to the query's result).</param>
        /// <returns>The found <see cref="Convo"/>s.</returns>
        public async Task<IEnumerable<Convo>> Find(Expression<Func<Convo, bool>> predicate)
        {
            try
            {
                IEnumerable<Convo> result = (await GetAll().ConfigureAwait(false))?.Where(predicate.Compile());
                return result;
            }
            catch
            {
                return Array.Empty<Convo>();
            }
        }
        
        /// <summary>
        /// Adds a <see cref="Convo"/> entry to the repository.
        /// </summary>
        /// <param name="convo">The <see cref="Convo"/> to add.</param>
        /// <returns>Whether the operation was successful or not.</returns>
        public async Task<bool> Add(Convo convo)
        {
            if (convo is null)
            {
                return false;
            }

            if (convo.Id.NullOrEmpty())
            {
                throw new ArgumentException($"{nameof(ConvoRepositorySQLite)}::{nameof(Add)}: The {nameof(convo)}'s Id property is null or empty. Very bad! Convos should be added to the local sqlite db using their backend unique id as primary key.");
            }

            bool success = false;
            string sql = $"INSERT INTO \"{tableName}\" VALUES (@Id, @CreatorId, @Name, @Description, @CreationTimestampUTC, @ExpirationUTC, @Participants, @BannedUsers)";
            
            using (var dbcon = OpenConnection())
            {
                success = await dbcon.ExecuteAsync(sql, new
                {
                    convo.Id,
                    convo.CreatorId,
                    convo.Name,
                    convo.Description,
                    ExpirationUTC = convo.ExpirationUTC,
                    CreationTimestampUTC = convo.CreationUTC,
                    Participants = convo.GetParticipantIdsCommaSeparated(),
                    BannedUsers = convo.GetBannedUsersCommaSeparated()
                }).ConfigureAwait(false) > 0;
            }
            
            return success;
        }

        /// <summary>
        /// Adds multiple <see cref="Convo"/>s in bulk to the repository (SQLite db).
        /// </summary>
        /// <param name="convos">The <see cref="Convo"/>s to add.</param>
        /// <returns>Whether the operation was successful or not.</returns>
        public async Task<bool> AddRange(IEnumerable<Convo> convos)
        {
            if (convos is null)
            {
                return false;
            }

            bool success = false;
            string sql = $"INSERT INTO \"{tableName}\" VALUES (@Id, @CreatorId, @Name, @Description, @CreationTimestampUTC, @ExpirationUTC, @Participants, @BannedUsers)";

            using var dbcon = OpenConnection();
            using var t = dbcon.BeginTransaction();
            
            success = await dbcon.ExecuteAsync(sql, convos.Where(c => c.Id.NotNullNotEmpty()).Select(c => new
            {
                Id = c.Id,
                CreatorId = c.CreatorId,
                Name = c.Name,
                Description = c.Description,
                ExpirationUTC = c.ExpirationUTC,
                CreationTimestampUTC = c.CreationUTC,
                Participants = c.GetParticipantIdsCommaSeparated(),
                BannedUsers = c.GetBannedUsersCommaSeparated()
            }), t).ConfigureAwait(false) > 0;

            if (success)
            {
                t.Commit();
            }

            return success;
        }

        /// <summary>
        /// Updates an existing <see cref="Convo"/> record inside the db.
        /// </summary>
        /// <param name="updatedConvo">The new (updated) <see cref="Convo"/> instance.</param>
        /// <returns>Whether the operation was successful or not.</returns>
        public async Task<bool> Update(Convo updatedConvo)
        {
            var sql = new StringBuilder(512)
                .Append($"UPDATE \"{tableName}\" SET ")
                .Append("\"CreatorId\" = @CreatorId, ")
                .Append("\"Name\" = @Name, ")
                .Append("\"Description\" = @Description, ")
                .Append("\"CreationTimestampUTC\" = @CreationTimestampUTC, ")
                .Append("\"ExpirationUTC\" = @ExpirationUTC, ")
                .Append("\"Participants\" = @Participants, ")
                .Append("\"BannedUsers\" = @BannedUsers ")
                .Append("WHERE \"Id\" = @Id");

            using var dbcon = OpenConnection();
            
            return await dbcon.ExecuteAsync(sql.ToString(), new
            {
                Id = updatedConvo.Id,
                Name = updatedConvo.Name,
                CreatorId = updatedConvo.CreatorId,
                Description = updatedConvo.Description,
                ExpirationUTC = updatedConvo.ExpirationUTC,
                CreationTimestampUTC = updatedConvo.CreationUTC,
                Participants = updatedConvo.GetParticipantIdsCommaSeparated(),
                BannedUsers = updatedConvo.GetBannedUsersCommaSeparated()
            }).ConfigureAwait(false) > 0;
        }

        /// <summary>
        /// Removes the specified <see cref="Convo"/>.
        /// </summary>
        /// <param name="convo">The <see cref="Convo"/> to remove.</param>
        /// <returns>Whether the <see cref="Convo"/> could be removed successfully or not.</returns>
        public Task<bool> Remove(Convo convo)
        {
            return Remove(convo.Id);
        }

        /// <summary>
        /// Removes the specified <see cref="Convo"/>.
        /// </summary>
        /// <param name="id">The unique id of the <see cref="Convo"/> to remove.</param>
        /// <returns>Whether the <see cref="Convo"/> could be removed successfully or not.</returns>
        public async Task<bool> Remove(string id)
        {
            bool result = false;
            IDbConnection dbcon = null;
            try
            {
                dbcon = OpenConnection();
                result = await dbcon.ExecuteAsync($"DELETE FROM \"{tableName}\" WHERE \"Id\" = @Id", new {Id = id}).ConfigureAwait(false) > 0;
            }
            catch
            {
                result = false;
            }
            finally
            {
                dbcon?.Dispose();
            }
            return result;
        }

        /// <summary>
        /// Removes all <see cref="Convo"/>s at once from the repository.
        /// </summary>
        /// <returns>Whether the <see cref="Convo"/>s were removed successfully or not. If the repository was already empty, <c>false</c> is returned (because nothing was actually &lt;&lt;removed&gt;&gt; ).</returns>
        public async Task<bool> RemoveAll()
        {
            bool result = false;
            IDbConnection dbcon = null;
            try
            {
                dbcon = OpenConnection();
                result = await dbcon.ExecuteAsync($"DELETE FROM \"{tableName}\"").ConfigureAwait(false) > 0;
            }
            catch
            {
                result = false;
            }
            finally
            {
                dbcon?.Dispose();
            }
            return result;
        }

        /// <summary>
        /// Removes all <see cref="Convo"/>s that match the specified conditions.<para> </para>
        /// </summary>
        /// <param name="predicate">The predicate <see cref="Expression"/> that defines which entities should be removed.</param>
        /// <returns>Whether the entities were removed successfully or not.</returns>
        public async Task<bool> RemoveRange(Expression<Func<Convo, bool>> predicate)
        {
            return await RemoveRange(await Find(predicate).ConfigureAwait(false)).ConfigureAwait(false);
        }

        /// <summary>
        /// Removes the range of <see cref="Convo"/>s from the repository.
        /// </summary>
        /// <param name="convos">The <see cref="Convo"/>s to remove.</param>
        /// <returns>Whether all <see cref="Convo"/>s were removed successfully or not.</returns>
        public Task<bool> RemoveRange(IEnumerable<Convo> convos)
        {
            return RemoveRange(convos.Select(e => e.Id));
        }
        
        /// <summary>
        /// Removes the range of <see cref="Convo"/>s from the repository.
        /// </summary>
        /// <param name="ids">The unique ids of the <see cref="Convo"/>s to remove.</param>
        /// <returns>Whether all <see cref="Convo"/>s were removed successfully or not.</returns>
        public async Task<bool> RemoveRange(IEnumerable<string> ids)
        {
            bool result = false;
            IDbConnection dbcon = null;
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
                
                dbcon = OpenConnection();
                result = await dbcon.ExecuteAsync(sqlString).ConfigureAwait(false) > 0;
            }
            catch
            {
                result = false;
            }
            finally
            {
                dbcon?.Dispose();
            }
            return result;
        }
    }
}
