using System.Collections;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab; // Prefab de la plataforma
    [SerializeField] private Transform[] pathPoints; // Puntos del recorrido
    [SerializeField] private float spawnInterval = 3.0f; // Intervalo de tiempo entre plataformas

    private void Start()
    {
        StartCoroutine(SpawnPlatforms());
    }

    private IEnumerator SpawnPlatforms()
    {
        while (true)
        {
            GameObject platform = Instantiate(platformPrefab, transform.position, Quaternion.identity);
            PlatformMover mover = platform.GetComponent<PlatformMover>();

            if (mover != null && pathPoints.Length > 0)
            {
                mover.InitializePath(pathPoints);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}