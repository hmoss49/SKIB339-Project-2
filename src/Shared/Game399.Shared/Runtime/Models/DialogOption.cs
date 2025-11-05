namespace Game399.Shared.Runtime.Models
{
    public class DialogOption
    {
        public string Text { get; }
        public int AffectionChange { get; }

        public DialogOption(string text, int affectionChange)
        {
            Text = text;
            AffectionChange = affectionChange;
        }
    }
}