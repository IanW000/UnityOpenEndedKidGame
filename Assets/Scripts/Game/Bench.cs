using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bench : MonoBehaviour, IDropHandler
{
    public Vector3 chairPos;
    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag.CompareTag("Player"))
        {
            eventData.pointerDrag.GetComponent<RectTransform>().position = chairPos;
            eventData.pointerDrag.GetComponent<Character>().onEvent = true;
            eventData.pointerDrag.GetComponent<Animator>().SetBool("Sitting", true);
        }
    }
}
