using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuNavigation : MonoBehaviour
{
    public Canvas menuPrincipalCanvas; // Menú principal
    public Canvas menuOpcionesCanvas; // Menú de opciones
    public Canvas canvasOpciones; // Canvas para configuraciones
    public List<TextMeshProUGUI> menuOptions; // Opciones del menú
    private int selectedIndex = 0; // Índice de la opción seleccionada
    private bool enMenuPrincipal = true; // Estado del menú principal
    private Coroutine blinkCoroutine; //parpadeo de opción
    public GameObject fadeIn;
    public AudioSource audioPc;

    void Start()
    {
        fadeIn.SetActive(false);
        menuOpcionesCanvas.gameObject.SetActive(false); // Ocultar menú de opciones al inicio
        menuPrincipalCanvas.gameObject.SetActive(true); // Mostrar menú principal
        canvasOpciones.gameObject.SetActive(false); // Ocultar menú de configuración
    }

    void Update()
    {
        // Solo permite la navegación del menú de opciones si está activo
        if (menuOpcionesCanvas.gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                selectedIndex = (selectedIndex + 1) % menuOptions.Count;
                UpdateMenuSelection();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                selectedIndex = (selectedIndex - 1 + menuOptions.Count) % menuOptions.Count;
                UpdateMenuSelection();
            }
            else if (Input.GetKeyDown(KeyCode.Return)) // Enter para seleccionar
            {
                EjecutarOpcionSeleccionada();
            }
        }
        else if (enMenuPrincipal)
        {
            // Solo activa el menú de opciones si está en el menú principal
            if (Input.anyKeyDown)
            {
                ActivarMenuOpciones();
            }
        }
    }


    void ActivarMenuOpciones()
    {
        menuPrincipalCanvas.gameObject.SetActive(false); // Oculta menú principal
        menuOpcionesCanvas.gameObject.SetActive(true);  // Muestra menú de opciones
        enMenuPrincipal = false; // Cambia estado
        UpdateMenuSelection();
    }

    void UpdateMenuSelection()
    {
        for (int i = 0; i < menuOptions.Count; i++)
        {
            if (i == selectedIndex)
            {
                menuOptions[i].text = $"> {menuOptions[i].text.Replace("> ", "").Replace(" <", "")} <";

                // Detenemos cualquier corutina previa antes de iniciar una nueva
                if (blinkCoroutine != null)
                {
                    StopCoroutine(blinkCoroutine);
                }
                blinkCoroutine = StartCoroutine(ParpadearTexto(menuOptions[i]));
            }
            else
            {
                menuOptions[i].text = menuOptions[i].text.Replace("> ", "").Replace(" <", "");

                // Asegurar que el texto vuelva a aparecer inmediatamente
                menuOptions[i].enabled = true;
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

    void EjecutarOpcionSeleccionada()
    {
        if (selectedIndex == 0) // Opción "Jugar"
        {
            fadeIn.SetActive(true);
            audioPc.Stop();
            StartCoroutine(CargarEscenaConRetraso("Intro"));
        }
        else if (selectedIndex == 1) // Opción "Opciones"
        {
            canvasOpciones.gameObject.SetActive(!canvasOpciones.gameObject.activeSelf); // Alternar visibilidad del canvas
            menuOpcionesCanvas.gameObject.SetActive(!menuOpcionesCanvas.gameObject.activeSelf); // Alternar visibilidad del canvas
        }
        else if (selectedIndex == 2) // Opción "Salir"
        {
            Application.Quit(); // Cierra la aplicación
            Debug.Log("saliste");
        }
    }

    private IEnumerator CargarEscenaConRetraso(string nombreEscena)
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(nombreEscena);
    }
}
