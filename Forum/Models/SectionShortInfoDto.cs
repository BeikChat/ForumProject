using System;
using System.Diagnostics.CodeAnalysis;

namespace Forum.Models
{
	public class SectionShortInfoDto
	{
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