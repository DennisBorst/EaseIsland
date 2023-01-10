using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToScene : MonoBehaviour
{
    //[SerializeField] private int levelIndex;
    [SerializeField] private Transform goToPos;
    [SerializeField] private GameObject enableScene;
    [SerializeField] private GameObject disableScene;
    [SerializeField] private GameManger.Location goToLocation;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameManger.Instance.SwitchLevel(goToPos, enableScene, disableScene);
            GameManger.Instance.SwitchLocation(goToLocation);
        }
    }
}
