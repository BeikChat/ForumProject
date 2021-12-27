using System.Collections.Generic;
using Forum.Data;

namespace Forum.Models
{
	public class TopicModel : SectionModel
	{
		public override SectionType Type => SectionType.Topic;
	}
}