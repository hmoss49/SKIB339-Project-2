using Game399.Shared.Runtime;
using Game399.Shared.Runtime.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Game399.Unity
{
    public class CharacterPortraitView : ObserverMonoBehaviour
    {
        [Header("Character Portraits")]
        [SerializeField] private Sprite vodkaSprite;
        [SerializeField] private Sprite strongZeroSprite;
        [SerializeField] private Sprite wineSprite;
        
        [Header("UI References")]
        [SerializeField] private Image portraitImage;
        [SerializeField] private GameObject portraitContainer;

        private GameState _gameState;

        protected override void Start()
        {
            // Resolve services BEFORE calling base.Start() which triggers Subscribe()
            _gameState = ServiceResolver.Resolve<GameState>();
            
            base.Start();
            
            portraitContainer.SetActive(false);
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
        }

        private void OnCurrentCharacterChanged(Character character)
        {
            if (character == null)
            {
                portraitContainer.SetActive(false);
                return;
            }

            portraitContainer.SetActive(true);
            
            switch (character.Name)
            {
                case "Vodka":
                    portraitImage.sprite = vodkaSprite;
                    break;
                case "Strong Zero":
                    portraitImage.sprite = strongZeroSprite;
                    break;
                case "Wine":
                    portraitImage.sprite = wineSprite;
                    break;
            }
        }
    }
}