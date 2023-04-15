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
using SeaBattleAPI.Tools;

namespace SeaBattleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly user50_battleContext _context;

        public User Player { get; set; }

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
        public async Task<ActionResult<User>> RegistrationUser(User user)
        {
            try
            {
                MailAddress mailAddress;
                try
                {
                    mailAddress = new MailAddress(user.Email);
                }
                catch
                {
                    return BadRequest("Неверный адрес алектронной почты");
                }

                Player = await _context.Users.FirstOrDefaultAsync(s => s.Email == user.Email);
                if (Player == null)
                {
                    Player = new User { Email = user.Email, Login = user.Email.Split('@')[0], Password = Hash.HashPass(user.Password) };
                    _context.Users.Add(Player);
                    await _context.SaveChangesAsync();
                }
            }
            catch
            {
                return BadRequest("Ошибка связи с БД");
            }
            return Player;
        }

        [HttpPost("SignIn")]
        public async Task<ActionResult<User>> SignInUser(User user)
        {
            if(!string.IsNullOrEmpty(user.Login) || !string.IsNullOrEmpty(user.Password))
            {
                try
                {
                    Player = await _context.Users.FirstOrDefaultAsync(s => s.Login == user.Login && s.Password == Hash.HashPass(user.Password));
                    if (Player == null)
                    {
                        return BadRequest("Неверный логин или пароль");
                    }
                }
                catch
                {
                    return BadRequest("Ошибка связи с БД");
                }
            }
            else
            {
                BadRequest("Не все поля заполнены");
            }
            return Player;
        }


        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
