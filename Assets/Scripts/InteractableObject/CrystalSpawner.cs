using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSpawner : MonoBehaviour
{
    [SerializeField] private int maxSpawned;
    [SerializeField] private float minTimeToSpawn;
    [SerializeField] private float maxTimeToSpawn;
    [Space]
    [SerializeField] private GameObject[] spawnObjects;
    [SerializeField] private Transform[] spawnPoints;

    private List<Transform> currentSpawnPointsLeft = new List<Transform>();
    private int currentActive;

    private float randomTime;
    private float elapsedTime;
    private float timeWeStarted;

    private bool timerRunning;

    public void CrystalDestroyed()
    {
        currentActive -= 1;
    }

    private void Awake()
    {
        CheckSpawnPoints();
        NewRandomTime();
        for (int i = 0; i < maxSpawned; i++)
        {
            int randomObjInt = Random.Range(0, spawnObjects.Length);
            int randomPosInt = Random.Range(0, currentSpawnPointsLeft.Count);
            currentActive += 1;
            Mineable mineAble = Instantiate(spawnObjects[randomObjInt], currentSpawnPointsLeft[randomPosInt].position, currentSpawnPointsLeft[randomPosInt].rotation, currentSpawnPointsLeft[randomPosInt]).GetComponent<Mineable>();
            mineAble.crystalSpawner = this;
            currentSpawnPointsLeft.RemoveAt(randomPosInt);
        }
        StartCoroutine(UpdateTimerIE());
    }

    private void UpdateTimer()
    {
        if (currentActive >= maxSpawned) { return; }

        if (!timerRunning)
        {
            NewRandomTime();
            StartCoroutine(SpawnNewCrystal(randomTime));
        }
    }

    private void NewRandomTime()
    {
        randomTime = Random.Range(minTimeToSpawn, maxTimeToSpawn);
    }

    private void SpawnRandomObject()
    {
        CheckSpawnPoints();
        NewRandomTime();
        currentActive += 1;

        int randomObjInt = Random.Range(0, spawnObjects.Length);
        int randomPosInt = Random.Range(0, currentSpawnPointsLeft.Count);
        Mineable mineAble = Instantiate(spawnObjects[randomObjInt], currentSpawnPointsLeft[randomPosInt].position, currentSpawnPointsLeft[randomPosInt].rotation, currentSpawnPointsLeft[randomPosInt]).GetComponent<Mineable>();
        mineAble.crystalSpawner = this;
        currentSpawnPointsLeft.RemoveAt(randomPosInt);
    }

    private void CheckSpawnPoints()
    {
        currentSpawnPointsLeft.Clear();

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if(spawnPoints[i].transform.childCount == 0)
            {
                currentSpawnPointsLeft.Add(spawnPoints[i]);
            }
        }
    }

    private IEnumerator UpdateTimerIE()
    {
        WaitForSeconds wait = new WaitForSeconds(0f);

        while (true)
        {
            yield return wait;
            UpdateTimer();
        }
    }

    private IEnumerator SpawnNewCrystal(float randomTime)
    {
        timerRunning = true;
        yield return new WaitForSeconds(randomTime);
        SpawnRandomObject();
        timerRunning = false;
    }
}