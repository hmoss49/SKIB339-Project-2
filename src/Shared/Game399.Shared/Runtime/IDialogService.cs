using Game399.Shared.Runtime.Models;
using System.Collections.Generic;

namespace Game399.Shared.Runtime
{
    public interface IDialogService
    {
        void RegisterCharacterDialogs(Character character, List<DialogNode> dialogs);
        DialogNode GetCurrentDialog(Character character);
        void SelectOption(Character character, int optionIndex);
        bool IsDialogComplete(Character character);
        int GetTotalDialogCount(Character character);
    }
}