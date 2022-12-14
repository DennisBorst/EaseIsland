using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToCam : MonoBehaviour
{
    private void Update()
    {
        RotateToCam();
    }
    private void RotateToCam()
    {
        this.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }
}
