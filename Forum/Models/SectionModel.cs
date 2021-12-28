using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Forum.Data;
using Forum.Data.Db;

namespace Forum.Models
{
	public class SectionModel
	{
		public SectionModel() {}

		public SectionModel(DbForumSection forumSection)
		{
			Id = forumSection.Id;
			Name = forumSection.Name;
			Description = forumSection.Description;
			Type = forumSection.Type;
		}
		
		public Guid Id { get; set; }
		
		[NotNull]
		public string Name { get; set; }

		[AllowNull]
		public string ParentName { get; set; }
		
		[AllowNull]
		public string Description { get; set; }
		
		[AllowNull]
		public string AuthorName { get; set; }

		[NotNull]
		public string Type { get; set; }
	}
}