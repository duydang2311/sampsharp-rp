using SampSharp.Entities.SAMP;

namespace Server.SAMP.Dialog.Services;

public interface IInputDialogBuilder : IContentDialogBuilder<InputDialog, IInputDialogBuilder>
{
	IInputDialogBuilder SetIsPassword(bool isPassword);
}
