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

	object Dialog_Character_Selection_Caption { get; }
	object Dialog_Character_Selection_Header_Column1 { get; }
	object Dialog_Character_Selection_Header_Column2 { get; }
	object Dialog_Character_Selection_Row_Column1 { get; }
	object Dialog_Character_Selection_Row_Column2 { get; }
	object Dialog_Character_Selection_RowNewChar_Column1 { get; }
	object Dialog_Character_Selection_RowNewChar_Column2 { get; }
	object Dialog_Character_Selection_Button1 { get; }
	object Dialog_Character_Selection_Button2 { get; }
	object Dialog_Character_Creation_Name_Caption { get; }
	object Dialog_Character_Creation_Name_Content { get; }
	object Dialog_Character_Creation_Name_Button1 { get; }
	object Dialog_Character_Creation_Name_Button2 { get; }
	object Dialog_Character_Creation_Gender_Caption { get; }
	object Dialog_Character_Creation_Gender_Button1 { get; }
	object Dialog_Character_Creation_Gender_Button2 { get; }
	object Dialog_Character_Creation_Gender_Header_Column1 { get; }
	object Dialog_Character_Creation_Gender_Row_Male { get; }
	object Dialog_Character_Creation_Gender_Row_Female { get; }
	object Dialog_Character_Creation_Age_Caption { get; }
	object Dialog_Character_Creation_Age_Content { get; }
	object Dialog_Character_Creation_Age_Button1 { get; }
	object Dialog_Character_Creation_Age_Button2 { get; }
}
