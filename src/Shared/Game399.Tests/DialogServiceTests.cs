using Game399.Shared.Runtime;
using Game399.Shared.Runtime.Models;
using System.Collections.Generic;

namespace Game399.Tests
{
    public class DialogServiceTests
    {
        private class TestGameLog : IGameLog
        {
            public List<string> InfoMessages { get; } = new List<string>();
            public List<string> WarningMessages { get; } = new List<string>();
            public List<string> ErrorMessages { get; } = new List<string>();

            public void Info(string message) => InfoMessages.Add(message);
            public void Warning(string message) => WarningMessages.Add(message);
            public void Error(string message) => ErrorMessages.Add(message);
        }

        [Test]
        public void RegisterCharacterDialogs_StoresDialogsCorrectly()
        {
            var log = new TestGameLog();
            var service = new DialogService(log);
            var character = new Character("Vodka", "A bold and intense spirit");
            var dialogs = CreateVodkaDialogs();

            service.RegisterCharacterDialogs(character, dialogs);

            var currentDialog = service.GetCurrentDialog(character);
            Assert.That(currentDialog, Is.Not.Null);
            Assert.That(currentDialog.CharacterLine, Is.EqualTo("Hey! Want to party tonight?"));
            Assert.That(log.InfoMessages.Count, Is.EqualTo(1));
            Assert.That(log.InfoMessages[0], Does.Contain("Registered 3 dialog nodes"));
        }

        [Test]
        public void GetCurrentDialog_ReturnsNullForUnregisteredCharacter()
        {
            var log = new TestGameLog();
            var service = new DialogService(log);
            var character = new Character("Vodka", "A bold and intense spirit");

            var dialog = service.GetCurrentDialog(character);

            Assert.That(dialog, Is.Null);
            Assert.That(log.WarningMessages.Count, Is.EqualTo(1));
            Assert.That(log.WarningMessages[0], Does.Contain("No dialogs registered"));
        }

        [Test]
        public void SelectOption_IncreasesAffectionAndAdvancesDialog()
        {
            var log = new TestGameLog();
            var service = new DialogService(log);
            var character = new Character("Vodka", "A bold and intense spirit");
            var dialogs = CreateVodkaDialogs();
            service.RegisterCharacterDialogs(character, dialogs);

            service.SelectOption(character, 0); // +5 affection, -15 sobriety

            Assert.That(character.Affection.Value, Is.EqualTo(55));
            Assert.That(character.Sobriety.Value, Is.EqualTo(35));
            Assert.That(character.CurrentDialogIndex.Value, Is.EqualTo(1));

            // Validate log lines:
            // [0] -> "Registered X dialog nodes"
            // [1] -> affection log
            // [2] -> sobriety log
            Assert.That(log.InfoMessages[1], Does.Contain("Affection change: +5"));
            Assert.That(log.InfoMessages[2], Does.Contain("Sobriety change: -15"));
        }


        [Test]
        public void SelectOption_DecreaseAffectionForBadChoice()
        {
            var log = new TestGameLog();
            var service = new DialogService(log);
            var character = new Character("Vodka", "A bold and intense spirit");
            var dialogs = CreateVodkaDialogs();
            service.RegisterCharacterDialogs(character, dialogs);

            service.SelectOption(character, 2); // -3 affection, +15 sobriety

            Assert.That(character.Affection.Value, Is.EqualTo(47));
            Assert.That(character.Sobriety.Value, Is.EqualTo(65));

            Assert.That(log.InfoMessages[1], Does.Contain("Affection change: -3"));
            Assert.That(log.InfoMessages[2], Does.Contain("Sobriety change: +15"));
        }


