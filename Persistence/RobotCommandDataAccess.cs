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
        /// <summary>
        /// Adds a new robot command.
        /// </summary>
        /// <param name="command">The robot command to add.</param>
        /// <returns>The created robot command.</returns>
        public void AddRobotCommand(RobotCommand command)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(@"
        INSERT INTO robotcommand (name, description, ismovecommand, createddate, modifieddate)
        VALUES (@name, @description, @ismovecommand, @createddate, @modifieddate)", conn);

            cmd.Parameters.AddWithValue("name", command.Name);
            cmd.Parameters.AddWithValue("description", (object?)command.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("ismovecommand", command.IsMoveCommand);
            cmd.Parameters.AddWithValue("createddate", command.CreatedDate);
            cmd.Parameters.AddWithValue("modifieddate", command.ModifiedDate);

            cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// Updates an existing robot command.
        /// </summary>
        /// <param name="id">The ID of the command to update.</param>
        /// <param name="command">The updated robot command.</param>
        /// <returns>No content if successful, or NotFound if the ID doesn't exist.</returns>

        public bool UpdateRobotCommand(int id, RobotCommand command)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(@"
        UPDATE robotcommand 
        SET name = @name,
            description = @description,
            ismovecommand = @ismovecommand,
            modifieddate = @modifieddate
        WHERE id = @id", conn);

            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("name", command.Name);
            cmd.Parameters.AddWithValue("description", (object?)command.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("ismovecommand", command.IsMoveCommand);
            cmd.Parameters.AddWithValue("modifieddate", command.ModifiedDate);

            return cmd.ExecuteNonQuery() > 0;
        }
        
    }
}