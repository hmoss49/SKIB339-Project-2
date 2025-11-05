using System.Collections.Generic;

namespace Game399.Shared.Runtime.Models
{
    public class DialogNode
    {
        public string CharacterLine { get; }
        public List<DialogOption> Options { get; }

        public DialogNode(string characterLine, DialogOption option1, DialogOption option2, DialogOption option3)
        {
            CharacterLine = characterLine;
            Options = new List<DialogOption> { option1, option2, option3 };
        }
    }
}