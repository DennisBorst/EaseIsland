using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDress : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer characterMeshRen;

    [Header("Hat")]
    [SerializeField] private Transform hat;

    [Header("Shirt")]
    [SerializeField] private Transform shirtBody;
    [SerializeField] private Transform shirtPipeL;
    [SerializeField] private Transform shirtPipeR;

    [Header("Pants")]
    [SerializeField] private Transform pipeL;
    [SerializeField] private Transform pipeR;

    [Header("Shoes")]
    [SerializeField] private Transform shoeL;
    [SerializeField] private Transform shoeR;

    public void UpdateCharacter(SkinnedMeshRenderer characterMeshRen, Clothing hat, Clothing shirt, Clothing pants, Clothing shoes)
    {
        this.characterMeshRen.materials = characterMeshRen.materials;
        EmptyChildren();

        Instantiate(hat.hat, this.hat.position, this.hat.rotation, this.hat).layer = 8;
        Instantiate(shirt.shirt[0], this.shirtBody.position, this.shirtBody.rotation, this.shirtBody).layer = 8;
        Instantiate(shirt.shirt[1], this.shirtPipeL.position, this.shirtPipeL.rotation, this.shirtPipeL).layer = 8;
        Instantiate(shirt.shirt[2], this.shirtPipeR.position, this.shirtPipeR.rotation, this.shirtPipeR).layer = 8;
        Instantiate(pants.pants[0], this.pipeL.position, this.pipeL.rotation, this.pipeL).layer = 8;
        Instantiate(pants.pants[1], this.pipeR.position, this.pipeR.rotation, this.pipeR).layer = 8;
        Instantiate(shoes.shoes[0], this.shoeL.position, this.shoeL.rotation, this.shoeL).layer = 8;
        Instantiate(shoes.shoes[1], this.shoeR.position, this.shoeR.rotation, this.shoeR).layer = 8;

        ChangeLayerChilds(this.hat);
        ChangeLayerChilds(shoeL);
        ChangeLayerChilds(shoeR);
    }

    private void ChangeLayerChilds(Transform root)
    {
        var children = root.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children)
        {
            child.gameObject.layer = 8;
        }
    }

    private void EmptyChildren()
    {
        Destroy(hat.transform.GetChild(0).gameObject);
        Destroy(shirtBody.transform.GetChild(0).gameObject);
        Destroy(shirtPipeL.transform.GetChild(0).gameObject);
        Destroy(shirtPipeR.transform.GetChild(0).gameObject);
        Destroy(pipeL.transform.GetChild(0).gameObject);
        Destroy(pipeR.transform.GetChild(0).gameObject);
        Destroy(shoeL.transform.GetChild(0).gameObject);
        Destroy(shoeR.transform.GetChild(0).gameObject);
    }

}
