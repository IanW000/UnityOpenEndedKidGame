using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Seamless_BGM : MonoBehaviour
{
    //Make music continue playing through scenes
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("BGM");
       if (objs.Length > 1)
          Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }
    private void Update()
    {

        if (SceneManager.GetActiveScene().name=="Main Menu")
        {
            this.gameObject.GetComponent<AudioSource>().volume = 0.191f;
        }
    }
}
