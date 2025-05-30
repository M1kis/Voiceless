using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerEntityActivator : MonoBehaviour
{
    [Header("Objetos a activar inmediatamente")]
    public List<GameObject> objectsToActivate;

    [Header("Objetos a desactivar inmediatamente")]
    public List<GameObject> objectsToDeactivate;

    [Header("GameObject que aparece tras 10 segundos")]
    public GameObject delayedObject;
    public GameObject delayedObject2;

    [Header("Una sola vez")]
    public bool triggerOnce = true;

    private bool alreadyTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (alreadyTriggered && triggerOnce) return;

        if (other.CompareTag("Player"))
        {

            foreach (GameObject obj in objectsToActivate)
            {
                if (obj != null) obj.SetActive(true);
            }

            foreach (GameObject obj in objectsToDeactivate)
            {
                if (obj != null) obj.SetActive(false);
            }

            if (delayedObject != null)
            {
                StartCoroutine(ActivateDelayedObject());
            }

            alreadyTriggered = true;
        }
    }

    private IEnumerator ActivateDelayedObject()
    {
        yield return new WaitForSeconds(1.4f);
        delayedObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        delayedObject2.SetActive(true);
    }
}
