using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class OpcionesMenu : MonoBehaviour
{
    public GameObject menuPrincipalCanvas;
    public GameObject opcionesCanvas;
    public Slider volumenSlider; // Slider para volumen
    public Slider sliderBrillo; // Slider para brillo
    public Slider sliderSensibilidad; // Slider para sensibilidad
    public TextMeshProUGUI pantallaCompletaText; // Texto de pantalla completa
    public TextMeshProUGUI resolucionText; // Texto de resolución
    public TextMeshProUGUI volumenTexto;
    public TextMeshProUGUI brilloTexto;
    public TextMeshProUGUI sensibilidadTexto;
    public Image pantallaNegra; // Referencia a la imagen de la pantalla negra
    public TextMeshProUGUI volverText;
    public Resolution[] resoluciones;
    private int resolucionIndex = 0; // Índice de la resolución actual
    private int selectedIndex = 0; // Índice de la opción seleccionada
    private bool pantallaCompleta;
    private Coroutine blinkCoroutine; //parpadeo de opción


    private void Start()
    {
        selectedIndex = 0;
        sliderSensibilidad.value = 1; //valor inicial del slider de la sensibilidad
        volumenSlider.value = AudioListener.volume;
        pantallaCompletaText.text = Screen.fullScreen ? "Pantalla Completa: ON" : "Pantalla Completa: OFF";

        RevisarResolucion(); // Carga las resoluciones al iniciar
        foreach (Resolution res in resoluciones)
        {
            Debug.Log("Resolucion disponible: " + res.width + "x" + res.height);
        }
        ActualizarSeleccion();
        ActualizarTextoResolucion();

        //BRILLO
        // Cargar el brillo guardado (si no existe, usa 1 como valor por defecto)
        float brilloGuardado = PlayerPrefs.GetFloat("Brillo", 1f);
        sliderBrillo.value = brilloGuardado;

        // Aplicar el brillo cargado
        CambiarBrillo();

        // Asegurar que la función se ejecute cada vez que el slider cambie
        sliderBrillo.onValueChanged.AddListener(delegate { CambiarBrillo(); });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            selectedIndex = (selectedIndex + 1) % 6; // 6 opciones en el menú
            ActualizarSeleccion();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            selectedIndex = (selectedIndex - 1 + 6) % 6;
            ActualizarSeleccion();
        }

        switch (selectedIndex)
        {
            case 0: // Volumen
                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                    volumenSlider.value -= 0.01f;
                if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                    volumenSlider.value += 0.01f;
                AudioListener.volume = volumenSlider.value;
                break;

            case 1: // Brillo
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    sliderBrillo.value -= 0.1f; // Baja el brillo
                    CambiarBrillo();
                }
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    sliderBrillo.value += 0.1f; // Sube el brillo
                    CambiarBrillo();
                }
                break;
            case 2: //Sensibilidad
                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                    sliderSensibilidad.value -= 0.01f;
                if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                    sliderSensibilidad.value += 0.01f;
                break;

            case 3: // Pantalla Completa
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) ||
                    Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                {
                    StartCoroutine(ToggleFullScreen());
                }
                break;

            case 4: // Resolución
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                {
                    resolucionIndex = (resolucionIndex - 1 + resoluciones.Length) % resoluciones.Length;
                    CambiarResolucion();
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                {
                    resolucionIndex = (resolucionIndex + 1) % resoluciones.Length;
                    CambiarResolucion();
                }
                break;
            case 5: //volver
                if (opcionesCanvas.activeSelf && Input.GetKeyDown(KeyCode.Return))
                {
                    opcionesCanvas.SetActive(false);  // Desactiva el menú de opciones
                    menuPrincipalCanvas.SetActive(true);  // Activa el menú principal
                }
                break;
        }
        // Si el menú de opciones está activo y se presiona Esc, volver al menú principal
        if (opcionesCanvas.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            opcionesCanvas.SetActive(false);  // Desactiva el menú de opciones
            menuPrincipalCanvas.SetActive(true);  // Activa el menú principal
        }
    }
    void CambiarBrillo()
    {
        Color color = pantallaNegra.color;
        color.a = 1 - sliderBrillo.value; // Reduce la opacidad al subir el brillo
        pantallaNegra.color = color;

        // Guardar el valor en PlayerPrefs
        PlayerPrefs.SetFloat("Brillo", sliderBrillo.value);
        PlayerPrefs.Save();
    }
    IEnumerator ToggleFullScreen()
    {
        pantallaCompleta = !pantallaCompleta;
        yield return new WaitForEndOfFrame(); // Espera un frame para evitar bloqueos
        Screen.fullScreen = pantallaCompleta;
        ActualizarPantallaCompletaTexto();
    }
    void ActualizarPantallaCompletaTexto()
    {
        pantallaCompletaText.text = $"FULL SCREEN: {(pantallaCompleta ? "ON" : "OFF")}";
    }

    void RevisarResolucion()
    {
        resoluciones = Screen.resolutions; // Obtener resoluciones disponibles

        // Cargar la resolución guardada (si no existe, usar la predeterminada 0)
        resolucionIndex = PlayerPrefs.GetInt("numeroResolucion", 0);
    }

    void CambiarResolucion()
    {
        Resolution resolucionSeleccionada = resoluciones[resolucionIndex];
        Debug.Log("Cambiando a resolucion: " + resolucionSeleccionada.width + "x" + resolucionSeleccionada.height);

        Screen.SetResolution(resolucionSeleccionada.width, resolucionSeleccionada.height, Screen.fullScreen);

        PlayerPrefs.SetInt("numeroResolucion", resolucionIndex);
        PlayerPrefs.Save();

        ActualizarTextoResolucion();
    }

    void ActualizarTextoResolucion()
    {
        if (resolucionText != null)
        {
            Resolution resolucionActual = resoluciones[resolucionIndex];
            resolucionText.text = "RESOLUTION: " + resolucionActual.width + "x" + resolucionActual.height;
        }
    }

    void ActualizarSeleccion()
    {
        string[] opciones = { "Volume", "Bright", "Sensitivity", "Full Screen", "Resolution", "Back (ESC)" };
        TextMeshProUGUI[] textos = { volumenTexto, brilloTexto, sensibilidadTexto, pantallaCompletaText, resolucionText, volverText };

        for (int i = 0; i < textos.Length; i++)
        {
            if (i == selectedIndex)
            {
                textos[i].text = $"> {opciones[i]} <";
                // Si ya hay una corutina corriendo, la detenemos antes de iniciar otra
                if (blinkCoroutine != null)
                {
                    StopCoroutine(blinkCoroutine);
                }
                blinkCoroutine = StartCoroutine(ParpadearTexto(textos[i]));
            }
            else
            {
                textos[i].text = opciones[i];
                // Si la opción deja de estar seleccionada, aseguramos que el texto esté visible
                textos[i].enabled = true;
            }
        }
    }
    IEnumerator ParpadearTexto(TextMeshProUGUI texto)
    {
        while (true)
        {
            texto.enabled = !texto.enabled;
            yield return new WaitForSeconds(0.5f);  // Tiempo de parpadeo
        }
    }
}
