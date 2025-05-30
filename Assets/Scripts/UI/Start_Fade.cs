using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Start_Fade : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    public GameObject sub;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        StartCoroutine(MoveAndPause());
    }

    IEnumerator MoveAndPause()
    {
        yield return new WaitForSeconds(2f); // Espera 1 segundo
        if (videoPlayer != null)
        {
            videoPlayer.Pause(); // Pausa el VideoPlayer
        }

        sub.SetActive(false);
    }
}
