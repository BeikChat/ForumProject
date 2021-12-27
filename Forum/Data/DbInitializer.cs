using System;
using System.Linq;
using Forum.Data.Db;

namespace Forum.Data
{
	public static class DbInitializer
	{
		public static void Initialize(ApplicationDbContext context)
		{
			context.Database.EnsureCreated();

			if (!context.ForumSections.Any())
				InitializeDbForumSection(context);

			if (!context.Messages.Any())
				InitializeDbMessage(context);
		}

		private static void InitializeDbForumSection(ApplicationDbContext context)
		{
			
		}
		
		private static void InitializeDbMessage(ApplicationDbContext context)
		{
			
		}
	}
}