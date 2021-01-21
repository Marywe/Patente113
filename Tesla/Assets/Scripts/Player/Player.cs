using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;



public class Player : MonoBehaviour
{

    //Vida(2 toques) : cuando le pegan -vel, 
    //viñeta rgba(255,0,0,0) => (255, 0, 0, 100)
    //Vel max, velHit -50%


    //Acciones:
    //Subir arma(el arma se mueve, pistola.subirarma)
    //Hit(animacion dañado, sprite ui, velHit, cameraShake)
    //Subir escalera(subir arma, animacion subir escalera, desactiva controles hasta que estes arriba)
    //Interactuar con el botón ascensor


   [SerializeField] private int maxLife = 3;
    private int currentLife = 0;
    [SerializeField] private int secondsToRecoverLife;

    [SerializeField] private Weapon weapon;
    private FirstPersonController firstPersonController;
    private Animator animator;
    private bool isClimbing;

    [SerializeField] private GameObject deathScreen;
    

    public  Vector3 initialPos;
    public  Quaternion rotation;
    private void Awake()
    {
       
        firstPersonController = gameObject.GetComponent<FirstPersonController>();
        animator = gameObject.GetComponent<Animator>();
        currentLife = maxLife;
        setBlood(0);
    }


    public void GetHit()
    {
        --currentLife;
        StartCoroutine(playerRegen(secondsToRecoverLife));

        switch (currentLife) 
        {
            case 2: setBlood(128);
                firstPersonController.m_WalkSpeed = 6f;
                break;

            case 1: setBlood(255, 210);
                firstPersonController.m_WalkSpeed = 4f;
                break;

            case 0:
                deathScreen.SetActive(true);
                StartCoroutine(playerDeath());
                break;
        }

        
        //animación get hit
        //-vel, mirar como lo hacemos para q conecte con la speed del player controller
    }

    public RawImage bloodUI;


    void setBlood(byte alpha, byte red = 175)
    {
        bloodUI.color = new Color32(red, 0, 0, alpha);

    }

    private IEnumerator playerDeath()
    {

        yield return new WaitForSeconds(3.0f);
        GameAssets.instance.ReloadScene();
    }
    private IEnumerator playerRegen(int secs)
    {
        if (currentLife < 2)
        {
            yield return new WaitForSeconds(secs - 5);
            ++currentLife;
            setBlood(128);
            firstPersonController.m_WalkSpeed = 6f;
        }

        yield return new WaitForSeconds(secs);
        ++currentLife;
        setBlood(0);
        firstPersonController.m_WalkSpeed = 8f;
        if (currentLife >= maxLife) StopCoroutine(playerRegen(secs));
    }

    //bool unaVez = false; // Solo sirve para probar recibir daño
    public void ClimbLadder() // Ya no funciona por tiempo, simplemente empieza a escalar al pulsar F y no para hasta Finishclimb()
    {
        //if (!unaVez) // Solo sirve para probar recibir daño
        //{
        //    GetHit();
        //    unaVez = true;
        //}
       
        //Debug.Log("bhwe");
        //StartCoroutine(firstPersonController.ActivateClimbing());
        firstPersonController.finishingClimbing = false;
        firstPersonController.climbing = true;
        firstPersonController.canMove = false;
        // firstPersonController.ActivateClimbing1();

        //StartCoroutine(FinishClimb());
        //animator.SetBool("isClimbing", true);

        //animation

        //subir arma
    }
    
    //bool otraVez = false; // Solo sirve para probar recibir daño
    public IEnumerator FinishClimb() //Cuando sale de la escalera (se llama dentro de ladder.cs) deja de escalar
    {
        //if (!otraVez)// Solo sirve para probar recibir daño
        //{
        //    GetHit();
        //    otraVez = true;
        //}
        //animator.applyRootMotion = false;

        firstPersonController.finishingClimbing = true;

        yield return new WaitForSeconds(0.2f);
        firstPersonController.canMove = true;
        firstPersonController.climbing = false;

        //firstPersonController.ActivateClimbing1();


        //firstPersonController.canMove = true;


        //poner arma guay

        //animator.SetBool("isClimbing", false);
        //animator.applyRootMotion = true;
    }

    public void InteractButton()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CogerArma")
        {

            Destroy(other.gameObject);
            weapon.Activate();
            weapon.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Stop();
        }
    }

}

#region Botón //Desactivar cuando se saque build
[CustomEditor(typeof(Player))]
public class MyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Player p = (Player)target;
        if (GUILayout.Button("Poner al PJ en su posición inicial"))
        {
            p.transform.position = p.initialPos;
            p.transform.rotation = p.rotation;
        }
    }

}
#endregion

