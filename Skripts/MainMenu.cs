using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject OptionsWindow;

    public void Start()
    {
        Time.timeScale = 1f;
       
        GameController.Instance.audioManager.PlaySound("heroic_soft_loop");
        Debug.Log("Start");
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


    public void ButtonPlay()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        GameController.Instance.audioManager.PlaySound("heroic_soft_loop");
        Debug.Log("Play");
    }
  
    public void Options()
    {
        ShowWindow(OptionsWindow);
    }
    
    public void ButtonExit()
    {
        Application.Quit();
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
