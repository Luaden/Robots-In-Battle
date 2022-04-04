using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardUIInteractionController : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    private Transform currentParent;
    private Transform previousParent;

    private CardUIObject cardUIObject;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Highlight
        throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Unhighlight
        throw new System.NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Set parent to null, follow pointer
        //Remove collider check
        throw new System.NotImplementedException();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Check pointer location for location to drop card
        //Add parent to new location or set current parent to previous parent
        throw new System.NotImplementedException();
    }

    private void Awake()
    {
        cardUIObject = GetComponent<CardUIObject>();
    }

    private void Update()
    {
        MoveToCurrentParent();
    }

    private void MoveToCurrentParent()
    {
        if(currentParent != null && transform.position != currentParent.position)
        {
            //Move to parent target
        }
    }
}
