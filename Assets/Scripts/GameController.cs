using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
using System.Reflection;
=======
>>>>>>> a75c61de5febad647ebd46599a49b9b82e10b266
using System;

public enum GameState { FreeRoam, Battle, Dialog, PartyScreen, Bag, Cutscene, Menu }

public class GameController : MonoBehaviour
{

    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;
    [SerializeField] PartyScreen partyScreen;
    [SerializeField] InventoryUI inventoryUI;
    GameState state;

    public static GameController Instance { get; private set; }
    MenuController menuController;
<<<<<<< HEAD
    AndroidJavaObject currentActivity;
    public string iduser;

=======
>>>>>>> a75c61de5febad647ebd46599a49b9b82e10b266

    private void Awake()
    {
        Instance = this;
        menuController = GetComponent<MenuController>();
        ConditionsDB.Init();
        PokemonDB.Init();
        MoveDB.Init();
    }
    private void Start()
    {
#if UNITY_ANDROID

            AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent");
            bool hasExtra = intent.Call<bool>("hasExtra", "id");

            if (hasExtra)
            {
                //AndroidJavaObject extras = intent.Call<AndroidJavaObject>("getExtras");
                iduser = intent.Call<String>("getStringExtra", "id");
            }
#endif

        playerController.OnEncountered += StartBattle;
        battleSystem.OnBattleOver += EndBattle;
        playerController.OnEnteredTrainersView += (Collider2D trainerCollider) => 
        {
            var trainer = trainerCollider.GetComponentInParent<TrainerController>();
            if (trainer != null)
            {
                state = GameState.Cutscene;
                StartCoroutine(trainer.TriggerTrainerBattle(playerController));
            }
        };

        DialogManager.Instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
        };
         DialogManager.Instance.OnCloseDialog += () =>
        {
            if (state == GameState.Dialog)
            {
                state = GameState.FreeRoam;
            }
        };
        menuController.onBack += () =>
         {
             state = GameState.FreeRoam;
         };
        menuController.onMenuSelected += OnMenuSelected;
    }

    void StartBattle()
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        var playerParty = playerController.GetComponent<PokemonParty>();
        var wildPokemon = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetWildPokemon();
        
        var wildPokemonCopy = new Pokemon(wildPokemon.Base, wildPokemon.Level);
        
        battleSystem.StartBattle(playerParty, wildPokemonCopy);
    }

    public void StartTrainerBattle(TrainerController trainer)
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);
        this.trainer = trainer;
        var playerParty = playerController.GetComponent<PokemonParty>();
        var trainerParty = trainer.GetComponent<PokemonParty>();
        battleSystem.StartTrainerBattle(playerParty, trainerParty);
    }
    TrainerController trainer;
    void EndBattle(bool won)
    {
        if(trainer != null && won == true)
        {
            trainer.BattleLost();
            trainer = null;
        }
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
            if (Input.GetKeyDown(KeyCode.Return))
            {
                menuController.OpenMenu();
                state = GameState.Menu;
            }
<<<<<<< HEAD
            if (Input.GetKeyDown(KeyCode.Q))
            {
#if UNITY_ANDROID
            AndroidJavaClass UnityPlayer = new AndroidJavaClass("dsa.ejercicios_practica.pokemon_android.ProfileActivity");
            AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            currentActivity.Call("onGameFinish", "83");
#endif
                Application.Quit();
            }
=======
>>>>>>> a75c61de5febad647ebd46599a49b9b82e10b266
        }
        else if (state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
        else if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
        else if (state == GameState.PartyScreen)
        {
            Action onSelected = () =>
            {
                // TODO: Go to Summary Screen
            };

            Action onBack = () =>
            {
                partyScreen.gameObject.SetActive(false);
                state = GameState.FreeRoam;
            };

            partyScreen.HandleUpdate(onSelected, onBack);
        }
        else if(state == GameState.Menu)
        {
            menuController.HandleUpdate();
        }
        else if (state == GameState.Bag)
        {
            Action onBack = () =>
            {
                inventoryUI.gameObject.SetActive(false);
                state = GameState.FreeRoam;
            };
            inventoryUI.HandleUpdate(onBack);
        }
    }
    void OnMenuSelected(int selectedItem)
    {
        if(selectedItem == 0)
        {
            //Pokemon selected
            partyScreen.gameObject.SetActive(true);
            partyScreen.SetPartyData(playerController.GetComponent<PokemonParty>().Pokemons);
            state = GameState.PartyScreen;
        }
        else if (selectedItem == 1)
        {
            //Bag selected
            inventoryUI.gameObject.SetActive(true);
            state = GameState.Bag;
        }
        else if (selectedItem == 2)
        {
            //Save selected
            SavingSystem.i.Save("saveSlot1");
        }
        else if (selectedItem == 3)
        {
            //Load selected 
            SavingSystem.i.Load("saveSlot1");
        }
        state = GameState.FreeRoam;
    }
}
