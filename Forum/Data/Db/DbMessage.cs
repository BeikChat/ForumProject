using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Data.Db
{
	[Table("Message")]
	public class DbMessage
	{
		[Key]
		public Guid Id { get; set; }
		
		[Column]
		public string AuthorId { get; set; }
		
		[Column]
		public Guid ForumSectionId { get; set; }
		
		[Column]
		public string Text { get; set; }
		
		[Column]
		public DateTime DateCreated { get; set; }
	}
}