using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainClothing : MonoBehaviour
{
    public Clothing clothingGained;
    [SerializeField] private Transform itemLoc;

    public void GetClothing(Clothing clothing)
    {
        this.transform.localPosition = new Vector3(0, 0, 0);
        this.transform.localRotation = new Quaternion(0, 0, 0, 0);
        this.transform.position += (Vector3.up * 1.5f);

        GameObject clothObj = Instantiate(clothing.itemObject, itemLoc.position, itemLoc.rotation, itemLoc);
        clothingGained = clothing;
        clothObj.layer = 8;
        foreach (Transform child in clothObj.transform)
        {
            child.gameObject.layer = 8;
        }
    }

    public void OpenBackPack()
    {
        PlayerAnimation.Instance.OpenBackPack();
    }

    public void EndOfAnimation()
    {
        //ClothManager.Instance.AddCloth(clothingGained);
        Destroy(this.gameObject);
    }
}
