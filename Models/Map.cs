using System;
namespace robot_controller_api;

/// <summary>
/// Represents a map with specific dimensions and metadata.
/// </summary>
public class Map
{
    /// <summary>
    /// Gets or sets the unique identifier for the map.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the number of columns in the map.
    /// </summary>
    public int Columns { get; set; }

    /// <summary>
    /// Gets or sets the number of rows in the map.
    /// </summary>
    public int Rows { get; set; }

    /// <summary>
    /// Gets or sets the description of the map.
    /// This property is optional and can be null.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the map was created.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the map was last modified.
    /// </summary>
    public DateTime ModifiedDate { get; set; }
}
