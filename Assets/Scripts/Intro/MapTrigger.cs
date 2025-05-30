using UnityEngine;

public class MapTrigger : MonoBehaviour
{
    private InfiniteRoadManager manager;

    void Start()
    {
        manager = FindObjectOfType<InfiniteRoadManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.TriggerNextSection();
        }
    }
}