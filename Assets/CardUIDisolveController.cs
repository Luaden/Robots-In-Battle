using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUIDisolveController : MonoBehaviour
{
    [SerializeField] private CardUIController controller;
    private void DestroyCardUIUI()
    {
        controller.DestroyCardUI();
    }
}
