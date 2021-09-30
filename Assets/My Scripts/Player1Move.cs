using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Move : MonoBehaviour
{
    private Animator Anim;
    public float WalkSpeed = 2f;
    private bool IsJumping = false;
    private AnimatorStateInfo Player1Layer0;
    private bool CanWalkLeft = true;
    private bool CanWalkRight = true;

    public GameObject Player1;
    public GameObject Opponent;
    public Vector3 OpponentPosition;

    public bool IsFacingLeft = false;

    private void Start()
    {
        Anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Player1Layer0 = Anim.GetCurrentAnimatorStateInfo(0);

        Vector3 ScreenBounds = Camera.main.WorldToScreenPoint(transform.position);

        CanWalkRight = ScreenBounds.x < Screen.width;
        CanWalkLeft = ScreenBounds.x > 0;

        OpponentPosition = Opponent.transform.position;

        if(OpponentPosition.x > Player1.transform.position.x)
        {
            StartCoroutine(FacingRight());
        }
        else if(OpponentPosition.x < Player1.transform.position.x)
        {
            StartCoroutine(FacingLeft());
        }

        if(Player1Layer0.IsTag("Motion"))
        {
            // Walking left and right
            if (Input.GetAxisRaw("Horizontal") > 0 && CanWalkRight)
            {
                Anim.SetBool("Forward", true);
                transform.Translate(WalkSpeed * Time.deltaTime, 0, 0);
            }

            if (Input.GetAxisRaw("Horizontal") < 0 && CanWalkLeft)
            {
                Anim.SetBool("Backward", true);
                transform.Translate(-WalkSpeed * Time.deltaTime, 0, 0);
            }
        }
        
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            Anim.SetBool("Forward", false);
            Anim.SetBool("Backward", false);
        }

        //Jumping and crouching
        if(Input.GetAxisRaw("Vertical") > 0)
        {
            if(IsJumping == false)
            {
                Anim.SetTrigger("Jump");
                IsJumping = true;
                StartCoroutine(JumpPause());
            }
        }

        if (Input.GetAxisRaw("Vertical") < 0)
        {
            Anim.SetBool("Crouch", true);
        }

        if(Input.GetAxisRaw("Vertical") == 0)
        {
            Anim.SetBool("Crouch", false);
            Anim.ResetTrigger("Jump");
        }
    }    

    private IEnumerator JumpPause()
    {
        yield return new WaitForSeconds(1f);
        IsJumping = false;
    }

    private IEnumerator FacingRight()
    {
        if (!IsFacingLeft) yield break;
        IsFacingLeft = false;
        yield return new WaitForSeconds(0.15f);
        Player1.transform.Rotate(0, 180, 0);
        Anim.SetLayerWeight(1, 0);
    }

    private IEnumerator FacingLeft()
    {
        if (IsFacingLeft) yield break;

        IsFacingLeft = true;
        yield return new WaitForSeconds(0.15f);
        Player1.transform.Rotate(0, 180, 0);
        Anim.SetLayerWeight(1, 1);
    }
}
