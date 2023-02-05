namespace Server.I18N.Localization.Models;

public interface ITextNameFakeModel
{
	object Badge_System { get; }
	object Badge_CommandUsage { get; }
	object Badge_System { get; }
	object CommandNotFound { get; }
	object CommandDenied { get; }
	object ChatMessage { get; }
	object DoorCommand_Help { get; }
	object DoorCommand_Options { get; }
	object DoorCommand_Create_Help { get; }
	object DoorCommand_Create_Options { get; }
	object DoorCommand_Create_NoEffect { get; }
	object DoorCommand_Create_Success { get; }
	object DoorCommand_Destroy_Help { get; }
	object DoorCommand_Destroy_InvalidId { get; }
	object DoorCommand_Destroy_SuccessNoEffect { get; }
	object DoorCommand_Destroy_Success { get; }
	object DoorCommand_Nearby_Empty { get; }
	object DoorCommand_Nearby_Found { get; }
	object DoorCommand_Nearby_ForEachInfo { get; }
}
