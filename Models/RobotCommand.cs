using System;
using System.Text.Json.Serialization;
namespace robot_controller_api;

/**
 * Represents a command for a robot with properties to define its characteristics and state.
 */
public class RobotCommand
{
    /// <summary>
    /// Gets or sets the unique identifier for the robot command.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the robot command.
    /// </summary>
    public string Name { get; set; }    

    /// <summary>
    /// Gets or sets the description of the robot command. This property is optional.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the command is a movement command.
    /// </summary>
    public bool IsMoveCommand { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the command was created. Defaults to the current date and time.
    /// </summary>
    public DateTime CreatedDate { get; set; } =  DateTime.Now;

    /// <summary>
    /// Gets or sets the date and time when the command was last modified. Defaults to the current date and time.
    /// </summary>
    public DateTime ModifiedDate { get; set; } =  DateTime.Now;

    /// <summary>
    /// Initializes a new instance of the <see cref="RobotCommand"/> class.
    /// </summary>
    public RobotCommand(){}

    /// <summary>
    /// Initializes a new instance of the <see cref="RobotCommand"/> class with specified parameters.
    /// </summary>
    /// <param name="id">The unique identifier for the robot command.</param>
    /// <param name="name">The name of the robot command.</param>
    /// <param name="isMoveCommand">Indicates whether the command is a movement command.</param>
    /// <param name="createdDate">The date and time when the command was created.</param>
    /// <param name="modifiedDate">The date and time when the command was last modified.</param>
    /// <param name="description">An optional description of the robot command.</param>
    public RobotCommand(int id, string name, bool isMoveCommand, DateTime createdDate, DateTime modifiedDate, string? description = null)
    {
        Id = id;
        Name = name;
        IsMoveCommand = isMoveCommand;
        CreatedDate = createdDate;
        ModifiedDate = modifiedDate;
        Description = description;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RobotCommand"/> class with a specified identifier and name.
    /// </summary>
    /// <param name="id">The unique identifier for the robot command.</param>
    /// <param name="name">The name of the robot command.</param>
    public RobotCommand(int id, string name)
    {
        Id = id;
        Name = name;
        IsMoveCommand = false;
        CreatedDate = DateTime.Now;
        ModifiedDate = DateTime.Now;
        Description = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RobotCommand"/> class using JSON constructor.
    /// </summary>
    /// <param name="id">The unique identifier for the robot command.</param>
    /// <param name="name">The name of the robot command.</param>
    /// <param name="isMoveCommand">Indicates whether the command is a movement command.</param>
    /// <param name="description">An optional description of the robot command.</param>
    [JsonConstructor]
    public RobotCommand(int id, string name, bool isMoveCommand, string? description = null)
    {
        Id = id;
        Name = name;
        IsMoveCommand = isMoveCommand;
        CreatedDate = DateTime.Now;
        ModifiedDate = DateTime.Now;
        Description = description;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RobotCommand"/> class with a specified identifier, name, and optional description.
    /// </summary>
    /// <param name="id">The unique identifier for the robot command.</param>
    /// <param name="name">The name of the robot command.</param>
    /// <param name="description">An optional description of the robot command.</param>
    public RobotCommand(int id, string name, string? description = null)
    {
        Id = id;
        Name = name;
        IsMoveCommand = false;
        CreatedDate = DateTime.Now;
        ModifiedDate = DateTime.Now;
        Description = description;
    }
}