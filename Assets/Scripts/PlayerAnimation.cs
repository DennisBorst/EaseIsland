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

    public void PlayAnimCountCanInteract(int animNumber)
    {
        anim.SetInteger("AnimNumber", animNumber);
        characterMovement.CanOnlyInteract(true);
    }

    public void Movement(Vector2 movement, bool isRunning)
    {
        if (isRunning && movement.magnitude > 0.1f)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", true);
        }
        else if (movement.magnitude > 0.1f)
        {
            anim.SetBool("isRunning", false);
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
            anim.SetBool("isWalking", false);
        }
    }

    public void EndOfAnimation()
    {
        anim.SetInteger("AnimNumber", 0);
        characterMovement.FreezePlayer(false);
        characterMovement.CanOnlyInteract(false);
    }

    public void ChangeAnimNumber(int animNumber)
    {
        anim.SetInteger("AnimNumber", animNumber);
    }

    public void InstantiateObject(GameObject gameObj, Transform transform)
    {
        Instantiate(gameObj, transform.position, transform.rotation);
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
