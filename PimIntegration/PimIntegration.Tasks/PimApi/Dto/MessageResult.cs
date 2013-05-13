using System;

namespace PimIntegration.Tasks.PimApi.Dto
{
	public class MessageResult
	{
		public int MessageId { get; set; }
 		public string PrimaryAction { get; private set; }
		public string SecondaryAction { get; private set; }
		public DateTime EnqueuedAt { get; set; }
		public DateTime? DequeuedAt { get; set; }
		public int NumberOfFailedAttemptsToDequeue { get; set; }
		public MessageStatus Status { get; set; }

		public MessageResult(string primaryAction, string secondaryAction)
		{
			PrimaryAction = primaryAction;
			SecondaryAction = secondaryAction;
		}
	}

	public enum MessageStatus
	{
		Enqueued = 100,
		Completed = 200,
		NoResponseFound = 404
	}
}