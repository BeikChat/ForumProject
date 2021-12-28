using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace Forum.Data.Db
{
	[Table("ForumSection")]
	public class DbForumSection
	{
		[Key]
		public Guid Id { get; set; }
		
		[Column]
		public Guid? ParentId { get; set; }
		
		[Column]
		public string Type { get; set; }
		
		[Column, NotNull]
		public string Name { get; set; }
		
		[Column]
		public DateTime DateCreated { get; set; }
		
		[Column, AllowNull]
		public string Description { get; set; }
		
		[Column]
		public string AuthorId { get; set; }
		
	}
}