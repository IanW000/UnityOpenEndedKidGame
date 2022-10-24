using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class fruitTree : MonoBehaviour
{
    public GameObject fruit;
    public GameObject[] spawnPoints;
    private GameObject[] generatedFruit = { null, null, null };

    [SerializeField] private GameObject dustCloud;
    [SerializeField] private AudioSource dustSFX;

    private void Start()
    {
        //At the game beginning, spawn 3 fruits on the trees at random spawning points
        for (int i = 0; i < 3; i++)
        {
            GameObject dust = (GameObject)Instantiate(dustCloud, spawnPoints[i].transform.position, dustCloud.transform.rotation);
            GameObject newFruit = (GameObject)Instantiate(fruit, spawnPoints[i].transform.position, Quaternion.identity);
            newFruit.transform.SetParent(transform);
            generatedFruit[i] = newFruit;
        }
    }

    //if a fruit is picked(not on the tree), then a new fruit spawns at the same place and replace the old fruit in the array
    private void Update()
    {
        for(int i = 0; i < generatedFruit.Length; i++) { 
            if (!generatedFruit[i].GetComponent<Fruit>().onTree)
            {
                    GameObject dust = (GameObject)Instantiate(dustCloud, spawnPoints[i].transform.position, dustCloud.transform.rotation);
                    GameObject newFruit = (GameObject)Instantiate(fruit, spawnPoints[i].transform.position, Quaternion.identity);
                    newFruit.transform.SetParent(transform);
                    generatedFruit[i] = newFruit;
                    dustSFX.Play();
            }

        }
    }
}
