using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDress : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer characterMeshRen;

    public void UpdateCharacter(SkinnedMeshRenderer characterMeshRen)
    {
        this.characterMeshRen.materials = characterMeshRen.materials;
    }
}
