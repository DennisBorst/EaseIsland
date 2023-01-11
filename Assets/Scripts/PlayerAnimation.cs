using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private ParticleSystem runPar;

    private Animator anim;
    private CharacterMovement characterMovement;

    [SerializeField] private Animator animBackPack;

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
            anim.SetFloat("WalkSpeed", 1.6f);
            anim.SetBool("isWalking", true);
            //anim.SetBool("isRunning", true);
        }
        else if (movement.magnitude > 0.1f)
        {
            anim.SetFloat("WalkSpeed", 1f);
            //anim.SetBool("isRunning", false);
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
            anim.SetBool("isWalking", false);
        }

        if (isRunning && !runPar.isEmitting) { runPar.Play(); }
        else if(!isRunning) { runPar.Stop(); }
    }

    public void EndOfAnimation()
    {
        anim.SetInteger("AnimNumber", -1);
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

    public void OpenBackPack()
    {
        animBackPack.SetTrigger("OpenBackPack");
    }

    #region Singleton
    private static PlayerAnimation instance;
    private void Awake()
    {
        instance = this;
        anim = GetComponent<Animator>();
        runPar.Stop(true, ParticleSystemStopBehavior.StopEmitting);
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
