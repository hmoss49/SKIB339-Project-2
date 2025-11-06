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

        private GameState _gameState;
        private IGameLog _gameLog;

        protected override void Start()
        {
            base.Start();
            
            _gameState = ServiceResolver.Resolve<GameState>();
            _gameLog = ServiceResolver.Resolve<IGameLog>();
            
            CreateCharacterButtons();
            ShowSelectionScreen();
        }

        protected override void Subscribe()
        {
            if (_gameState != null)
            {
                _gameState.IsGameActive.ChangeEvent += OnGameActiveChanged;
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
            foreach (var character in _gameState.Characters)
            {
                var buttonObj = Instantiate(characterButtonPrefab, characterButtonContainer);
                
                var buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = $"{character.Name}\n<size=60%>{character.Description}</size>";
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