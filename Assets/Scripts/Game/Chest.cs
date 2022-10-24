using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animator anim;
    public GameObject[] gem;
    private GameObject generatedGem = null;
    [SerializeField] private AudioClip chestSFX;
    [SerializeField] private Texture2D normal;
    [SerializeField] private Texture2D clickable;
    [SerializeField] private GameObject dustCloud;
    private AudioSource audioSource;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    //hover
    private void OnMouseOver()
    {
        Cursor.SetCursor(clickable, Vector2.zero, CursorMode.ForceSoftware);
    }
    //click
    private void OnMouseDown()
    {
        
        anim.SetTrigger("Open");
        StartCoroutine(openChest());
    }
    //exit
    private void OnMouseExit()
    {
        Cursor.SetCursor(normal, Vector2.zero, CursorMode.ForceSoftware);
    }

    IEnumerator openChest()
    {
        int randNumber = Random.Range(0, 3);
        yield return new WaitForSeconds(.5f);
        audioSource.PlayOneShot(chestSFX, 1f);
        yield return new WaitForSeconds(.3f);
        GameObject dust = (GameObject)Instantiate(dustCloud, transform.position + new Vector3(0, 0.4f, 0), Quaternion.identity);
        GameObject newGem = (GameObject)Instantiate(gem[randNumber], transform.position+new Vector3(0,0.4f,0), Quaternion.identity);
        generatedGem = newGem;
    }
    private void Update()
    {

        if (generatedGem != null && generatedGem.GetComponent<Gem>().newGenerated)
        {
            transform.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            transform.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
