using System.Text.Json.Serialization;

namespace FCG_Games.Domain.External.Payments.DTO;

public class DurableOrchestrationStatus
{
	[JsonPropertyName("id")]
	public string Id { get; set; }
	[JsonPropertyName("statusQueryGetUri")]
	public string StatusQueryGetUri { get; set; }
	[JsonPropertyName("sendEventPostUri")]
	public string SendEventPostUri { get; set; }
	[JsonPropertyName("terminatePostUri")]
	public string TerminalPostUri { get; set; }
	[JsonPropertyName("rewindPostUri")]
	public string RewindPostUri { get; set; }
	[JsonPropertyName("purgeHistoryDeleteUri")]
	public string PurgeHistoryDeleteUri { get; set; }
	[JsonPropertyName("restartPostUri")]
	public string RestartPostUri { get; set; }
	[JsonPropertyName("suspendPostUri")]
	public string SuspendPostUri { get; set; }
	[JsonPropertyName("resumePostUri")]
	public string ResumePostUri { get; set; }
}
