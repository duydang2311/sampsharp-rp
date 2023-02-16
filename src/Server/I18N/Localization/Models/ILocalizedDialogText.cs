namespace Server.I18N.Localization.Models;

public interface ILocalizedDialogText
{
	object Dialog_Account_SignUp_Caption { get; }
	object Dialog_Account_SignUp_Content { get; }
	object Dialog_Account_SignUp_Button1 { get; }
	object Dialog_Account_SignUp_Button2 { get; }
	object Dialog_Account_Login_Caption { get; }
	object Dialog_Account_Login_Content { get; }
	object Dialog_Account_Login_Button1 { get; }
	object Dialog_Account_Login_Button2 { get; }

	object Dialog_CharacterSelection_Caption { get; }
	object Dialog_CharacterSelection_Header_Column1 { get; }
	object Dialog_CharacterSelection_Header_Column2 { get; }
	object Dialog_CharacterSelection_Row_Column1 { get; }
	object Dialog_CharacterSelection_Row_Column2 { get; }
	object Dialog_CharacterSelection_Button1 { get; }
	object Dialog_CharacterSelection_Button2 { get; }
}
