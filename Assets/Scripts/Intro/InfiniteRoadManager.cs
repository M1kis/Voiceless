using System.Collections.Generic;
using UnityEngine;

public class InfiniteRoadManager : MonoBehaviour
{
    public GameObject roadSectionPrefab;
    public int maxSections = 2;
    public float moveSpeed = 5f;
    public float sectionLength = 50f;

    private Queue<GameObject> roadSections = new Queue<GameObject>();
    private Vector3 spawnPosition;

    void Start()
    {
        // Busca la sección inicial en la escena
        GameObject firstSection = GameObject.FindWithTag("FirstRoad");
        if (firstSection != null)
        {
            roadSections.Enqueue(firstSection);
            spawnPosition = firstSection.transform.position + Vector3.forward * sectionLength;
        }
        else
        {
            Debug.LogError("No se encontró una sección inicial con la etiqueta 'FirstRoad'");
        }
    }

    void Update()
    {
        foreach (GameObject section in roadSections)
        {
            section.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }
    }

    public void TriggerNextSection()
    {
        if (roadSections.Count >= maxSections)
        {
            GameObject oldSection = roadSections.Dequeue();
            Destroy(oldSection);
        }

        SpawnSection();
    }

    private void SpawnSection()
    {
        Vector3 newPosition;

        if (roadSections.Count > 0)
        {
            // Tomamos la última sección
            GameObject lastSection = roadSections.ToArray()[roadSections.Count - 1];

            // Usamos el BoxCollider para obtener la longitud de la carretera
            float sectionLength = lastSection.GetComponent<Collider>().bounds.size.z;

            // Ajustamos la nueva posición justo al final de la anterior
            newPosition = lastSection.transform.position + Vector3.forward * sectionLength;
        }
        else
        {
            newPosition = Vector3.zero;
        }

        GameObject newSection = Instantiate(roadSectionPrefab, newPosition, Quaternion.identity);
        roadSections.Enqueue(newSection);
    }

}
