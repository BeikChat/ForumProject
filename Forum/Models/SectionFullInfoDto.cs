using System.Collections.Generic;

namespace Forum.Models
{
	public class SectionFullInfoDto
	{
		public SectionShortInfoDto ShortInfo { get; set; }
		
		public List<MessageModel> Messages { get; set; }
	}
}