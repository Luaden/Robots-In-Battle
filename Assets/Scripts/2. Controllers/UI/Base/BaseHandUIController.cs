using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseHandUIController : MonoBehaviour
{
    [SerializeField] private GameObject cardSlotPrefab;
    [SerializeField] private float vertCardSlotPadding;

    private void AddToHand(CardUIInteractionController cardUIInteractionController)
    {

    }

    private void RemoveCardFromHand(CardUIInteractionController cardUIInteractionController)
    {

    }
}
