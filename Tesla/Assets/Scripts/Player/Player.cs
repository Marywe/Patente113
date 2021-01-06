using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    

    [SerializeField] private int maxLife;
    private int currentLife;

    [SerializeField] private Weapon weapon;
    private FirstPersonController firstPersonController;
    private Animator animator;
    private bool isClimbing;

    public  Vector3 initialPos;
    public  Quaternion rotation;
    private void Awake()
    {
        firstPersonController = gameObject.GetComponent<FirstPersonController>();
        animator = gameObject.GetComponent<Animator>();
        currentLife = maxLife;
    }


    public void GetHit()
    {
        //animación get hit
        //-vel, mirar como lo hacemos para q conecte con la speed del player controller
    }

    public void ClimbLadder() // Ya no funciona por tiempo, simplemente empieza a escalar al pulsar F y no para hasta Finishclimb()
    {
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

    public void InteractButton()
    {

    }

    public IEnumerator FinishClimb() //Cuando sale de la escalera (se llama dentro de ladder.cs) deja de escalar
    {
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

