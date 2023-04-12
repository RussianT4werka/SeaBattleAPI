using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeaBattleAPI.DB;
using SeaBattleAPI.Models;

namespace SeaBattleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly user50_battleContext _context;

        public UsersController(user50_battleContext context)
        {
            _context = context;
        }

        

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPost("Registration")]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            MailAddress mailAddress;
            try
            {
                mailAddress = new MailAddress(user.Email);
            }
            catch
            {
                return BadRequest("email shit");
            }

            var player = await _context.Users.FirstOrDefaultAsync(s => s.Email == user.Email);
            if (player == null)
            {
                player = new User {Email = user.Email, Login = user.Email.Split('@')[0], Password = user.Password};
                _context.Users.Add(player);
                await _context.SaveChangesAsync();
            }
            return player;
        }

        [HttpPost("SignIn")]

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
