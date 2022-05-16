using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDFloatingTextController : MonoBehaviour
{
    [SerializeField] private Animator textAnimator;

    private void OnEnable()
    {
        textAnimator.SetTrigger("onEnableTextFade");
    }

    private void DisableObject()
    {
        textAnimator.ResetTrigger("onEnableTextFade");
        gameObject.SetActive(false);
    }
}
