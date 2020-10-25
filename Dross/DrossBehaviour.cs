using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

[System.Serializable]
public class AudiosPersonaje
{
    public AudioClip[] getHitSounds;
    public AudioClip[] muerteSounds;
    public AudioClip[] principalAttackSounds;
    public AudioClip[] secundaryAttackSounds;
}


[RequireComponent(typeof(AudioSource))]
public class DrossBehaviour : Mortal
{
    //extensiones ocultadas
    [NonSerialized]
    internal new bool disparo_Mele;
    [NonSerialized]
    internal new float damage;
    [NonSerialized]
    internal new float alcanceMele;
    [NonSerialized]
    internal new float coolDownAtackTime;
    public float SwicthCoolDown = 2;
    private bool canSwitch = true;
    [Header("Velocidades Dross")]
    public float WalkSpeed;
    public float RunSpeed;
    [Header("Cosas de visualizacion")]
    public DamageInGUI damageGui;
    internal byte curCharacter = 0;
    public Image[] ImagenPerfil;
    public Image ImagenTorreta;
    [Header("0.Frontal, 1.Izquierda, 2.Back, 3.Derecha")]
    public FadeB[] Damage;
    public Image Mira;
    [Header("0 Dross,1 Troll,2 Fuyito")]
    public Animator ArmaIzquierda;
    public Animator ArmaDerecha;
    public Sprite[] ImagenesPerfil;
    public Sprite[] Miras;
    public AudiosPersonaje[] sonidosPersonajes;
    [Header("0 Oriam , 1 Namagem, 2 Knil")]
    public Sprite[] ImagenesTorretas;
    internal float m_RunSpeed;
    internal float m_WalkSpeed;
    // stats iniciales
    internal float inicialArmor;
    internal float porcentOfDamageIncrement;
    private bool soundEnable;
    internal bool inTienda = false;
    [Header("Visualizacion Tienda")]
    public GameObject[] tiendas;
    internal int actualTiendaIndex = -1;
    public GameObject AvisoTienda;
    public float radioDetecTienda;
    internal bool apuntando;

    internal override void SpawnSpecifico()
    {
        porcentOfDamageIncrement = 0;
        inicialArmor = armor;
        m_RunSpeed = RunSpeed;
        m_WalkSpeed = WalkSpeed;
        curCharacter = 0;
        setPersonaje();
        soundEnable = true;
        damageGui.DamageInScreen(1, 1);
    }

