using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // lol no se mostrar textos por pantalla con el otro controlador, por lo demás, sigue siendo más flexible lo de que el otro se invoque en cualquier escena
    // no me cuentes tu vida

    public GameObject climbText;
    public GameObject rechargeText;

    public int triggerNum = 0;

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

    public Monster monster;   
    public void CheckTrigger()
    {
        ++triggerNum;

        switch (triggerNum)
        {
            case 1:
                StartCoroutine(StartHunt(true, true)); //laberinto
                break;

            case 2:
                StartCoroutine(StartHunt(false, false)); //sales del laberinto
                break;

            case 3:
                StartCoroutine(StartHunt(true, false)); //entras en la sala bífida
                break;
            case 4:
                StartCoroutine(StartHunt(false, false)); //sales de ella y desaparece
                break;

        }
    }

    private IEnumerator StartHunt(bool hasToChase, bool disToApp)
    {       
        yield return new WaitForSeconds(2);

        //open door (soniditos tal cual)
        monster.hasToChase = hasToChase;
        monster.disappearToAppear = disToApp;
        if (!hasToChase) monster.Disappear();
    }



}