using System;

namespace Forum.Models
{
	public class MessageModel
	{
		public string AuthorName { get; set; }
		
		public string Text { get; set; }
		
		public DateTime DateCreated { get; set; }
	}
}