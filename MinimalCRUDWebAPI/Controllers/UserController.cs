using Microsoft.AspNetCore.Mvc;
using MinimalCRUDWebAPI.Entities;
using MinimalCRUDWebAPI.Persistence;

namespace MinimalCRUDWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        private async Task<List<UserEntity>> GetUsers() =>
            await _context.Users.ToListAsync();

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await GetUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _context.Users.FindAsync(id) is UserEntity entity ?
                    Results.Ok(entity) :
                    Results.NotFound("Sorry, user not found.");

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserEntity user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var users = await GetUsers();

            return Ok(users);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(UserEntity user, int id)
        {
            var dbUser = await _context.Users.FindAsync(id);
            if (dbUser == null)
                return NotFound("Sorry, user not found.");

            dbUser.FirstName = user.FirstName;
            dbUser.LastName = user.LastName;
            await _context.SaveChangesAsync();
            var users = await GetUsers();

            return Ok(users);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dbUser = await _context.Users.FindAsync(id);
            if (dbUser == null)
                return NotFound("Sorry, user not found.");

            _context.Users.Remove(dbUser);
            await _context.SaveChangesAsync();
            var users = await GetUsers();

            return Ok(users);
        }
    }
}
