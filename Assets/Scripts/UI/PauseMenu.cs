using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public LevelLoader levelLoader;

    public void Quit()
    {
        Time.timeScale = 1f;
        StartCoroutine(levelLoader.LoadLevel(0));
    }
}
