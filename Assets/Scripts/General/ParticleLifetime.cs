using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLifetime : MonoBehaviour
{
    [SerializeField] private float lifeTime;

    private void Start()
    {
        StartCoroutine(ParticleLifeTime());
    }

    private IEnumerator ParticleLifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(this.gameObject);
    }
}
