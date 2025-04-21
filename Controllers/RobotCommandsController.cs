using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Persistence;
using System.Collections.Generic;

namespace robot_controller_api.Controllers
{
    [Route("api/robot-commands")]
    [ApiController]
    public class RobotCommandsController : ControllerBase
    {
        private readonly RobotCommandDataAccess _dataAccess;

        /// <summary>
        /// Initializes a new instance of the <see cref="RobotCommandsController"/> class.
        /// </summary>
        /// <param name="dataAccess">An instance of <see cref="RobotCommandDataAccess"/> used for data operations.</param>
        public RobotCommandsController(RobotCommandDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        /// <summary>
        /// Retrieves all robot commands from the data source.
        /// </summary>
        /// <returns>An enumerable collection of <see cref="RobotCommand"/> objects.</returns>
        [HttpGet("")]
        public IEnumerable<RobotCommand> GetAllRobotCommands()
        {
            return _dataAccess.GetRobotCommands();
        }

        [HttpGet("{id}")]
        public ActionResult<RobotCommand> GetRobotCommandById(int id)
        {
            var robotCommand = _dataAccess.GetRobotCommands().FirstOrDefault(rc => rc.Id == id);
            if (robotCommand == null)
            {
                return NotFound();
            }
            return Ok(robotCommand);
        }
        [HttpPost]
        public IActionResult AddRobotCommand([FromBody] RobotCommand command)
        {
            command.CreatedDate = DateTime.UtcNow;
            command.ModifiedDate = DateTime.UtcNow;

            _dataAccess.AddRobotCommand(command);
            return CreatedAtAction(nameof(GetRobotCommandById), new { id = command.Id }, command);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRobotCommand(int id, [FromBody] RobotCommand command)
        {
            command.ModifiedDate = DateTime.UtcNow;
            var updated = _dataAccess.UpdateRobotCommand(id, command);
            if (!updated)
                return NotFound();

            return NoContent();
        }
        
    }
}