using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSpawner : MonoBehaviour
{
    public GameObject starPrefab;
    public float spawnInterval = 5f;  // Time in seconds between each star spawn
    public float startDelay = 10f;     // Initial delay before stars start spawning

    void Start()
    {
        StartCoroutine(SpawnStarsPeriodically());
    }

    Vector3 GetRandomPositionInsideCameraView(Camera camera)
    {
        float zDistanceFromCamera = 10f;
        Vector3 randomViewportPoint = new Vector3(Random.Range(0f, 0.5f), Random.Range(0f, 0.5f), zDistanceFromCamera);
        return camera.ViewportToWorldPoint(randomViewportPoint);
    }

    IEnumerator SpawnStarsPeriodically()
    {
        yield return new WaitForSeconds(startDelay);
        if (!GameManager.gameIsPaused)
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnInterval);
                Camera mainCamera = Camera.main;
                if (mainCamera != null)
                {
                    Vector3 randomPosition = GetRandomPositionInsideCameraView(mainCamera);
                    Instantiate(starPrefab, randomPosition, Quaternion.identity);
                }
            }
        }
        
    }
}
