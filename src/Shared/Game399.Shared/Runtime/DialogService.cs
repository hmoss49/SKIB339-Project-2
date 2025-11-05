using Game399.Shared.Runtime.Models;
using System.Collections.Generic;

namespace Game399.Shared.Runtime
{
    public class DialogService : IDialogService
    {
        private readonly IGameLog _gameLog;
        private readonly Dictionary<Character, List<DialogNode>> _characterDialogs;

        public DialogService(IGameLog gameLog)
        {
            _gameLog = gameLog;
            _characterDialogs = new Dictionary<Character, List<DialogNode>>();
        }

        public void RegisterCharacterDialogs(Character character, List<DialogNode> dialogs)
        {
            _characterDialogs[character] = dialogs;
            _gameLog.Info($"Registered {dialogs.Count} dialog nodes for {character.Name}");
        }

        public DialogNode GetCurrentDialog(Character character)
        {
            if (!_characterDialogs.ContainsKey(character))
            {
                _gameLog.Warning($"No dialogs registered for character: {character.Name}");
                return null;
            }

            var dialogs = _characterDialogs[character];
            int currentIndex = character.CurrentDialogIndex.Value;

            if (currentIndex >= dialogs.Count)
            {
                _gameLog.Info($"Dialog complete for {character.Name}");
                return null;
            }

            return dialogs[currentIndex];
        }

        public void SelectOption(Character character, int optionIndex)
        {
            var currentDialog = GetCurrentDialog(character);
            if (currentDialog == null)
            {
                _gameLog.Warning($"Cannot select option - no current dialog for {character.Name}");
                return;
            }

            if (optionIndex < 0 || optionIndex >= currentDialog.Options.Count)
            {
                _gameLog.Error($"Invalid option index: {optionIndex}");
                return;
            }

            var selectedOption = currentDialog.Options[optionIndex];
            
            // Apply affection change
            character.Affection.Value += selectedOption.AffectionChange;
            
            _gameLog.Info($"{character.Name} - Selected: '{selectedOption.Text}' (Affection change: {selectedOption.AffectionChange:+#;-#;0}, Total: {character.Affection.Value})");

            // Move to next dialog
            character.CurrentDialogIndex.Value++;
        }

        public bool IsDialogComplete(Character character)
        {
            if (!_characterDialogs.ContainsKey(character))
            {
                return true;
            }

            return character.CurrentDialogIndex.Value >= _characterDialogs[character].Count;
        }

        public int GetTotalDialogCount(Character character)
        {
            if (!_characterDialogs.ContainsKey(character))
            {
                return 0;
            }

            return _characterDialogs[character].Count;
        }
    }
}