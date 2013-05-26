using System;

namespace PimIntegration.Tasks.Database.Dto
{
	public class PimRequestLogItem
	{
		public int MessageId { get; set; }
		public string PrimaryAction { get; set; }
		public string SecondaryAction { get; set; }
		public DateTime EnqueuedAt { get; set; }
		public DateTime? DequeuedAt { get; set; }
		public int NumberOfFailedAttemptsToDequeue { get; set; }
		public int? Status { get; set; }
		public string ErrorDetails { get; set; }
	}
}