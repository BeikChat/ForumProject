using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Forum.Data;
using Forum.Data.Db;
using Forum.Models;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Forum.Controllers
{
	public class MessageController : Controller
	{
		private readonly ILogger<MessageController> _logger;

		private readonly ApplicationDbContext _context;

		public MessageController(ILogger<MessageController> logger, ApplicationDbContext context)
		{
			_logger = logger;
			_context = context;
		}

		[HttpPost]
		public async Task<IActionResult> CreateMessage(MessageModel message)
		{
			if (message == null || !ModelState.IsValid)
				return new BadRequestResult();
			
			if (User.Identity is { IsAuthenticated: false })
				return new UnauthorizedResult();

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var toSave = new DbMessage
			{
				Id = Guid.NewGuid(),
				AuthorId = userId,
				ForumSectionId = message.ForumSectionId,
				Text = message.Text,
				DateCreated = DateTime.Now
			};
			await _context.AddAsync(toSave);
			await _context.SaveChangesAsync();
			
			return new RedirectToActionResult("View", "Section", new{sectionId = toSave.ForumSectionId});
		}
	}
}