using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Forum.Data;
using Forum.Data.Db;
using Forum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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

		public async Task<IActionResult> Index(string sectionName)
		{
			var selectedSection = await _context.ForumSections
				.SingleOrDefaultAsync(x => x.Name == sectionName);

			
			
			var sections = await _context.ForumSections
				.Where(x => (selectedSection == null && !x.ParentId.HasValue) 
				             || (selectedSection != null && x.ParentId == selectedSection.Id))
				.Select(x => new SectionShortInfoDto
				{
					Id = x.Id,
					Name = x.Name,
					Type = x.Type,
					ParentName = null,
					AuthorName = "Кто-то",
					Description = x.Description,
					
				})
				.ToListAsync();

			var childByParentSections = new Dictionary<Guid,SectionShortInfoDto[]>();
			if (childByParentSections == null) throw new ArgumentNullException(nameof(childByParentSections));

			foreach (var section in sections)
			{
				var child = await _context.ForumSections
					.Where(x => x.ParentId == section.Id)
					.Select(x => new SectionShortInfoDto
					{
						Id = x.Id,
						Name = x.Name,
						Type = x.Type,
						ParentName = x.ParentId.ToString(),
						AuthorName = _context.Users.FirstOrDefault(y => y.Id == x.AuthorId).UserName,
						Description = x.Description
					})
					.ToArrayAsync();
				
				childByParentSections.Add(section.Id, child);
			}

			ViewBag.childSections = childByParentSections;
			ViewBag.selectedSectionName = selectedSection?.Name;

			return View("Index", sections);
		}

		public async Task<IActionResult> CreateSection(string sectionType)
		{
			if (!Enum.TryParse(sectionType, out SectionType type))
				type = SectionType.NotSet;
			
			return View(new SectionModel { Type = type.ToString()});
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateSection(
			[Bind("Name,Description,ParentName,Type")]SectionModel sectionModel)
		{
			if(sectionModel == null || !ModelState.IsValid)
				return new BadRequestResult();
			
			if (User.Identity is { IsAuthenticated: false })
				return new UnauthorizedResult();

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			try
			{
				DbForumSection parent = await _context.ForumSections
						.Where(x => x.Name == sectionModel.ParentName)
						.FirstOrDefaultAsync();

				DbForumSection sectionToSave = new()
					{
						Id = Guid.NewGuid(),
						DateCreated = DateTime.Now,
						Description = sectionModel.Description,
						Name = sectionModel.Name,
						AuthorId = userId,
						ParentId = parent?.Id,
						Type = sectionModel.Type
					};
					
					_context.Add(sectionToSave);
					await _context.SaveChangesAsync();
					
					return RedirectToAction(nameof(View), routeValues: new { sectionId = sectionToSave.Id} );// new RedirectResult("Section/Index", true);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Error in save changes" );
				ModelState.AddModelError("","Unable to save changes. " +
				                            "Try again, and if the problem persists " +
				                            "see your system administrator.");
				return RedirectToAction(nameof(Error));
			}
		}

		[ActionName("View")]
		public async Task<IActionResult> SectionView(Guid sectionId)
		{
			var section = await _context.ForumSections
				.Where(x => x.Id == sectionId)
				.FirstOrDefaultAsync();

			if (section == null)
				return RedirectToActionPermanent(nameof(Error));

			if (section.Type == SectionType.Section.ToString())
				return await Index(section.Name);
			
			var parent = await _context.ForumSections
				.Where(x => x.Id == section.ParentId)
				.FirstOrDefaultAsync();

			var author = await _context.Users
				.Where(x => x.Id == section.AuthorId)
				.FirstOrDefaultAsync();

			var messages = _context.Messages
				.Where(x => x.ForumSectionId == sectionId)
				.OrderBy(x => x.DateCreated)
				.ToList()
				.Select(x => new MessageModel
				{
					AuthorName = _context.Users
						.First(y => y.Id == x.AuthorId.ToString()).UserName,
					Text = x.Text,
					DateCreated = x.DateCreated
				})
				.ToList();

			ViewBag.sectionId = sectionId;
			
			return View("View", new SectionFullInfoDto()
			{
				ShortInfo = new SectionShortInfoDto
				{
					Id = sectionId,
					Name = section?.Name,
					ParentName = parent?.Name,
					AuthorName = author?.UserName,
					Description = section?.Description
				},
				Messages = messages
			});
		}

		[ActionName("Message")]
		public async Task<IActionResult> GetMessage(Guid messageId)
		{
			var message = await _context.Messages
				.Where(x => x.Id == messageId)
				.Select(x => new MessageModel
				{
					Text = x.Text,
					DateCreated = x.DateCreated,
					AuthorName = "Now empty"
				})
				.FirstOrDefaultAsync();
			
			return PartialView("Shared/_Message");
		}
		
		public IActionResult Error()
		{
			return View(new ErrorViewModel{ RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}