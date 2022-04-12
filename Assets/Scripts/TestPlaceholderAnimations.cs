using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlaceholderAnimations : MonoBehaviour
{
    public Animator anim;
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

 
    void Update()
    {
        Punch();
        Kick();
    }

    private void Punch()
    {
        if (Input.GetKey(KeyCode.W))
        {
            anim.SetBool("isPunching", true);
        }
        else anim.SetBool("isPunching", false);
    }

    private void Kick()
    {
        if (Input.GetKey(KeyCode.E))
        {
            anim.SetBool("isKicking", true);
        }
        else anim.SetBool("isKicking", false);
    }
}
