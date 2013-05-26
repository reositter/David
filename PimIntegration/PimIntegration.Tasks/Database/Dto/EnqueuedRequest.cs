using System;

namespace PimIntegration.Tasks.Database.Dto
{
	public class EnqueuedRequest
	{
		public int MessageId { get; set; }
		public string PrimaryAction { get; set; }
		public string SecondaryAction { get; set; }
		public string RequestItem { get; set; }
		public DateTime EnqueuedAt { get; set; }
	}
}