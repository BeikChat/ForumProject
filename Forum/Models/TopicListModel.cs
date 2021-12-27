using System.Collections.Generic;

namespace Forum.Models
{
	public class TopicListModel
	{
		public readonly List<string> Topics = new()
		{
			"First", "Second", "Third"
		};
	}
}