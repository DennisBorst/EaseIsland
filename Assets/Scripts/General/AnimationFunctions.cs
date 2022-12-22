using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFunctions : MonoBehaviour
{
    private Animator anim;

    public void ChangeAnimNumber(int animNumber)
    {
        anim.SetInteger("AnimNumber", animNumber);
    }

    public void DisableObject()
    {
        this.gameObject.SetActive(false);
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
}
