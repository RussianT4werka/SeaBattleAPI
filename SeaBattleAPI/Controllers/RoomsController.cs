using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SeaBattleAPI.DB;
using SeaBattleAPI.Models;
using SeaBattleAPI.Tools;

namespace SeaBattleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IHubContext<MainHub> _hub;
        private readonly user50_battleContext _context;

        public RoomsController(user50_battleContext context, IHubContext<MainHub> hubContext)
        {
            _context = context;
            _hub = hubContext;
        }

        // GET: api/Rooms
        [HttpPost("GetRooms")]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms(int userId)
        {
            if (_context.Rooms == null)
            {
                return NotFound("Комнат нет, но вы создайте");
            }
            return await _context.Rooms.Include(s => s.Status).Include(s => s.UserCreator).Include(s => s.UserSlow).ToListAsync();
        }

        // GET: api/Rooms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            if (_context.Rooms == null)
            {
                return NotFound();
            }
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
            {
                return NotFound();
            }

            return room;
        }

        // PUT: api/Rooms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoom(int id, Room room)
        {
            if (id != room.Id)
            {
                return BadRequest();
            }

            _context.Entry(room).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(id))
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

        // POST: api/Rooms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateRoom")]
        public async Task<ActionResult<Room>> CreateRoom(User user)
        {
            if (_context.Rooms == null)
            {
                return Problem("Entity set 'user50_battleContext.Rooms'  is null.");
            }
            var room = new Room { UserCreatorId = user.Id, StatusId = 1 };
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            Room room1 = _context.Rooms.ToList().Last();

            await _hub.Clients.All.SendAsync("AddRoom", room1);

            return Ok(room);
        }

        [HttpPost("JoinRoom/{userID}/{roomID}")]
        public async Task<ActionResult<Room>> JoinRoom([FromRoute]int userID, [FromRoute]int roomID)
        {

            if (_context.Rooms == null)
            {
                return Problem("Entity set 'user50_battleContext.Rooms'  is null.");
            }
            var room = _context.Rooms.Find(roomID);
            if(room == null)
            {
                return BadRequest("Комнаты нет :)");
            }
            room.UserSlowId = userID;
            room.StatusId = 4;
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();

            await _hub.Clients.All.SendAsync("UpdateRoom", room);

            return Ok(room);
        }

        // DELETE: api/Rooms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            if (_context.Rooms == null)
            {
                return NotFound();
            }
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoomExists(int id)
        {
            return (_context.Rooms?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
