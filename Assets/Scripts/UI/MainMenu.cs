using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Start and Exit button function
    public LevelLoader levelLoader;
   
    public void GoToGameScreen(){
        StartCoroutine(levelLoader.LoadLevel(1));
    }

    public void quitGame(){
        Application.Quit();
    }
}
