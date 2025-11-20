using Game399.Shared.Runtime;
using Game399.Shared.Runtime.Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game399.Unity
{
    public class CharacterSelectionUI : ObserverMonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject selectionPanel;
        [SerializeField] private Transform characterButtonContainer;
        [SerializeField] private GameObject characterButtonPrefab;
        [SerializeField] private Sprite[] characterSelectSprites;
        [SerializeField] private Sprite defaultCharacterSelectSprite;

        private GameState _gameState;
        private IGameLog _gameLog;

        protected override void Start()
        {
            // Resolve services BEFORE calling base.Start() which triggers Subscribe()
            _gameState = ServiceResolver.Resolve<GameState>();
            _gameLog = ServiceResolver.Resolve<IGameLog>();
            
            base.Start();
            
            CreateCharacterButtons();
            ShowSelectionScreen();
        }

        protected override void Subscribe()
        {
            if (_gameState != null)
            {
                Debug.Log($"GameState is not null. IsGameActive value: {_gameState.IsGameActive.Value}");
                _gameState.IsGameActive.ChangeEvent += OnGameActiveChanged;
                Debug.Log("Subscribed to IsGameActive.ChangeEvent");
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
                _gameState.IsGameActive.ChangeEvent -= OnGameActiveChanged;
            }
        }

        private void OnGameActiveChanged(bool isActive)
        {
            // Hide selection screen when a character is selected
            selectionPanel.SetActive(!isActive);
        }

        private void CreateCharacterButtons()
        {
            int index = 0;
            
            foreach (var character in _gameState.Characters)
            {
                var buttonObj = Instantiate(characterButtonPrefab, characterButtonContainer);
                
                var buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = $"{character.Name}\n<size=60%>{character.Description}</size>";
                }
                var buttonImage = buttonObj.GetComponent<Image>();
                if (buttonImage != null)
                {
                    if (characterSelectSprites.Length == _gameState.Characters.Count)
                    {
                        buttonImage.sprite = characterSelectSprites[index];
                        index++;
                    }
                    else
                    {
                        buttonImage.sprite = defaultCharacterSelectSprite;
                    }
                }
                var button = buttonObj.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.AddListener(() => SelectCharacter(character));
                }
            }

            _gameLog.Info($"Created {_gameState.Characters.Count} character selection buttons");
        }

        private void SelectCharacter(Character character)
        {
            _gameLog.Info($"Player selected character: {character.Name}");
            _gameState.SelectCharacter(character);
        }

        public void ShowSelectionScreen()
        {
            _gameState.ResetToCharacterSelection();
        }
    }
}