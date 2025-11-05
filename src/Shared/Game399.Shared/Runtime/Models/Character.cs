namespace Game399.Shared.Runtime.Models
{
    public class Character
    {
        public string Name { get; }
        public string Description { get; }
        public ObservableValue<int> Affection { get; }
        public ObservableValue<int> CurrentDialogIndex { get; }

        public Character(string name, string description)
        {
            Name = name;
            Description = description;
            Affection = new ObservableValue<int>(0);
            CurrentDialogIndex = new ObservableValue<int>(0);
        }
    }
}