using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Persistence;
using System.Collections.Generic;

namespace robot_controller_api.Controllers
{
    [Route("api/robotcommands")]
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
    }
}