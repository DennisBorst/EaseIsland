using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private CharacterMovement characterMovement;

    public void PlayAnimCount(int animNumber)
    {
        anim.SetInteger("AnimNumber", animNumber);
        characterMovement.FreezePlayer(true);
    }

    public void Movement(Vector2 movement)
    {
        if(movement.magnitude > 0.1f)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }

    public void EndOfAnimation()
    {
        anim.SetInteger("AnimNumber", 0);
        characterMovement.FreezePlayer(false);
    }

    public void SetCharacterMovement(CharacterMovement characterMovement)
    {
        this.characterMovement = characterMovement;
    }

    #region Singleton
    private static PlayerAnimation instance;
    private void Awake()
    {
        instance = this;
        anim = GetComponent<Animator>();
    }
    public static PlayerAnimation Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerAnimation();
            }

            return instance;
        }
    }
    #endregion
}
