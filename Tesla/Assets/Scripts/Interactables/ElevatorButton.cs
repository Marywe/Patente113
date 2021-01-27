using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorButton : MonoBehaviour
{
    [SerializeField] GameObject armaPuesta;

    public void DejarArma()
    {
        armaPuesta.SetActive(true);
    }
}
