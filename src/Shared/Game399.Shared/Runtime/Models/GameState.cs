using System.Collections.Generic;

namespace Game399.Shared.Runtime.Models
{
    public class GameState
    {
        public List<Character> Characters { get; }
        public ObservableValue<Character> CurrentCharacter { get; }
        public ObservableValue<bool> IsGameActive { get; }

        public GameState()
        {
            Characters = new List<Character>();
            CurrentCharacter = new ObservableValue<Character>(null);
            IsGameActive = new ObservableValue<bool>(false);
        }

        public void AddCharacter(Character character)
        {
            Characters.Add(character);
        }

        public void SelectCharacter(Character character)
        {
            CurrentCharacter.Value = character;
            IsGameActive.Value = true;
        }

        public void ResetToCharacterSelection()
        {
            CurrentCharacter.Value = null;
            IsGameActive.Value = false;
        }
    }
}