using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Chest;

public class HUD : MonoBehaviour
{
    [SerializeField] private Text scoreLabel;
    [SerializeField] public Slider healthBar;
    [SerializeField] public GameObject inventoryWindow;
    [SerializeField] public Transform inventoryContainer;
    [SerializeField] private InventoryUIButton inventoryItemPrefab;
    [SerializeField] public Text damageValue;
    [SerializeField] public Text healthValue;
    [SerializeField] public Text speedValue;
    [SerializeField] private GameObject LevelLoseWindow;
    [SerializeField] private GameObject LevelWonWindow;
    [SerializeField] private GameObject InGameMenu;
    [SerializeField] public GameObject optionsWindow;
    public void ShowLevelLoseWindow()
    {
        ShowWindow(LevelLoseWindow);
    }


    public void UpdateCharacterValues(float newHealth, float newSpeed, float newDamage)
    {
        healthValue.text = newHealth.ToString();
        speedValue.text = newSpeed.ToString();
        damageValue.text = newDamage.ToString();
    }


    static private HUD _instance;

    private void Start()
    {
        LoadInventory();

        GameController.Instance.OnUpdateHeroParameters += HandleOnUpdateHeroParameters;
        GameController.Instance.StartNewLevel();
    }
    private void OnDestroy()
    {
        GameController.Instance.OnUpdateHeroParameters -= HandleOnUpdateHeroParameters;
        
    }


    public static HUD Instance 
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    

    public void SetScore(string scoreValue)
    {
        scoreLabel.text = scoreValue;
    }

    public void ShowWindow(GameObject window)
    {
        window.GetComponent<Animator>().SetBool("Open", true);
        GameController.Instance.State = GameState.Pause;
    }
    public void HideWindow(GameObject window)
    {
        window.GetComponent<Animator>().SetBool("Open", false);
        GameController.Instance.State = GameState.Play;
    }
   

     public InventoryUIButton AddNewInventoryItem(InventoryItem itemData)
     {
         InventoryUIButton newItem = Instantiate(inventoryItemPrefab) as InventoryUIButton;
         newItem.transform.SetParent(inventoryContainer);
         newItem.ItemData = itemData;
         return newItem;
     }


    public void ButtonNext()
    {
        GameController.Instance.LoadNextLevel();

    }

    public void ButtonRestart()
    {
        GameController.Instance.RestartLevel();
        GameController.Instance.State = GameState.Play;
     
    }

    public void ButtonMainMenu()
    {
        GameController.Instance.LoadMainMenu();
      
    }

    public void ShowLevelWonWindow()
    {
        ShowWindow(LevelWonWindow);
    }
    public void LoadInventory()
    {
        InventoryUsedCallback callback = new InventoryUsedCallback(GameController.Instance.InventoryItemUsed);

        for (int i = 0; i < GameController.Instance.inventory.Count; i++)
        {
            InventoryUIButton newItem = AddNewInventoryItem(GameController.Instance.inventory[i]);


            newItem.callback = callback;
        }

    }
    private void HandleOnUpdateHeroParameters(HeroParameters parameters)
    {
        healthBar.maxValue = parameters.maxHealth;

        healthBar.value = parameters.maxHealth;

        UpdateCharacterValues(parameters.maxHealth, parameters.speed, parameters.damage);
    }
    public void SetSoundVolume(Slider slider)
    {
        GameController.Instance.audioManager.SfxVolume = slider.value;
    }
    public void SetMusicVolume(Slider slider)
    {
        GameController.Instance.audioManager.MusicVolume = slider.value;
    }


}





