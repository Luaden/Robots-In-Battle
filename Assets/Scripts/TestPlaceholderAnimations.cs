using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlaceholderAnimations : MonoBehaviour
{
    public Animator anim;

    public void DoAThing()
    {

    }

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
        Guard();
        Damage();
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

    private void Guard()
    {
        if (Input.GetKey(KeyCode.G))
        {
            anim.SetBool("isGuarding", true);
        }
        else anim.SetBool("isGuarding", false);
    }

    private void Damage()
    {
        if (Input.GetKey(KeyCode.D))
        {
            anim.SetBool("isDamaged", true);
        }
        else anim.SetBool("isDamaged", false);
    }
}
