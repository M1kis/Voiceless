using System.Collections;
using UnityEngine;

public class Pomni_PickUp : MonoBehaviour, IInteractable
{
    [SerializeField]
    private AudioSource pomni_laught;
    [SerializeField]
    private GameObject Fade_In;
    [SerializeField]
    private GameObject Fade_Out;
    [SerializeField]
    private GameObject eastergg_zone;
    [SerializeField]
    private GameObject prision_zone;
    [SerializeField]
    private GameObject teleportZone;

    private CharacterController characterController;
    private GameObject player;
    private BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            characterController = player.GetComponent<CharacterController>();
        }
    }

    public void Interact()
    {
        if (boxCollider != null)
            boxCollider.enabled = false;
        StartCoroutine(InteractSequence());
    }

    private IEnumerator InteractSequence()
    {
        if (pomni_laught != null)
            pomni_laught.Play();

        if (Fade_In != null)
            Fade_In.SetActive(true);

        if (Fade_Out != null)
            Fade_Out.SetActive(false);

        yield return new WaitForSeconds(3f);

        if (characterController != null && player != null && teleportZone != null)
        {
            characterController.enabled = false;
            player.transform.position = teleportZone.transform.position;
            characterController.enabled = true;
        }

        if (Fade_In != null)
            Fade_In.SetActive(false);
        if (Fade_Out != null)
            Fade_Out.SetActive(true);

        if (eastergg_zone != null)
            eastergg_zone.SetActive(true);
        if (prision_zone != null)
            prision_zone.SetActive(false);
    }
}
