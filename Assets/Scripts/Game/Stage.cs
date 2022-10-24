using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Stage : MonoBehaviour, IDropHandler
{
    public Vector3 stagePos;
    public AudioSource song;
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.CompareTag("Player"))
        {
            eventData.pointerDrag.GetComponent<RectTransform>().position = stagePos;
            eventData.pointerDrag.GetComponent<Character>().onEvent = true;
            eventData.pointerDrag.GetComponent<Animator>().SetBool("Singing", true);
            song.Play();
        }
    }
}