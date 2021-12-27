using System;
using System.Diagnostics.CodeAnalysis;

namespace Forum.Data.Db
{
	public class DbForumSection
	{
		public Guid Id { get; set; }
		
		public Guid? ParentId { get; set; }
		
		public SectionType Type { get; set; }
		
		[NotNull]
		public string Name { get; set; }
		
		public DateTime DateCreated { get; set; }
		
		[AllowNull]
		public string Description { get; set; }
		
		public Guid AuthorId { get; set; }
		
	}
}