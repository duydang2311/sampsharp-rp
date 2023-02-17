namespace Server.I18N.Localization.Models;

public interface ILocalizedText : ILocalizedBadge
{
	object Chat_CommandNotFound { get; }
	object Chat_CommandDenied { get; }
	object Chat_Message { get; }
	object Account_Authentication_Loading { get; }
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
	object DoorCommand_Entrance_Help { get; }
	object DoorCommand_Entrance_InvalidId { get; }
	object DoorCommand_Entrance_InvalidDoorType { get; }
	object DoorCommand_Entrance_Success { get; }
	object DoorCommand_Exit_Help { get; }
	object DoorCommand_Exit_InvalidId { get; }
	object DoorCommand_Exit_Success { get; }
	object DoorCommand_Exit_InvalidDoorType { get; }
	object LanguageCommand_Help { get; }
	object LanguageCommand_Options { get; }
	object LanguageCommand_Success { get; }
	object MeCommandText { get; }
	object MeCommandHelp { get; }
	object DoCommandText { get; }
	object DoCommandHelp { get; }
	object ShoutCommand_Text { get; }
	object ShoutCommand_Help { get; }
	object LowCommand_Text { get; }
	object LowCommand_Help { get; }
	object AmeCommand_Text { get; }
	object AmeCommand_Help { get; }

}
