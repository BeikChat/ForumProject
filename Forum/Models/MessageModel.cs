using System;
using Forum.Data.Db;

namespace Forum.Models
{
	public class MessageModel
	{
		public Guid ForumSectionId { get; set; }
		public string AuthorName { get; set; }
		
		public string Text { get; set; }
		
		public DateTime DateCreated { get; set; }
	}
}