        [Test]
        public void SelectOption_WithInvalidIndex_LogsError()
        {
            var log = new TestGameLog();
            var service = new DialogService(log);
            var character = new Character("Vodka", "A bold and intense spirit");
            var dialogs = CreateVodkaDialogs();
            service.RegisterCharacterDialogs(character, dialogs);

            service.SelectOption(character, 99);

            Assert.That(character.Affection.Value, Is.EqualTo(50));
            Assert.That(character.Sobriety.Value, Is.EqualTo(50));
            Assert.That(character.CurrentDialogIndex.Value, Is.EqualTo(0));
            Assert.That(log.ErrorMessages.Count, Is.EqualTo(1));
            Assert.That(log.ErrorMessages[0], Does.Contain("Invalid option index"));
        }

        [Test]
        public void IsDialogComplete_ReturnsTrueWhenAllDialogsFinished()
        {
            var log = new TestGameLog();
            var service = new DialogService(log);
            var character = new Character("Vodka", "A bold and intense spirit");
            var dialogs = CreateVodkaDialogs();
            service.RegisterCharacterDialogs(character, dialogs);

            // Make 3 choices to exhaust dialogs
            service.SelectOption(character, 0);
            service.SelectOption(character, 1);
            service.SelectOption(character, 2);

            Assert.That(service.IsDialogComplete(character), Is.True);
            Assert.That(service.GetCurrentDialog(character), Is.Null);
        }

        [Test]
        public void IsDialogComplete_ReturnsFalseWhenDialogsRemain()
        {
            var log = new TestGameLog();
            var service = new DialogService(log);
            var character = new Character("Vodka", "A bold and intense spirit");
            var dialogs = CreateVodkaDialogs();
            service.RegisterCharacterDialogs(character, dialogs);

            service.SelectOption(character, 0);

            Assert.That(service.IsDialogComplete(character), Is.False);
            Assert.That(service.GetCurrentDialog(character), Is.Not.Null);
        }

        [Test]
        public void GetTotalDialogCount_ReturnsCorrectCount()
        {
            var log = new TestGameLog();
            var service = new DialogService(log);
            var character = new Character("Vodka", "A bold and intense spirit");
            var dialogs = CreateVodkaDialogs();
            service.RegisterCharacterDialogs(character, dialogs);

            int count = service.GetTotalDialogCount(character);

            Assert.That(count, Is.EqualTo(3));
        }

        [Test]
        public void AffectionAndSobrietyAccumulate_AcrossMultipleChoices()
        {
            var log = new TestGameLog();
            var service = new DialogService(log);
            var character = new Character("Vodka", "A bold and intense spirit");
            var dialogs = CreateVodkaDialogs();
            service.RegisterCharacterDialogs(character, dialogs);

            // Choices: +5/-15, 0/+15, +5/-15
            service.SelectOption(character, 0);
            service.SelectOption(character, 1);
            service.SelectOption(character, 0);

            Assert.That(character.Affection.Value, Is.EqualTo(60));
            Assert.That(character.Sobriety.Value, Is.EqualTo(35)); // 50 -15 +15 -15
            Assert.That(service.IsDialogComplete(character), Is.True);
        }

        private List<DialogNode> CreateVodkaDialogs()
        {
            return new List<DialogNode>
            {
                new DialogNode(
                    "Hey! Want to party tonight?",
                    new DialogOption("Absolutely! Let's go wild!", 5, -15),
                    new DialogOption("Maybe... depends on the vibe.", 0, 15),
                    new DialogOption("I don't really party.", -3, 15)
                ),
                new DialogNode(
                    "I love living life on the edge. You?",
                    new DialogOption("Same! Life's too short to play it safe!", 4, -15),
                    new DialogOption("I prefer a balanced approach.", 0, 15),
                    new DialogOption("I like taking risks with you!", 3, -15)
                ),
                new DialogNode(
                    "Want to take this relationship to the next level?",
                    new DialogOption("Yes! I'm all in with you!", 5, -15),
                    new DialogOption("Let me think about it...", 1, 15),
                    new DialogOption("I'm not sure we're compatible.", -1, 15)
                )
            };
        }
    }
}
