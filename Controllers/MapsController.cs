using Microsoft.AspNetCore.Mvc;

namespace robot_controller_api.Controllers;

[ApiController]
[Route("api/maps")]
public class MapsController : ControllerBase
{
    // Static list to simulate persistent storage for maps
    private static readonly List<Map> _maps = new List<Map>
    {
        new Map { Id = 1, Columns = 5, Rows = 5, Description = "Square map", CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now },
        new Map { Id = 2, Columns = 5, Rows = 3, Description = "Rectangular map", CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now }
    };

    /// <summary>
    /// Retrieves all maps.
    /// </summary>
    /// <returns>An enumerable collection of maps.</returns>
    [HttpGet]
    public IEnumerable<Map> GetAllMaps() => _maps;

    /// <summary>
    /// Retrieves all square maps where the number of columns equals the number of rows.
    /// </summary>
    /// <returns>An enumerable collection of square maps.</returns>
    [HttpGet("square")]
    public IEnumerable<Map> GetSquareMaps() => _maps.Where(m => m.Columns == m.Rows);

    /// <summary>
    /// Retrieves a map by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the map.</param>
    /// <returns>An IActionResult containing the map if found, or a NotFound result if not.</returns>
    [HttpGet("{id}")]
    public IActionResult GetMapById(int id)
    {
        var map = _maps.FirstOrDefault(m => m.Id == id);
        if (map == null)
        {
            return NotFound();
        }
        return Ok(map);
    }

    /// <summary>
    /// Adds a new map to the collection.
    /// </summary>
    /// <param name="newMap">The map object to be added.</param>
    /// <returns>An IActionResult indicating the result of the operation.</returns>
    [HttpPost]
    public IActionResult AddMap([FromBody] Map newMap)
    {
        if (newMap == null)
        {
            return BadRequest();
        }

        // Generate new ID and set dates
        int newId = _maps.Any() ? _maps.Max(m => m.Id) + 1 : 1;
        newMap.Id = newId;
        newMap.CreatedDate = DateTime.Now;
        newMap.ModifiedDate = DateTime.Now;

        _maps.Add(newMap);
        return CreatedAtAction(nameof(GetMapById), new { id = newMap.Id }, newMap);
    }

    /// <summary>
    /// Updates an existing map identified by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the map to be updated.</param>
    /// <param name="updatedMap">The updated map object.</param>
    /// <returns>An IActionResult indicating the result of the operation.</returns>
    [HttpPut("{id}")]
    public IActionResult UpdateMap(int id, [FromBody] Map updatedMap)
    {
        if (updatedMap == null || id != updatedMap.Id)
        {
            return BadRequest();
        }

        var existingMap = _maps.FirstOrDefault(m => m.Id == id);
        if (existingMap == null)
        {
            return NotFound();
        }

        // Update properties
        existingMap.Columns = updatedMap.Columns;
        existingMap.Rows = updatedMap.Rows;
        existingMap.Description = updatedMap.Description;
        existingMap.ModifiedDate = DateTime.Now;

        return NoContent();
    }

    /// <summary>
    /// Deletes a map identified by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the map to be deleted.</param>
    /// <returns>An IActionResult indicating the result of the operation.</returns>
    [HttpDelete("{id}")]
    public IActionResult DeleteMap(int id)
    {
        var map = _maps.FirstOrDefault(m => m.Id == id);
        if (map == null)
        {
            return NotFound();
        }

        _maps.Remove(map);
        return NoContent();
    }

    /// <summary>
    /// Checks if the specified coordinates are within the bounds of the map.
    /// </summary>
    /// <param name="id">The unique identifier of the map.</param>
    /// <param name="x">The x-coordinate to check.</param>
    /// <param name="y">The y-coordinate to check.</param>
    /// <returns>An IActionResult indicating whether the coordinates are within bounds.</returns>
    [HttpGet("{id}/{x}-{y}")]
    public IActionResult CheckCoordinate(int id, int x, int y)
    {
        // Validate coordinate values
        if (x < 0 || y < 0)
        {
            return BadRequest("Coordinates cannot be negative.");
        }

        var map = _maps.FirstOrDefault(m => m.Id == id);
        if (map == null)
        {
            return NotFound();
        }

        // Check if the coordinate is within the map bounds
        bool isWithinBounds = (x < map.Columns) && (y < map.Rows);
        return Ok(isWithinBounds);
    }
}
