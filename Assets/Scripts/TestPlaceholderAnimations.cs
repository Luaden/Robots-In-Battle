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
        Win();
        Lose();
        Counter();
    }

    private void Punch()
    {
        if (Input.GetKey(KeyCode.P))
        {
            anim.SetBool("isPunching", true);
        }
        else anim.SetBool("isPunching", false);
    }

    private void Kick()
    {
        if (Input.GetKey(KeyCode.K))
        {
            anim.SetBool("isKicking", true);
        }
        else anim.SetBool("isKicking", false);
    }

    private void Win()
    {
        if (Input.GetKey(KeyCode.W))
        {
            anim.SetBool("isWinning", true);
        }
    }

    private void Lose()
    {
        if (Input.GetKey(KeyCode.L))
        {
            anim.SetBool("isLosing", true);
        }
    }

    private void Counter()
    {
        if (Input.GetKey(KeyCode.C))
        {
            anim.SetBool("isCountering", true);
        }
        else anim.SetBool("isCountering", false);
    }
}
