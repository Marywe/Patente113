using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    [SerializeField]
    private GameObject LoadingScreen;
    [SerializeField]
    private Slider loadingPorcentaje; // O esto o un texto que ponga el porcentaje de carga
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame(string Scene)
    {
        Debug.Log("Cambiando a la escena " + Scene); //Para que en la consola de unity aparezca si se esta realizando
        StartCoroutine(OperationAsync(Scene));

    }
    //corrutina que creamos para la pantalla durante la carga de escenas, a modo de transicion.
    IEnumerator OperationAsync(string Scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(Scene);
        LoadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            //float progress = Mathf.Clamp01(operation.progress / 0.9f);
            //loadingPorcentaje.value = progress;
            yield return null;
        }
    }

    public void ExitGame()
    {
        Debug.Log("Saliendo del juego");
        Application.Quit();
    }
}
