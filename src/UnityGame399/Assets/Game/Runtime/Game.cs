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
            CreateWhiskeyCharacter();
            CreateSojuCharacter();

            _gameLog.Info($"Created {_gameState.Characters.Count} characters");
        }

        private void CreateVodkaCharacter()
        {
            var vodka = new Character("Vodka", "A bold and intense spirit");
          
            var dialogs = new List<DialogNode>
            {
                new DialogNode(
                    "You’re standing a little close. Most people give me space. Did you not notice, or do you not care?",
                    new DialogOption("If it bothered you, you'd have said so.", 15,-15),
                    new DialogOption("I can take a step back if you want.", -5, 5),
                    new DialogOption("Maybe you should back up instead", -15, 15)
                ),
                new DialogNode(
                    "I noticed someone getting too close to you earlier. You didn't react, so I stepped in. Don't read into it, I just don't like careless people.",
                    new DialogOption("I didn't realize anything happened.", -5, 5),
                    new DialogOption("You really think I need help?", -15, 15),
                    new DialogOption("Either way, thanks.", 15, -15)
                ),
                new DialogNode(
                    "I'm not great with people. But I pay attention to the ones who matter. And lately... you've been hard to ignore.",
                    new DialogOption("Don't make this complicated.", -15, 15),
                    new DialogOption("I've noticed you've been paying attention.", 15, -15),
                    new DialogOption("Didn't expect to hear that from you.", -5, 5)
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
                    "You looked at me longer than you looked at anyone else. That means something… doesn’t it?",
                    new DialogOption("I wasn't keeping track.", -5, 5),
                    new DialogOption("Maybe I was drawn to you.", 15, -15),
                    new DialogOption("Don't read into it.", -15, 15)
                ),
                new DialogNode(
                    "When you talk to other people, it makes my chest hurt. Is that normal?.. No, don't answer. I already know.",
                    new DialogOption("That's not healthy.", -15, 15),
                    new DialogOption("You're overreacting.", -5, 5),
                    new DialogOption("If it bothers you, I'll stay close", 15, -15)
                ),
                new DialogNode(
                    "You won't disappear without telling me, right? I'd get confused and upset.",
                    new DialogOption("Don't worry. I'm not going anywhere.", 15, -15),
                    new DialogOption("You're way too clingy.", -15, 15),
                    new DialogOption("I'll try to remember.", -5, 5)
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
                    "Well… you caught my eye quicker than I expected. Not many people manage that.",
                    new DialogOption("Oh thanks, I think?", -5, 5),
                    new DialogOption("Guess we noticed each other at the same time.", 15, -15),
                    new DialogOption("You must say that to everyone", -15, 15)
                ),
                new DialogNode(
                    "You choose your words carefully. I like that. It makes me wonder what you’re holding back.",
                    new DialogOption("Maybe I’m saving my best lines for you.", 15, -15),
                    new DialogOption("Stop trying to read me.", -15, 15),
                    new DialogOption("I'm just trying to be polite.", -5, 5)
                ),
                new DialogNode(
                    "Your reactions are interesting. One smile from you changes the whole atmosphere... and I'm not exaggerating.",
                    new DialogOption("That sounds dramatic.", -15, 15),
                    new DialogOption("If my smile means that much to you, I'll show it more.", 15, -15),
                    new DialogOption("Uh... that's nice of you to say.", -5, 5)
                )
            };


            _gameState.AddCharacter(wine);
            _dialogService.RegisterCharacterDialogs(wine, dialogs);
        }
        private void CreateWhiskeyCharacter()
        {
            var whiskey = new Character("Whiskey", "Peppy and Southern");
          
            var dialogs = new List<DialogNode>
            {
                new DialogNode(
                    "Well look at you. Most folks walk in lookin’ tired, but you’ve got a spark in your eyes. Makes me curious, y'know?",
                    new DialogOption("I'm just trying to be friendly.", -5, 5),
                    new DialogOption("Maybe we're both curious about each other.", 15, -15),
                    new DialogOption("You're imagining things.", -15, 15)
                ),
                new DialogNode(
                    "You know, people think I’m all sunshine ‘cause I smile a lot. But sometimes I smile ‘cause it’s easier than explainin’ things. Funny I'm tellin' you this.",
                    new DialogOption("You can be real with me. I don't mind.", 15, -15),
                    new DialogOption("You don't need to overshare with me.", -15, 15),
                    new DialogOption("Everyone hides things sometimes.", -5, 5)
                ),
                new DialogNode(
                    "Feels nice bein’ around someone who don’t treat me like a joke.  I get loud, sure, but you actually listen. That means more than you know.",
                    new DialogOption("Don't get used to it.", -15, 15),
                    new DialogOption("I try to pay attention.", -5, 5),
                    new DialogOption("I listen because I care about you.", 15, -15)
                )
            };


            _gameState.AddCharacter(whiskey);
            _dialogService.RegisterCharacterDialogs(whiskey, dialogs);
        }

        private void CreateSojuCharacter()
        {
            var soju = new Character("Soju", "A charming playboy");
          
            var dialogs = new List<DialogNode>
            {
                new DialogNode(
                    "You walked over pretty confidently. Most people hesitate with me... What made you decide to bother?",
                    new DialogOption("I just felt like saying hi.", -5, 5),
                    new DialogOption("You looked interesting enough to talk to.", 15, -15),
                    new DialogOption("Trust me, this wasn't a big decision.", -15, 15)
                ),
                new DialogNode(
                    "You don’t react the way I expect. Usually people either try too hard or get flustered. You don’t do either. And it’s… throwing me off.",
                    new DialogOption("I like kpop.", -15, 15),
                    new DialogOption("I'm just talking to you like a normal person.", 15, -15),
                    new DialogOption("Is that a bad thing?", 5, -5)
                ),
                new DialogNode(
                    "I’m trying to figure out why I keep thinking about our conversations. It’s annoying, honestly. But it’s happening, so… whatever it is, it’s there.",
                    new DialogOption("You’re not the only one thinking about it.", 15, -15),
                    new DialogOption("You're over analyzing.", -15, 15),
                    new DialogOption("I'll take that as a compliment", -5, 5)
                )
            };


            _gameState.AddCharacter(soju);
            _dialogService.RegisterCharacterDialogs(soju, dialogs);
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