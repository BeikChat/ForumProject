using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Forum.Models;

namespace Forum.Controllers
{
	public class TopicController : Controller
	{
		private readonly ILogger<TopicController> _logger;

		public TopicController(ILogger<TopicController> logger)
		{
			_logger = logger;
		}

		public IActionResult Topic()
		{
			return View();
		}

		public IActionResult TopicsList()
		{
			var model = new TopicListModel();
			return View(model);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}