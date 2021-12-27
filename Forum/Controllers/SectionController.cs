using System;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Forum.Data;
using Forum.Data.Db;
using Forum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Forum.Controllers
{
	public class SectionController : Controller
	{
		private readonly ILogger<SectionController> _logger;

		private readonly ApplicationDbContext _context;

		public SectionController(ILogger<SectionController> logger, ApplicationDbContext context)
		{
			_logger = logger;
			_context = context;
		}

		public IActionResult Index()
		{
			var sections = _context.ForumSections
				.Where(x => x.ParentId == null)
				.ToList();
			return View(sections);
		}

		public async Task<IActionResult> CreateTopic()
		{
			return View(new TopicModel());
		}
		
		public async Task<IActionResult> CreateSection()
		{
			return View(new SectionModel());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateTopic(
			[Bind("Name,Description,ParentName")]TopicModel topicModel)
		{
			if (topicModel == null)
				return View(new TopicModel());
			
			try
			{
				if (ModelState.IsValid)
				{
					DbForumSection sectionToSave = new()
					{
						DateCreated = DateTime.Now,
						Description = topicModel.Description,
						Name = topicModel.Name,
						ParentId = null,
						Type = topicModel.Type
					};
					_context.Add(sectionToSave);
					await _context.SaveChangesAsync();
					return RedirectToPage(nameof(Index));
				}

			}
			catch (Exception e)
			{
				_logger.LogError(e, "Error when try save new topic");
				ModelState.AddModelError("","Unable to save changes. " +
	                "Try again, and if the problem persists " +
	                "see your system administrator.");
			}
			return View(topicModel);
		}
		
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateSection(
			[Bind("Name,Description,ParentName")]SectionModel sectionModel)
		{
			if(sectionModel == null)
				return View(new SectionModel());

			try
			{
				if (ModelState.IsValid)
				{
					DbForumSection parent = await _context.ForumSections
						.Where(x => x.Name == sectionModel.ParentName)
						.FirstOrDefaultAsync();

					var userId = Guid.Empty;
					if (User.Identity is { IsAuthenticated: true })
						userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

					DbForumSection sectionToSave = new()
					{
						DateCreated = DateTime.Now,
						Description = sectionModel.Description,
						Name = sectionModel.Name,
						AuthorId = userId,
						ParentId = null,
						Type = sectionModel.Type
					};
					
					_context.Add(sectionToSave);
					await _context.SaveChangesAsync();
					
					return RedirectToPage(nameof(Index));
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Error in save changes" );
				ModelState.AddModelError("","Unable to save changes. " +
				                            "Try again, and if the problem persists " +
				                            "see your system administrator.");
			}
			return View(sectionModel);
		}
	}
}