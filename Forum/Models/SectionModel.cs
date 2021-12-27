using System.Collections.Generic;
using Forum.Data;

namespace Forum.Models
{
	public class SectionModel
	{
		public string Name { get; set; }

		public string ParentName { get; set; }
		
		public string Description { get; set; }

		public virtual SectionType Type => SectionType.Section;
		
		public List<string> InnerSection { get; set; }
	}
}