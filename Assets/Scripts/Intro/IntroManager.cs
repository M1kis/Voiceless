using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class IntroManager : MonoBehaviour
{
    [Header("Referencias de Escena")]
    public GameObject blackScreen;
    public GameObject blackSreen2;
    public GameObject fadeOutScreen;       
    public TextMeshProUGUI tituloInicial;  
    public AudioSource musicBobMarley;
    public AudioSource audioPrincipal;
    public AudioSource carEngine;
    public GameObject entidadOscura;
    private bool emisoraCambiada = false;
    public TextMeshProUGUI subtituloTMP;         
    public BoxCollider radioBoxCollider;
    public Animator lucesAnimator;
    public InfiniteRoadManager roadManager;
    public GameObject panelInteraccionLlave;
    public AudioSource audioIntentoEncendido;
    public AudioSource audioEncendidoFinal;
    public AudioSource audioSuspiro;

    void Start()
    {
        StartCoroutine(SecuenciaIntro());
    }

    IEnumerator SecuenciaIntro()
    {
        // 1. Música + pantalla negra ya activa
        musicBobMarley.Play();

        // 2. Esperar antes de mostrar título
        yield return new WaitForSeconds(4f);

        // Activar título (tiene animación propia)
        tituloInicial.gameObject.SetActive(true);

        // Esperar duración de la animación del título
        yield return new WaitForSeconds(9f); // o la duración real de tu animación

        // 3. Transición: desactivar pantalla negra y activar fade-out
        blackScreen.SetActive(false);
        fadeOutScreen.SetActive(true);
        StartCoroutine(FadeInMotorSound());

        // Esperar a que el fade-out termine (si tiene animación, ajusta el tiempo)
        yield return new WaitForSeconds(2f);
        fadeOutScreen.SetActive(false);

        // 4. Mostrar instrucción y activar interacción con radio
        yield return new WaitForSeconds(8f);
        yield return StartCoroutine(MostrarSubtituloConRadio("Veamos qué escuchamos en la siguiente emisora."));

        // 5. Esperar interacción o cambio automático
        yield return StartCoroutine(EsperarCambioDeEmisora());

        // 6. Reproducir audio principal
        audioPrincipal.Play();

        // 7. Esperar hasta que falten 9 segundos
        yield return new WaitForSeconds(audioPrincipal.clip.length - 9f);

        // Iniciar animación de parpadeo
        lucesAnimator.SetBool("Parpadeando", true);
        StartCoroutine(DetenerAutoDuranteParpadeo());

        // 8. Esperar 10 segundos, luego simular auto apagado
        yield return new WaitForSeconds(10f);

        // 9. Activar interacción con llave
        StartCoroutine(MostrarSubtituloFinal());
    }

    IEnumerator MostrarSubtituloConRadio(string texto)
    {
        subtituloTMP.text = texto;
        subtituloTMP.gameObject.SetActive(true);

        // Activar collider del radio para permitir interacción
        radioBoxCollider.enabled = true;

        // Mostrar el subtítulo por 3 segundos
        yield return new WaitForSeconds(3f);

        subtituloTMP.gameObject.SetActive(false);

        // Esperar interacción o cambio automático
        yield return StartCoroutine(EsperarCambioDeEmisora());

        // Una vez interactuado o pasados los 2 minutos, desactivar collider
        radioBoxCollider.enabled = false;
    }

    IEnumerator EsperarCambioDeEmisora()
    {
        float tiempo = 0;
        while (!emisoraCambiada && tiempo < 120f)
        {
            tiempo += Time.deltaTime;
            yield return null;
        }
        if (!emisoraCambiada)
        {
            CambiarEmisora();
        }
    }

    public void InteractuarConRadio()
    {
        if (!emisoraCambiada)
        {
            CambiarEmisora();
        }
    }

    void CambiarEmisora()
    {
        emisoraCambiada = true;
        musicBobMarley.Stop();
       
    }

    IEnumerator DetenerAutoDuranteParpadeo()
    {
        // Espera 8 segundos antes de empezar a frenar (si la animación dura 10)
        yield return new WaitForSeconds(8f);

        float duracionFrenado = 3f; // Tiempo en segundos para que se detenga
        float tiempo = 0f;

        float velocidadInicial = roadManager.moveSpeed;
        float volumenInicial = 0.15f;

        while (tiempo < duracionFrenado)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracionFrenado;

            roadManager.moveSpeed = Mathf.Lerp(velocidadInicial, 0f, t);
            carEngine.volume = Mathf.Lerp(volumenInicial, 0f, t);

            yield return null;
        }

        carEngine.Stop();
        roadManager.moveSpeed = 0f;
    }

    IEnumerator FadeInMotorSound()
    {
        carEngine.volume = 0f;
        carEngine.Play();

        float duracionFadeIn = 2f;
        float tiempo = 0f;
        float volumenFinal = 0.15f;

        while (tiempo < duracionFadeIn)
        {
            tiempo += Time.deltaTime;
            carEngine.volume = Mathf.Lerp(0f, volumenFinal, tiempo / duracionFadeIn);
            yield return null;
        }

        carEngine.volume = volumenFinal;
    }

    IEnumerator MostrarSubtituloFinal()
    {
        yield return new WaitForSeconds(3f);

        audioSuspiro.Play();

        yield return new WaitForSeconds(2f);
        // Primer subtítulo
        subtituloTMP.text = "Qué... que fue eso? ... Qué está pasando?";
        subtituloTMP.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        subtituloTMP.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);

        // Segundo subtítulo
        subtituloTMP.text = "Tengo que volver a encender el auto";
        subtituloTMP.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        subtituloTMP.gameObject.SetActive(false);

        // Iniciar interacción con el panel
        yield return StartCoroutine(IntentarEncenderAuto());
    }

    IEnumerator IntentarEncenderAuto()
    {
        for (int i = 0; i < 3; i++)
        {
            panelInteraccionLlave.SetActive(true);

            yield return new WaitUntil(() =>
            {
                return Input.GetKeyDown(KeyCode.E);
            });

            panelInteraccionLlave.SetActive(false);

            if (i < 2)
            {
                audioIntentoEncendido.Play();
                yield return new WaitForSeconds(audioIntentoEncendido.clip.length + 1f);
            }
            else
            {
                lucesAnimator.SetBool("Parpadeando", false);
                audioEncendidoFinal.Play();

                yield return new WaitForSeconds(11f);

                blackSreen2.SetActive(true);
                audioSuspiro.Stop();
                audioEncendidoFinal.Stop();

                yield return new WaitForSeconds(3f);

                SceneManager.LoadScene("Level_1");

            }
        }
    }
}
