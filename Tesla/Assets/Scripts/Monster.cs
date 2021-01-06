﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
   
    [SerializeField]
    [Range(2, 8)]
    private float maxSpeed;
    [SerializeField]
    [Range(0, 5)]
    private float dmgSpeed;

    public Transform despawnPoint;
    [SerializeField]
    private Transform target;
    private Player targetScript;

    private NavMeshAgent agent;
    private Animator animator;

    private bool hasToChase = true;

    [SerializeField]
    private float recoveryTime;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        targetScript = target.gameObject.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasToChase) Chase();
    }

    private void Chase()
    {
        agent.SetDestination(target.position);
    }

    private void GetHit() //cambia vel, ejecuta animación de hit
    {
        SoundManager.PlaySound(SoundManager.Sound.EnemyHitted, transform.position);
        // SoundManager.PlaySound(SoundManager.Sound.LightOn);
        // Descomenta para sentir en tus carnes como dos audios se reproducen a la vez


        //Stun
        agent.isStopped = true;

        //Quitar stun a los x secs
        Invoke("StartWalking", 2);

        //Cambia vel
        agent.speed = dmgSpeed;

        //anim dmg

        //Se recupera al x tiempo
        StartCoroutine(Recover());
    }
    private IEnumerator Recover()
    {
        yield return new WaitForSeconds(recoveryTime);
        agent.speed = maxSpeed;
       
    }

    private void StartWalking() //Para después del stun o para cuando tenga que andar
    {
        agent.isStopped = false;
    }

    private void Atacar()
    {
        targetScript.GetHit();

        //animación atacar
    }

    private void Disappear()
    {
        hasToChase = false;
        transform.position = despawnPoint.position;
    }

    private void Appear() //Para que aparezca en los puntos q nos interesa
    {
        hasToChase = true;
    }

    public void RayTargetHit() //la función que llama la weapon
    {
        GetHit();
    }

}