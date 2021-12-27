using System;

namespace Forum.Data.Db
{
	public class DbMessage
	{
		public Guid Id { get; set; }
		
		public Guid AuthorId { get; set; }
		
		public Guid ForumSectionId { get; set; }
		
		public string Text { get; set; }
		
		public DateTime DataCreated { get; set; }
	}
}