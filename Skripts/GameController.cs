using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Chest;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState { Play, Pause }

public delegate void InventoryUsedCallback(InventoryUIButton item);
public delegate void UpdateHeroParametersHandler(HeroParameters parameters);



public class GameController : MonoBehaviour
{
    public event UpdateHeroParametersHandler OnUpdateHeroParameters;

    static private GameController _instance;

    private GameState state;

    private int score;

    [SerializeField] public HeroParameters hero;

    [SerializeField] public List<InventoryItem> inventory;

    [SerializeField] private int dragonHitScore;

    [SerializeField] private int dragonKillScore;

    [SerializeField] public float maxHealth;

    [SerializeField] public Audio audioManager;

    [SerializeField] private int dragonKillExperience;

    

    
    private void InitializeAudioManager()
    {
        audioManager.SourceMusic = gameObject.AddComponent<AudioSource>();
        audioManager.SourceRandomPitchSFX = gameObject.AddComponent<AudioSource>();
        audioManager.SourceSFX = gameObject.AddComponent<AudioSource>();
        gameObject.AddComponent<AudioListener>();
    }


    public void AddNewInventoryItem(InventoryItem itemData)
    {
        InventoryUIButton newUiButton = HUD.Instance.AddNewInventoryItem(itemData);
        InventoryUsedCallback callback = new InventoryUsedCallback(InventoryItemUsed);

        newUiButton.callback = callback;

        inventory.Add(itemData);
    }

    public void InventoryItemUsed(InventoryUIButton item)
    {
        switch (item.ItemData.CrystallType)
        {
            case CrystallType.Blue:
               
                hero.speed += item.ItemData.Quantity / 10f;
                break;

            case CrystallType.Red:
               
                hero.damage += item.ItemData.Quantity / 10f;
                break;



            case CrystallType.Green:
                //убираем
                //MaxHealth += item.ItemData.Quantity / 10f;
                //knight.Health = MaxHealth;
                //HUD.Instance.HealthBar.maxValue = MaxHealth;
                //HUD.Instance.HealthBar.value = MaxHealth;  
                //добавляем
                hero.maxHealth += item.ItemData.Quantity / 10f;
                break;
            default:
                Debug.LogError("Wrong crystall type!");
                break;
        }

        inventory.Remove(item.ItemData);

        Destroy(item.gameObject);
       
        if (OnUpdateHeroParameters != null)
        {
            OnUpdateHeroParameters(hero);
        }

       
    }



   

    public  void StartNewLevel()
    {
        HUD.Instance.SetScore(Score.ToString());

        if (OnUpdateHeroParameters != null)
        {
            OnUpdateHeroParameters(hero);
        }

        State = GameState.Play;
    }


    public static GameController Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject gameController = Instantiate(Resources.Load("Prefabs/GameController")) as GameObject;
                 
                _instance = gameController.GetComponent<GameController>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        DontDestroyOnLoad(gameObject);
        State = GameState.Play;
        inventory = new List<InventoryItem>();
        InitializeAudioManager();

    }

    public void Hit(IDestructable victim)
    {
        if (victim.GetType() == typeof(Dragon))
        {
            if (victim.Health > 0)
            {
                //дракон получил урон
                Score += dragonHitScore;
            }
            if (victim.GetType() == typeof(Knight))
            {
                HUD.Instance.healthBar.value = victim.Health;
            }

            Debug.Log("Dragon hit.Current score " + score);
        }
        if (victim.GetType() == typeof(Knight)) HUD.Instance.healthBar.value = victim.Health;
       
    }
    public void Killed(IDestructable victim)
    {
        if (victim.GetType() == typeof(Dragon))
        {
            Score += dragonKillScore;

            hero.experience += dragonKillExperience;

            Destroy((victim as MonoBehaviour).gameObject);
        }

        if (victim.GetType() == typeof(Knight))
        {

            GameOver();
        }
    }

    private int Score
    {
        get
        {
            return score;
        }
        set
        {
            if (value != score)
            {
                score = value;
                HUD.Instance.SetScore(score.ToString());
            }

        }


    }
    public GameState State
    {
        get
        {
            return state;
        }
        set
        {
            if (value == GameState.Play) Time.timeScale = 1.0f;
           
            else Time.timeScale = 0.0f;
           
            state = value;
        }
    }

    public Knight Knight { get; internal set; }
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1,LoadSceneMode.Single);
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void PrincessFound()
    {
        HUD.Instance.ShowLevelWonWindow();
    }

    public void GameOver()
    {
        HUD.Instance.ShowLevelLoseWindow();
    }

    public void LevelUp()
    {
        if (OnUpdateHeroParameters != null)
        {
            OnUpdateHeroParameters(hero);//обновление параметров рыцаря и внутриигрового интерфейса + звук апа
        }
    }


   

}
