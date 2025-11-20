namespace Game399.Shared.Runtime.Models
{
    public class DialogOption
    {
        public string Text { get; }
        public int AffectionChange { get; }
        public int SobrietyChange { get; }

        public DialogOption(string text, int affectionChange, int sobrietyChange)
        {
            Text = text;
            AffectionChange = affectionChange;
            SobrietyChange = sobrietyChange;
        }
    }
}