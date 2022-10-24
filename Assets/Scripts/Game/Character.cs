using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Character : MonoBehaviour, IPointerDownHandler, IBeginDragHandler,IEndDragHandler, IDragHandler
{
    private Vector3 mousePositionOffset;
    private AudioSource audioSource;
    private bool PlayOnce;
    private Animator anim;
    private float horizontalVelocity;
    private Vector3 startPoint, endPoint, pointDifference;
    private CanvasGroup canvasGroup;
    private BoxCollider2D bench,stage;


    public bool dragging,onEvent;
    public float dropSpeed;
    public AudioSource song;

    [SerializeField] private AudioClip pickSFX;
    [SerializeField] private AudioClip dropSFX;
    [SerializeField] private Texture2D normal;
    [SerializeField] private Texture2D clickable;

    private void Awake()
    {
        dropSpeed = 2;
        dragging = false;
        audioSource = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();
        Cursor.SetCursor(normal, Vector2.zero, CursorMode.ForceSoftware) ;
        bench = GameObject.Find("bench").GetComponent<BoxCollider2D>();
        stage = GameObject.Find("stage").GetComponent<BoxCollider2D>();
    }

    //get mouse position
    private Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    //hover - change cursor outlook
    private void OnMouseOver()
    {
        Cursor.SetCursor(clickable, Vector2.zero, CursorMode.ForceSoftware);
    }
    //click
    public void OnPointerDown(PointerEventData eventData)
    {
        anim.SetBool("Sitting", false);
        anim.SetBool("Singing", false);
        song.Stop();
        onEvent = false;
    }
    //start to drag
    public void OnBeginDrag(PointerEventData eventData)
    {
        startPoint = Input.mousePosition;
        dropSpeed = 0;
        dragging = true;
        mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
        audioSource.PlayOneShot(pickSFX);
        canvasGroup.blocksRaycasts = false;
    }
    //on drag - change object position
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = GetMouseWorldPosition() + mousePositionOffset;
    }
    //end drag
    public void OnEndDrag(PointerEventData eventData)
    {
        endPoint = Input.mousePosition;
        dragging = false;
        canvasGroup.blocksRaycasts = true;
    }

    
    ////exit
    private void OnMouseExit()
    {
        Cursor.SetCursor(normal, Vector2.zero, CursorMode.ForceSoftware);
    }

    //use code to simulate the fruit falling down in a accelerated value instead of using Unity’s physics system
    //btw the 2D Collider attached to Fruit objects are only for interaction with mouse event
    private void Update()
    {
        //use startPoint (the user click) minus endPoint (the user release) to get distance how long user drag the objects, then convert that value into horizontal velocity
        pointDifference = startPoint - endPoint;
        horizontalVelocity = pointDifference.x;
        horizontalVelocity /= -5000;
        if (horizontalVelocity > 1)
            horizontalVelocity = 1;
        else if (horizontalVelocity < -1)
            horizontalVelocity = -1;
        else if (horizontalVelocity >= -0.005 && horizontalVelocity <= 0.005)
            horizontalVelocity = 0;

        if (dragging)
        {
            PlayOnce = false;
            bench.enabled = true;
            stage.enabled = true;
            bench.GetComponent<Animator>().SetBool("dragging", true);
            stage.GetComponent<Animator>().SetBool("dragging", true);
        }
        else
        {
            bench.enabled = false;
            stage.enabled = false;
            bench.GetComponent<Animator>().SetBool("dragging", false);
            stage.GetComponent<Animator>().SetBool("dragging", false);

        }

        if (!dragging && !onEvent)
        {
            if (transform.position.y >= -3)
            {

                //fall rate fomula
                dropSpeed += Time.deltaTime * 9.8f;
                //the fruit get horizontal velocity when droping down
                transform.position = new Vector3(transform.position.x + horizontalVelocity * Time.deltaTime * 100, transform.position.y - dropSpeed * Time.deltaTime, transform.position.z);
            }
            else
            {
                //play fall animation & SFX once when the fruit fall on the ground, if the fruit is repicked after falling on the ground, then animation & SFX will be reactived
                if (!PlayOnce)
                {
                    StartCoroutine(hitGroundSFX(dropSpeed));
                    PlayOnce = true;
                }
            }
        }

        if (onEvent)
        {
            bench.enabled = false;
            stage.enabled = false;
        }
        else
        {
            bench.enabled = true;
            stage.enabled = true;
        }
    }
    //since the fruit hit ground 2 times (for dropping animation), the SFX play two times as well
    //SFX volume are affected by drop speed, faster drop speed for higher volume
    IEnumerator hitGroundSFX(float dropSpeed)
    {
        if (dropSpeed <= 4)
            dropSpeed = 4.0001f;
        float magnitude = 0.1f * (dropSpeed - 4);
        if (magnitude > 1f)
            magnitude = 1f;
        else if (magnitude <= 0.2f)
            magnitude = 0.2f;
        audioSource.PlayOneShot(dropSFX, magnitude);
        yield return new WaitForSeconds(1);
        audioSource.PlayOneShot(dropSFX, magnitude / 2);
    }
}

