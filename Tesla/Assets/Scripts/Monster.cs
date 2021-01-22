using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    //AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABBBBBBBBBBBBBBBBBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
    //AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABBAAAAAAAAAAAAAAAAABBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
    //AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABBAAAAAOAAAAAAAAOAAAAAABBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
    //AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABBAAAAAAAAAWWAAAAAAAAAABBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
    //AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABBBBBBBBBBBBBBBBBBBBBBBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
    //Ahora que tengo tu atención, nuevos sonidos que imagino que irán asociados al enemigo:
    // Romper cristal:                                                                                              SoundManager.PlaySound(SoundManager.Sound.BrokenGlass, transform.position);
    // Encontrarse al enemigo:                                                                                      SoundManager.PlaySound(SoundManager.Sound.EnemEncounter, transform.position);
    // Chirrido (este no es del enemigo pero meh, ya lo pongo aquí):                                                SoundManager.PlaySound(SoundManager.Sound.Screech, transform.position);
    // Perseguir al player (este se repite cada 7s, así que lo puedes poner en una función que vaya en el update):  SoundManager.PlaySound(SoundManager.Sound.ChasingPlayer, transform.position);
    // Si borras las position se reproducirán en 2D (se oirán iwal independientemente de la posición)


    //[SerializeField]
    //[Range(2, 8)]
    //private float maxSpeed;
    //[SerializeField]
    //[Range(0, 5)]
    //private float dmgSpeed;


    [SerializeField]
    private Transform target;
    private Player targetScript;

    private NavMeshAgent agent;
    private Animator animator;

    public bool hasToChase = false;

    [SerializeField]
    private float recoveryTime;

    [SerializeField]
    private Transform[] spawnZones;
    public Transform despawnPoint;


    public bool disappearToAppear=true;
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
        if (hasToChase && disappearToAppear) Chase();
        else if (hasToChase && !disappearToAppear)
        {
            //spawn en sitio
            if (GameManager.instance.triggerNum == 3)
            {
                transform.position = spawnZones[3].position;
            }
            
        }
        else if (!hasToChase && !disappearToAppear) { //pa que no haga nada
            
            disappearToAppear = true;
        }
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
        hasToChase = false;
        StartCoroutine(Disappear());

        //Cambia vel
        //agent.speed = dmgSpeed;

        //anim dmg

        //Se recupera al x tiempo
        //StartCoroutine(Recover());
    }
  
    private void Atacar()
    {
        targetScript.GetDamage();

        //animación atacar
    }

    public IEnumerator Disappear()
    {
        agent.isStopped = true;
        agent.enabled = false;
        yield return new WaitForSeconds(3);
        
        transform.position = despawnPoint.position;
        if (disappearToAppear)
        {
            yield return new WaitForSeconds(7);
            Appear();
        }
    }

    private void Appear() //Para que aparezca en los puntos q nos interesa
    {
        transform.position = spawnZones[Random.Range(0, spawnZones.Length -1)].position;
        agent.enabled = true;
        agent.isStopped = false;
        hasToChase = true;
    }

    public void RayTargetHit() //la función que llama la weapon
    {
        GetHit();
    }


    //Deprefukincapted
    private IEnumerator Recover()
    {
        yield return new WaitForSeconds(recoveryTime);
        //agent.speed = maxSpeed;
    }


}
