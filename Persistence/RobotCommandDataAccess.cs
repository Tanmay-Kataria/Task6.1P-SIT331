using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;

namespace robot_controller_api.Persistence
{
    /// <summary>
    /// Provides data access methods for retrieving robot commands from a PostgreSQL database.
    /// </summary>
    public class RobotCommandDataAccess
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="RobotCommandDataAccess"/> class.
        /// </summary>
        /// <param name="configuration">The configuration object used to retrieve the database connection string.</param>
        public RobotCommandDataAccess(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("PostgresConnection");
        }

        /// <summary>
        /// Retrieves a list of robot commands from the database.
        /// </summary>
        /// <returns>A list of <see cref="RobotCommand"/> objects representing the robot commands.</returns>
        public List<RobotCommand> GetRobotCommands()
        {
            var robotCommands = new List<RobotCommand>();

            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM robotcommand", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                robotCommands.Add(new RobotCommand
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                    IsMoveCommand = reader.GetBoolean(3),
                    CreatedDate = reader.GetDateTime(4),
                    ModifiedDate = reader.GetDateTime(5)
                });
            }
            return robotCommands;
        }
    }
}