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

    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject deathText;
    private Image screenMuerte;
    [SerializeField] private GameObject endText;


    public  Vector3 initialPos;
    public  Quaternion rotation;


    private void Awake()
    {
       
        firstPersonController = gameObject.GetComponent<FirstPersonController>();
        animator = gameObject.GetComponent<Animator>();
        screenMuerte = deathScreen.GetComponent<Image>();
        currentLife = maxLife;
        setBlood(0);
        StartCoroutine(spawnSound());
    }


    public void GetDamage()
    {
        StopCoroutine("playerRegen");

        --currentLife;
        SoundManager.PlaySound(SoundManager.Sound.PlayerGetHit);
        //SoundManager.PlaySound(SoundManager.Sound.EnemEncounter);

        StartCoroutine(playerRegen(secondsToRecoverLife));

        switch (currentLife) 
        {
            case 2: setBlood(128);
                firstPersonController.m_WalkSpeed = 6f;
                break;

            case 1: setBlood(255, 170);
                firstPersonController.m_WalkSpeed = 4f;
                break;

            case 0:
                StartCoroutine(playerDeath());
                break;
        }
        //animación get hit
        //-vel, mirar como lo hacemos para q conecte con la speed del player controller
    }

    public RawImage bloodUI;
    void setBlood(byte alpha, byte red = 135)
    {
        bloodUI.color = new Color32(red, 0, 0, alpha);
    }

    void stopPlayer()
    {
        firstPersonController.m_WalkSpeed = 0;
        firstPersonController.canMove = false;
        weapon.canShoot = false;
    }

    private IEnumerator playerDeath()
    {
        stopPlayer();

        animator.SetTrigger("ceMurio");

        screenMuerte.color = new Color32(0, 0, 0, 0);
        deathScreen.SetActive(true);
        deathText.SetActive(false);

        yield return new WaitForSeconds(0.4f);
        screenMuerte.color = new Color32(0, 0, 0, 63);
        yield return new WaitForSeconds(0.4f);
        screenMuerte.color = new Color32(0, 0, 0, 91);
        yield return new WaitForSeconds(0.4f);
        screenMuerte.color = new Color32(0, 0, 0, 127);
        yield return new WaitForSeconds(0.4f);
        screenMuerte.color = new Color32(0, 0, 0, 255);
        yield return new WaitForSeconds(0.2f);
        deathText.SetActive(true);

        yield return new WaitForSeconds(3.0f);
        GameAssets.instance.ReloadScene();
    }

    private IEnumerator End()
    {
        yield return new WaitForSeconds(5.0f);
        stopPlayer();

        screenMuerte.color = new Color32(0, 0, 0, 0);
        deathScreen.SetActive(true);
        deathText.SetActive(false);

        yield return new WaitForSeconds(0.4f);
        screenMuerte.color = new Color32(0, 0, 0, 63);
        yield return new WaitForSeconds(0.4f);
        screenMuerte.color = new Color32(0, 0, 0, 91);
        yield return new WaitForSeconds(0.4f);
        screenMuerte.color = new Color32(0, 0, 0, 127);
        yield return new WaitForSeconds(0.4f);
        screenMuerte.color = new Color32(0, 0, 0, 255);
        yield return new WaitForSeconds(0.2f);
        endText.SetActive(true);

        yield return new WaitForSeconds(5.0f);
        GameAssets.instance.LoadMenu();
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

    private IEnumerator spawnSound()
    {
        yield return new WaitForSeconds(0f);
        SoundManager.PlaySound(SoundManager.Sound.Spawn, transform.position);
        StopCoroutine(spawnSound());
    }

    //bool unaVez = false; // Solo sirve para probar recibir daño
    public void ClimbLadder() // Ya no funciona por tiempo, simplemente empieza a escalar al pulsar F y no para hasta Finishclimb()
    {
        //if (!unaVez) // Solo sirve para probar recibir daño
        //{
        //    GetDamage();
        //    unaVez = true;
        //}

        firstPersonController.finishingClimbing = false;
        firstPersonController.climbing = true;
        firstPersonController.canMove = false;
    }
    
    //bool otraVez = false; // Solo sirve para probar recibir daño
    public IEnumerator FinishClimb() //Cuando sale de la escalera (se llama dentro de ladder.cs) deja de escalar
    {
        //if (!otraVez)// Solo sirve para probar recibir daño
        //{
        //    GetDamage();
        //    otraVez = true;
        //}

        firstPersonController.finishingClimbing = true;

        yield return new WaitForSeconds(0.2f);
        firstPersonController.canMove = true;
        firstPersonController.climbing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CogerArma")
        {  
            Destroy(other.gameObject);
            weapon.Activate();
            weapon.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Stop();
        }

        if (other.tag == "AbrirAscensor")
        {
            Door d = other.GetComponent<Door>();
            ElevatorButton b = other.GetComponent<ElevatorButton>();
            weapon.Deactivate();
            b.DejarArma();
            d.Open();
        }

        if(other.tag == "End")
        {
            StartCoroutine(End());
        }
    }

}


#region Botón
#if UNITY_EDITOR
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
#endif
#endregion

