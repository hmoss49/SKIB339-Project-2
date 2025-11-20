using System;
using Game399.Shared.Runtime;
using Game399.Shared.Runtime.Models;
using System.Collections.Generic;
using UnityEngine;

namespace Game399.Unity
{
    [DefaultExecutionOrder(-100)]
    public class Game : MonoBehaviour
    {
        private static Game _instance;
        public static Game Instance => _instance;

        [Header("Audio")]
        [SerializeField] private AudioSource backgroundMusicSource;
        [SerializeField] private AudioClip menuMusic;
        [SerializeField] private AudioClip vodkaMusic;
        [SerializeField] private AudioClip strongZeroMusic;
        [SerializeField] private AudioClip wineMusic;
        [SerializeField] private AudioClip whiskeyMusic;
        [SerializeField] private AudioClip sojuMusic;

        private GameState _gameState;
        private IDialogService _dialogService;
        private IGameLog _gameLog;

        public GameState GameState => _gameState;
        public IDialogService DialogService => _dialogService;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeServices();
            CreateCharacters();
            PlayMenuMusic();
        }

        private void InitializeServices()
        {
            _gameLog = new UnityGameLogger();
            _dialogService = new DialogService(_gameLog);
            _gameState = new GameState();

            ServiceResolver.Register(_gameLog);
            ServiceResolver.Register(_dialogService);
            ServiceResolver.Register(_gameState);

            _gameLog.Info("Game services initialized");
        }
        
        private void CreateCharacters()
        {
            CreateVodkaCharacter();
            CreateStrongZeroCharacter();
            CreateWineCharacter();

            _gameLog.Info($"Created {_gameState.Characters.Count} characters");
        }

        private void CreateVodkaCharacter()
        {
            var vodka = new Character("Vodka", "A bold and intense spirit");
            
            var dialogs = new List<DialogNode>
            {
                new DialogNode(
                    "Hello!",
                    new DialogOption("Good option", 15,-15),
                    new DialogOption("OK option", 0,15),
                    new DialogOption("Bad option", -15,15)
                ),
                new DialogNode(
                    "Line 2",
                    new DialogOption("Good option", 15,-15),
                    new DialogOption("OK option", 0,15),
                    new DialogOption("Bad option", -15,15)
                ),
                new DialogNode(
                    "Line 3",
                    new DialogOption("Good option", 15,-15),
                    new DialogOption("OK option", 0,15),
                    new DialogOption("Bad option", -15,15)
                )
            };

            _gameState.AddCharacter(vodka);
            _dialogService.RegisterCharacterDialogs(vodka, dialogs);
        }

        private void CreateStrongZeroCharacter()
        {
            var strongZero = new Character("Strong Zero", "Fun and energetic with a kick");
            
            var dialogs = new List<DialogNode>
            {
                new DialogNode(
                    "Hello!",
                    new DialogOption("Good option", 15,-15),
                    new DialogOption("OK option", 0,15),
                    new DialogOption("Bad option", -15,15)
                ),
                new DialogNode(
                    "Line 2",
                    new DialogOption("Good option", 15,-15),
                    new DialogOption("OK option", 0,15),
                    new DialogOption("Bad option", -15,15)
                ),
                new DialogNode(
                    "Line 3",
                    new DialogOption("Good option", 15,-15),
                    new DialogOption("OK option", 0,15),
                    new DialogOption("Bad option", -15,15)
                )
            };

            _gameState.AddCharacter(strongZero);
            _dialogService.RegisterCharacterDialogs(strongZero, dialogs);
        }

        private void CreateWineCharacter()
        {
            var wine = new Character("Wine", "Sophisticated and elegant");
            
            var dialogs = new List<DialogNode>
            {
                new DialogNode(
                    "Hello!",
                    new DialogOption("Good option", 15,-15),
                    new DialogOption("OK option", 0,15),
                    new DialogOption("Bad option", -15,15)
                ),
                new DialogNode(
                    "Line 2",
                    new DialogOption("Good option", 15,-15),
                    new DialogOption("OK option", 0,15),
                    new DialogOption("Bad option", -15,15)
                ),
                new DialogNode(
                    "Line 3",
                    new DialogOption("Good option", 15,-15),
                    new DialogOption("OK option", 0,15),
                    new DialogOption("Bad option", -15,15)
                )
            };

            _gameState.AddCharacter(wine);
            _dialogService.RegisterCharacterDialogs(wine, dialogs);
        }

        public void PlayCharacterMusic(string characterName)
        {
            AudioClip clipToPlay = null;

            switch (characterName)
            {
                case "Vodka":
                    clipToPlay = vodkaMusic;
                    break;
                case "Strong Zero":
                    clipToPlay = strongZeroMusic;
                    break;
                case "Wine":
                    clipToPlay = wineMusic;
                    break;
                case "Whiskey":
                    clipToPlay = whiskeyMusic;
                    break;
                case "Soju":
                    clipToPlay = sojuMusic;
                    break;
            }

            if (clipToPlay != null)
            {
                PlayMusic(clipToPlay);
                _gameLog.Info($"Playing music for {characterName}");
            }
        }

        public void PlayMenuMusic()
        {
            if (menuMusic != null)
            {
                PlayMusic(menuMusic);
                _gameLog.Info("Playing menu music");
            }
        }

        private void PlayMusic(AudioClip clip)
        {
            if (backgroundMusicSource != null && clip != null)
            {
                if (backgroundMusicSource.clip != clip)
                {
                    backgroundMusicSource.clip = clip;
                    backgroundMusicSource.loop = true;
                    backgroundMusicSource.Play();
                }
            }
        }
        
    }
}