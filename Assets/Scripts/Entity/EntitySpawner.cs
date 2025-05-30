using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [Header("Dependencias")]
    public PointCounter pointCounter;
    public Transform player;

    [Header("Prefabs de entidades")]
    public GameObject entity10Prefab;
    public GameObject entity60Prefab;

    [Header("Puntos de spawn")]
    public Transform[] spawners; // Asignar Spawner 1, 2, 3, 4 en orden

    private GameObject entity10Instance;
    private GameObject entity60Instance;
    private bool hasSpawned10 = false;
    private bool hasSpawned60 = false;

    void Update()
    {
        if (pointCounter == null || player == null) return;

        float progress = pointCounter.GetProgress();

        if (!hasSpawned10 && progress >= 0.10f)
        {
            SpawnEntity10();
        }

        if (!hasSpawned60 && progress >= 0.60f)
        {
            SpawnEntity60();
        }
    }

    void SpawnEntity10()
    {
        if (entity10Prefab == null || spawners.Length == 0) return;

        int index = Random.Range(0, spawners.Length);
        entity10Instance = Instantiate(entity10Prefab, spawners[index].position, Quaternion.identity);
        entity10Instance.SetActive(true);
        hasSpawned10 = true;
    }

    void SpawnEntity60()
    {
        if (entity60Prefab == null || spawners.Length == 0) return;

        Vector3 referencePosition = entity10Instance != null ? entity10Instance.transform.position : player.position;

        Transform farthestSpawn = spawners[0];
        float maxDistance = Vector3.Distance(referencePosition, spawners[0].position);

        for (int i = 1; i < spawners.Length; i++)
        {
            float dist = Vector3.Distance(referencePosition, spawners[i].position);
            if (dist > maxDistance)
            {
                maxDistance = dist;
                farthestSpawn = spawners[i];
            }
        }

        entity60Instance = Instantiate(entity60Prefab, farthestSpawn.position, Quaternion.identity);
        entity60Instance.SetActive(true);
        hasSpawned60 = true;
    }
}
