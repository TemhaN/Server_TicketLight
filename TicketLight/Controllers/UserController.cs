using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketLightAPI.Data;
using TicketLightAPI.Models;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace TicketLightAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TicketLightContext _context;

        public UserController(TicketLightContext context)
        {
            _context = context;
        }

        // 📌 Регистрация пользователя
        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                return BadRequest("Email уже зарегистрирован");

            user.PasswordHash = HashPassword(user.PasswordHash);
            user.RegistrationDate = DateTime.UtcNow;
            user.Role = "User";

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Создание сессии сразу после регистрации
            var session = new UserSession
            {
                UserId = user.UserId,
                Token = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddHours(2),
                IsActive = true
            };

            _context.UserSessions.Add(session);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Пользователь зарегистрирован", token = session.Token, user });
        }


        // 📌 Авторизация
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User userData)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userData.Email);
            if (user == null || user.PasswordHash != HashPassword(userData.PasswordHash))
                return Unauthorized("Неверный email или пароль");

            var session = new UserSession
            {
                UserId = user.UserId,
                Token = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddHours(2),
                IsActive = true
            };

            _context.UserSessions.Add(session);
            await _context.SaveChangesAsync();

            return Ok(new { token = session.Token, user });
        }

        // 📌 Получение категорий льгот
        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<BenefitCategory>>> GetCategories()
        {
            return await _context.BenefitCategories.ToListAsync();
        }

        // 📌 Просмотр своей заявки
        [HttpGet("{userId}/application")]
        public async Task<IActionResult> GetUserApplication(int userId)
        {
            var application = await _context.Applications
                .Include(a => a.Category)
                .FirstOrDefaultAsync(a => a.UserId == userId);

            if (application == null)
                return NotFound("Заявка не найдена");

            return Ok(application);
        }

        // 📌 Получение билета, если заявка одобрена
        [HttpGet("{userId}/ticket")]
        public async Task<IActionResult> GetUserTicket(int userId)
        {
            var application = await _context.Applications.FirstOrDefaultAsync(a => a.UserId == userId);

            if (application == null)
                return NotFound("Заявка не найдена");

            if (application.Status != "Одобрено")
                return BadRequest("Заявка ещё не одобрена");

            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.ApplicationId == application.ApplicationId);

            if (ticket == null)
                return NotFound("Билет не найден");

            return Ok(ticket);
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
