using Game399.Shared.Runtime;
using Game399.Shared.Runtime.Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game399.Unity
{
    public class DialogUI : ObserverMonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject dialogPanel;
        [SerializeField] private TextMeshProUGUI characterNameText;
        [SerializeField] private TextMeshProUGUI dialogText;
        [SerializeField] private Button option1Button;
        [SerializeField] private Button option2Button;
        [SerializeField] private Button option3Button;
        [SerializeField] private TextMeshProUGUI option1Text;
        [SerializeField] private TextMeshProUGUI option2Text;
        [SerializeField] private TextMeshProUGUI option3Text;
        [SerializeField] private TextMeshProUGUI affectionText;
        [SerializeField] private Slider affectionSlider;
        [SerializeField] private Button backButton;

        private GameState _gameState;
        private IDialogService _dialogService;
        private IGameLog _gameLog;
        private Character _currentCharacter;

        protected override void Start()
        {
            base.Start();
            
            _gameState = ServiceResolver.Resolve<GameState>();
            _dialogService = ServiceResolver.Resolve<IDialogService>();
            _gameLog = ServiceResolver.Resolve<IGameLog>();

            option1Button.onClick.AddListener(() => OnOptionSelected(0));
            option2Button.onClick.AddListener(() => OnOptionSelected(1));
            option3Button.onClick.AddListener(() => OnOptionSelected(2));
            backButton.onClick.AddListener(OnBackToSelection);

            dialogPanel.SetActive(false);
        }

        protected override void Subscribe()
        {
            if (_gameState != null)
            {
                _gameState.CurrentCharacter.ChangeEvent += OnCurrentCharacterChanged;
            }
        }

        protected override void Unsubscribe()
        {
            if (_gameState != null)
            {
                _gameState.CurrentCharacter.ChangeEvent -= OnCurrentCharacterChanged;
            }

            if (_currentCharacter != null)
            {
                _currentCharacter.Affection.ChangeEvent -= OnAffectionChanged;
                _currentCharacter.CurrentDialogIndex.ChangeEvent -= OnDialogIndexChanged;
            }
        }

        private void OnCurrentCharacterChanged(Character character)
        {
            // Unsubscribe from previous character
            if (_currentCharacter != null)
            {
                _currentCharacter.Affection.ChangeEvent -= OnAffectionChanged;
                _currentCharacter.CurrentDialogIndex.ChangeEvent -= OnDialogIndexChanged;
            }

            _currentCharacter = character;

            if (character != null)
            {
                // Subscribe to new character's events
                character.Affection.ChangeEvent += OnAffectionChanged;
                character.CurrentDialogIndex.ChangeEvent += OnDialogIndexChanged;

                characterNameText.text = character.Name;
                dialogPanel.SetActive(true);
                UpdateAffectionDisplay(character.Affection.Value);
                DisplayCurrentDialog();
            }
            else
            {
                dialogPanel.SetActive(false);
            }
        }

        private void OnAffectionChanged(int newAffection)
        {
            UpdateAffectionDisplay(newAffection);
        }

        private void OnDialogIndexChanged(int newIndex)
        {
            DisplayCurrentDialog();
        }

        private void DisplayCurrentDialog()
        {
            if (_currentCharacter == null) return;

            var dialog = _dialogService.GetCurrentDialog(_currentCharacter);

            if (dialog == null)
            {
                ShowCompletionScreen();
                return;
            }

            dialogText.text = dialog.CharacterLine;

            option1Text.text = dialog.Options[0].Text;
            option2Text.text = dialog.Options[1].Text;
            option3Text.text = dialog.Options[2].Text;
        }

        private void OnOptionSelected(int optionIndex)
        {
            if (_currentCharacter == null) return;

            // Interact with the Model via Service, not the View
            _dialogService.SelectOption(_currentCharacter, optionIndex);
        }

        private void UpdateAffectionDisplay(int affection)
        {
            affectionText.text = $"Affection: {affection}";
    
            // Map affection to slider (range 0 to 100)
            float normalizedAffection = Mathf.InverseLerp(0, 100, affection);
            affectionSlider.value = normalizedAffection;

            // Color code
            var sliderFill = affectionSlider.fillRect.GetComponent<Image>();
            if (sliderFill != null)
            {
                if (affection >= 80)
                    sliderFill.color = new Color(1f, 0.4f, 0.7f); // Pink - Love
                else if (affection >= 60)
                    sliderFill.color = new Color(1f, 0.6f, 0.6f); // Light red - Very positive
                else if (affection >= 40)
                    sliderFill.color = new Color(0.5f, 0.8f, 1f); // Light blue - Positive
                else if (affection >= 20)
                    sliderFill.color = new Color(0.7f, 0.7f, 0.7f); // Gray - Neutral
                else
                    sliderFill.color = new Color(0.6f, 0.6f, 0.8f); // Purple - Negative
            }
        }

        private void ShowCompletionScreen()
        {
            if (_currentCharacter == null) return;

            int finalAffection = _currentCharacter.Affection.Value;
            string endingMessage;

            if (finalAffection >= 70)
            {
                endingMessage = $"{_currentCharacter.Name} has fallen in love with you! ❤️\n\n" +
                                "They want to be with you always. This is the best ending!";
            }
            else if (finalAffection >= 50)
            {
                endingMessage = $"{_currentCharacter.Name} really likes you!\n\n" +
                                "They're interested in seeing where this goes. Good job!";
            }
            else if (finalAffection >= 30)
            {
                endingMessage = $"{_currentCharacter.Name} thinks you're okay.\n\n" +
                                "You're friends, but nothing more developed.";
            }
            else
            {
                endingMessage = $"{_currentCharacter.Name} isn't interested in you.\n\n" +
                                "Your choices pushed them away. Maybe try again?";
            }

            dialogText.text = endingMessage;
    
            option1Button.gameObject.SetActive(false);
            option2Button.gameObject.SetActive(false);
            option3Button.gameObject.SetActive(false);

            _gameLog.Info($"Completed {_currentCharacter.Name}'s route with {finalAffection} affection");
        }

        private void OnBackToSelection()
        {
            option1Button.gameObject.SetActive(true);
            option2Button.gameObject.SetActive(true);
            option3Button.gameObject.SetActive(true);

            // Reset current character's progress
            if (_currentCharacter != null)
            {
                _currentCharacter.Affection.Value = 50;
                _currentCharacter.CurrentDialogIndex.Value = 0;
            }

            _gameState.ResetToCharacterSelection();
        }
    }
}