using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pomni_JumpScare : MonoBehaviour
{
    [SerializeField]
    private GameObject pomni_panel;
    [SerializeField]
    private GameObject blackscree_panel;
    [SerializeField]
    private AudioSource jumpScare;
    [SerializeField]
    private GameObject fade_out;
    [SerializeField]
    private GameObject teleportZone;
    [SerializeField]
    private GameObject eastergg_zone;
    [SerializeField]
    private GameObject prision_zone;

    [Header("Objetos a desactivar en el susto")]
    [SerializeField]
    private List<GameObject> objectsToDeactivate;

    private CharacterController characterController;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            characterController = player.GetComponent<CharacterController>();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (objectsToDeactivate != null)
            {
                foreach (GameObject obj in objectsToDeactivate)
                {
                    if (obj != null)
                        obj.SetActive(false);
                }
            }

            fade_out.SetActive(false);
            pomni_panel.SetActive(true);
            jumpScare.Play();
            StartCoroutine(TeleportPlayer());
        }
    }

    private IEnumerator TeleportPlayer()
    {
        yield return new WaitForSecondsRealtime(1.5f);


        if (blackscree_panel != null)
            blackscree_panel.SetActive(true);

        if (fade_out != null)
            fade_out.SetActive(false);

        if (pomni_panel != null)
            pomni_panel.SetActive(false);

        if (jumpScare != null)
            jumpScare.Stop();

        yield return new WaitForSeconds(3f);

        if (characterController != null && player != null && teleportZone != null)
        {
            characterController.enabled = false;
            player.transform.position = teleportZone.transform.position;
            characterController.enabled = true;
        }

        if (blackscree_panel != null)
            blackscree_panel.SetActive(false);
        if (fade_out != null)
            fade_out.SetActive(true);

        if (eastergg_zone != null)
            eastergg_zone.SetActive(false);
        if (prision_zone != null)
            prision_zone.SetActive(true);

    }

}
