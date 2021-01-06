using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // lol no se mostrar textos por pantalla con el otro controlador, por lo demás, sigue siendo más flexible lo de que el otro se invoque en cualquier escena

    public GameObject climbText;
    public GameObject rechargeText;
   
    public static GameManager instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }

        else
        {
            instance = this;

        }
    }
}