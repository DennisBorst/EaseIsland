using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToScene : MonoBehaviour
{
    //[SerializeField] private int levelIndex;
    [SerializeField] private Transform goToPos;
    [SerializeField] private GameObject enableScene;
    [SerializeField] private GameObject disableScene;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameManger.Instance.SwitchLevel(goToPos, enableScene, disableScene);
        }
    }
}
