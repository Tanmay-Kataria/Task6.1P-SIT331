using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;

namespace robot_controller_api.Persistence
{
    /// <summary>
    /// Provides data access methods for retrieving map data from a PostgreSQL database.
    /// </summary>
    public class MapDataAccess
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapDataAccess"/> class.
        /// </summary>
        /// <param name="configuration">The configuration object used to retrieve the database connection string.</param>
        public MapDataAccess(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("PostgresConnection");
        }

        /// <summary>
        /// Retrieves a list of maps from the database.
        /// </summary>
        /// <returns>A list of <see cref="Map"/> objects representing the maps in the database.</returns>
        public List<Map> GetMaps()
        {
            var maps = new List<Map>();

            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM map", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                maps.Add(new Map
                {
                    Id = reader.GetInt32(0),
                    Rows = reader.GetInt32(2),
                    Columns = reader.GetInt32(3),
                    CreatedDate = reader.GetDateTime(4),
                    ModifiedDate = reader.GetDateTime(5),
                });
            }
            return maps;
        }
    }
}