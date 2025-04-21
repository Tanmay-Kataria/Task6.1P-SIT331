using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using robot_controller_api;
using robot_controller_api.Persistence;

namespace robot_controller_api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly UserDataAccess _context;
        
        public UserController(UserDataAccess context)
        {
            _context = context;
        }

        // 1. GET /users - Get all users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // 2. GET /users/admin - Get users in the Admin role
        [HttpGet("admin")]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetAdminUsers()
        {
            var admins = await _context.Users
                .Where(u => u.Role != null && u.Role.ToLower() == "admin")
                .ToListAsync();
            return Ok(admins);
        }

        // 3. GET /users/{id} - Get a user by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        // 4. POST /users - Add a new user (register new users)
        [HttpPost]
        public async Task<ActionResult<UserModel>> AddUser(UserModel user)
        {
            // Assume the incoming user.PasswordHash property contains the plain text password.
            var plainPassword = user.PasswordHash; // plain text password provided by client
            var pwHash = BCrypt.Net.BCrypt.HashPassword(plainPassword);
            user.PasswordHash = pwHash;
            
            // Set created and modified dates.
            user.CreatedDate = DateTime.UtcNow;
            user.ModifiedDate = DateTime.UtcNow;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        // 5. PUT /users/{id} - Update user (disregarding email and password)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserModel updatedUser)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            // Update only allowed fields (ignore email and password)
            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Description = updatedUser.Description;
            user.Role = updatedUser.Role;
            user.ModifiedDate = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // 6. DELETE /users/{id} - Delete a user by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // 7. PATCH /users/{id} - Update email and password using LoginModel
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateEmailAndPassword(int id, [FromBody] LoginModel login)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            // Update email and hash the new password using BCrypt.
            user.Email = login.Email;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(login.Password);
            user.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