    // Use this for initialization
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (canSwitch)
        {
            if (Input.GetButtonDown("Character Change Forward"))
            {
                cambioDePersonaje(true);
            }
            else if (Input.GetButtonDown("Character Change Backward"))
            {
                cambioDePersonaje(false);
            }
        }
        if (Input.GetButtonDown("EntrarTienda"))
        {
            if (!inTienda)
            {
                if (actualTiendaIndex != -1)
                    tiendas[actualTiendaIndex].GetComponent<TiendaDatos>().setActualTienda();
            }
            else
            {
                Master.tienda.EnterExitTienda(false);
            }
        }
        DetectNearTienda();

    }

    private void DetectNearTienda()
    {
        int i = 0;
        foreach (GameObject tienda in tiendas)
        {
            if (Vector3.Distance(tienda.transform.position, transform.position) <= radioDetecTienda)
            {
                AvisoTienda.SetActive(true);
                actualTiendaIndex = i;
                return;
            }
            i++;
        }
        AvisoTienda.SetActive(false);
        actualTiendaIndex = -1;
    }

    private void cambioDePersonaje(bool atrasAdelante) // adelante true __ atras false
    {
        if (curCharacter == 2)
            ImagenTorretaActive(false);

        if (atrasAdelante)
        {
            if (curCharacter == 1)
            {
                ImagenTorretaActive(true);
                setTorretaImage();
            }
            if (curCharacter == 2)
                curCharacter = 0;
            else
                curCharacter++;

        }
        else
        {
            if (curCharacter == 0)
            {
                curCharacter = 2;
                ImagenTorretaActive(true);
                setTorretaImage();
            }
            else
                curCharacter--;

        }
        setPersonaje();
        StartCoroutine(CoolDownsRutine());
    }

    private void ImagenTorretaActive(bool active)
    {
        //hacerle un fade mejor ? 
        ImagenTorreta.gameObject.transform.parent.gameObject.SetActive(active);
        ImagenTorreta.enabled = active;
    }

    internal void setTorretaImage()
    {
        ImagenTorreta.sprite = ImagenesTorretas[GetComponent<Ataque>().hab21.indexTorreta];
    }

    private void setPersonaje()
    {
        //animacion parte grafica cambio de personaje
        Mira.sprite = Miras[curCharacter];
        ArmaDerecha.SetInteger("CurPersonaje", curCharacter);
        ArmaIzquierda.SetInteger("CurPersonaje", curCharacter);
        ImagenPerfil[0].sprite = ImagenesPerfil[curCharacter];
        ImagenPerfil[1].sprite = ImagenesPerfil[curCharacter - 1 < 0 ? 2 : curCharacter - 1];
        ImagenPerfil[2].sprite = ImagenesPerfil[curCharacter + 1 > 2 ? 0 : curCharacter + 1];
        Invoke("cancelCarga", 0.3f);
        //GetComponent<Ataque>().HacerZoom();
        if (apuntando)
        {
            GetComponent<Ataque>().HacerZoom();
        }
    }
    void cancelCarga()
    {
        ArmaDerecha.SetBool("HoldingAttack", false);
        ArmaIzquierda.SetBool("HoldingAttack", false);
    }

    internal override void dieProcess()
    {

        Invoke("goGameOver", 1);
    }

    private void goGameOver()
    {
        SceneManager.LoadScene(3);
    }

    internal override void getHitSpecific(GameObject attacker, byte atackType)
    {
        Vector3 dirToMonster = Vector3.Normalize(attacker.transform.position - transform.position);
        float angle = ShaderXDXD.AngleBetweenVector2(transform.forward, dirToMonster);
        byte index = 0;
        if (angle < 0f)
            angle += 360;

        if (angle <= 45)
            index = 0; // frontal
        else if (angle <= 135)
            index = 3; // izquierda
        else if (angle <= 225)
            index = 2; //atras
        else if (angle <= 315)
            index = 1; // Derecha
        else if (angle > 315)
            index = 0;
        Damage[index].letsDoFade();
    }
    internal void ChangeSpeed(float newWSpeed, float newRSpeed)
    {
        m_WalkSpeed = newWSpeed;
        m_RunSpeed = newRSpeed;
    }
    IEnumerator CoolDownsRutine()
    {
        canSwitch = false;
        yield return new WaitForSeconds(SwicthCoolDown);
        canSwitch = true;
    }
    internal override void getHit(float damage, GameObject attacker, byte atackType)
    {
        getHitSpecific(attacker, atackType);
        damage = damage * UnityEngine.Random.Range(0.6f, 1.3f);
        curHP -= (damage - (armor * damage));
        if (barraVida != null)
            barraVida.modBarra(maxHP, curHP);
        if (damageGui != null)
            damageGui.DamageInScreen(curHP, maxHP);
        if (curHP <= 0)
        {
            dieProcess();
            audSource.PlayOneShot(sonidosPersonajes[curCharacter].muerteSounds[UnityEngine.Random.Range(0, sonidosPersonajes[curCharacter].muerteSounds.Length)]);
        }
        else
        {
            if (soundEnable && Random.Range(0f, 1f) < 0.5f && !GetComponent<Ataque>().DialogSource.isPlaying)
            {
                int index = Random.Range(0, sonidosPersonajes[curCharacter].getHitSounds.Length);
                GetComponent<Ataque>().DialogSource.PlayOneShot(sonidosPersonajes[curCharacter].getHitSounds[index]);
                soundEnable = false;
                Invoke("EnableSound", sonidosPersonajes[curCharacter].getHitSounds[index].length);
            }
        }
    }
    internal void recoverHealth(float healthRestored)
    {
        curHP += healthRestored;
        if (curHP > maxHP)
        {
            curHP = maxHP;
        }
        barraVida.modBarra(maxHP, curHP);
        damageGui.DamageInScreen(curHP, maxHP);
    }
    internal void recoverHealthPorcentage(float porcentHeal)
    {
        curHP += maxHP * porcentHeal;
        if (curHP > maxHP)
        {
            curHP = maxHP;
        }
        barraVida.modBarra(maxHP, curHP);
        damageGui.DamageInScreen(curHP, maxHP);
    }
    void EnableSound()
    {
        soundEnable = true;
    }

}
