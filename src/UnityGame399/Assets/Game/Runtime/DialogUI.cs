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
        [SerializeField] private Image affectionSlider;
        [SerializeField] private TextMeshProUGUI sobrietyText;
        [SerializeField] private Image sobrietySlider;
        [SerializeField] private Button backButton;

        private GameState _gameState;
        private IDialogService _dialogService;
        private IGameLog _gameLog;
        private Character _currentCharacter;

        protected override void Start()
        {
            // Resolve services BEFORE calling base.Start() which triggers Subscribe()
            _gameState = ServiceResolver.Resolve<GameState>();
            _dialogService = ServiceResolver.Resolve<IDialogService>();
            _gameLog = ServiceResolver.Resolve<IGameLog>();
            
            base.Start();

            option1Button.onClick.AddListener(() => OnOptionSelected(0));
            option2Button.onClick.AddListener(() => OnOptionSelected(1));
            option3Button.onClick.AddListener(() => OnOptionSelected(2));
            backButton.onClick.AddListener(OnBackToSelection);
        }

        protected override void Subscribe()
        {
            if (_gameState != null)
            {
                Debug.Log($"GameState is not null. CurrentCharacter value: {_gameState.CurrentCharacter.Value?.Name ?? "null"}");
                _gameState.CurrentCharacter.ChangeEvent += OnCurrentCharacterChanged;
                Debug.Log("Subscribed to CurrentCharacter.ChangeEvent");
            }
            else
            {
                Debug.Log("ERROR: GameState is NULL!");
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
                _currentCharacter.Sobriety.ChangeEvent -= OnSobrietyChanged;
                _currentCharacter.CurrentDialogIndex.ChangeEvent -= OnDialogIndexChanged;
            }
        }

        private void OnCurrentCharacterChanged(Character character)
        {
            Debug.Log($"OnCurrentCharacterChanged called! Character: {character?.Name}");
            
            // Unsubscribe from previous character
            if (_currentCharacter != null)
            {
                _currentCharacter.Affection.ChangeEvent -= OnAffectionChanged;
                _currentCharacter.Sobriety.ChangeEvent -= OnSobrietyChanged;
                _currentCharacter.CurrentDialogIndex.ChangeEvent -= OnDialogIndexChanged;
            }

            _currentCharacter = character;

            if (character != null)
            {
                // Subscribe to new character's events
                character.Affection.ChangeEvent += OnAffectionChanged;
                character.Sobriety.ChangeEvent += OnSobrietyChanged;
                character.CurrentDialogIndex.ChangeEvent += OnDialogIndexChanged;

                characterNameText.text = character.Name;
                dialogPanel.SetActive(true);
                UpdateAffectionDisplay(character.Affection.Value);
                UpdateSobrietyDisplay(character.Sobriety.Value);
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

        private void OnSobrietyChanged(int newSobriety)
        {
            UpdateSobrietyDisplay(newSobriety);
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

            _dialogService.SelectOption(_currentCharacter, optionIndex);
        }

        private void UpdateAffectionDisplay(int affection)
        {
            affectionText.text = $"Affection: {affection}";
            
            affectionSlider.fillAmount = affection / 100f;
            
            if (affectionSlider != null)
            {
                if (affection >= 80)
                    affectionSlider.color = new Color(1f, 0.4f, 0.7f); // Pink - Love
                else if (affection >= 65)
                    affectionSlider.color = new Color(1f, 0.6f, 0.6f); // Light red - Very positive
                else if (affection >= 50)
                    affectionSlider.color = new Color(0.7f, 0.7f, 0.7f); // Grey - Neutral
                else if (affection >= 30)
                    affectionSlider.color = Color.orange; // Orange - Dislike
                else
                    affectionSlider.color = Color.red; // Red - Very Negative
            }
        }
        
        private void UpdateSobrietyDisplay(int sobriety)
        {
            sobrietyText.text = $"Sobriety: {sobriety}";
            
            sobrietySlider.fillAmount = sobriety / 100f;
            
            if (sobrietySlider != null)
            {
                if (sobriety >= 65)
                    sobrietySlider.color = Color.skyBlue; // Blue - Super Sober
                else if (sobriety >= 50)
                    sobrietySlider.color = new Color(0.7f, 0.7f, 0.7f); // Grey - Sober
                else if (sobriety >= 30)
                    sobrietySlider.color = new Color(0.51f, 0.05f, 0.34f); // Tipsy - 
                else
                    sobrietySlider.color = new Color(0.32f, 0.03f,0.21f); // Red - Very Negative
            }
        }

        private void ShowCompletionScreen()
        {
            if (_currentCharacter == null) return;

            int finalAffection = _currentCharacter.Affection.Value;
            string endingMessage;

            if (finalAffection >= 80)
            {
                endingMessage = $"{_currentCharacter.Name} has fallen in love with you!";
            }
            else if (finalAffection >= 65)
            {
                endingMessage = $"{_currentCharacter.Name} really likes you!";
            }
            else if (finalAffection >= 50)
            {
                endingMessage = $"{_currentCharacter.Name} doesn't know how to feel about you.";
            }
            else if (finalAffection >= 30)
            {
                endingMessage = $"{_currentCharacter.Name} isn't interested in you.";
            }
            else
            {
                endingMessage = $"{_currentCharacter.Name} really doesn't like you.";
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