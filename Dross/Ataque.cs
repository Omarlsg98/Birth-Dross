using UnityEngine;
using System.Collections;
using System;
using UnityStandardAssets.Characters.FirstPerson;
using Random = UnityEngine.Random;
using UnityEngine.UI;

//Aqui utilizaria herencia , si tan solo el editor de unity soportara Herencia :CCC 
[System.Serializable]
public class Habilidad01
{
    //Para Escopeta
    public GameObject disparoPrefab;
    public float bulletSpeed;
    public float damage;
    public float coolDownTime;
    internal bool active = true;
    public float time;
    public AudioClip[] fireSound;
}

[System.Serializable]
public class Habilidad02
{
    // para Patada
    public float alcanceMele;
    internal bool active = true;
    public float coolDownTime;
    public float damage;
    public AudioClip[] fireSound;
    public float roboDeVidaPorGolpe;
}
[System.Serializable]
public class Habilidad11
{
    // para Decoy
    internal bool active = true;
    public float coolDownTime;
    public float damage;
    public GameObject Decoy;
    public float newWSpeed;
    public float newRSpeed;
    public float radio;
    public float timeToBoom;
    public AudioClip[] fireSound;
}
[System.Serializable]
public class Habilidad12
{
    // para Insulto en area Taunt
    internal bool active = true;
    public float coolDownTime;
    public float damage;
    public float radioEfecto;
    public AudioClip[] fireSound;
    public AudioClip[] fireSoundCharged;
    public float maxMultiplier;
    public float maxDamage;
    public float maxRadio;
    internal float timerInsult;
    public float timeToConsiderPress;

    //internal float damagemu;
}
[System.Serializable]
public class Habilidad21
{
    //Para torretas 
    internal bool[] active;
    public float coolDownTime;
    [Header("0 Oriam, 1 Namagem,2 Knil")]
    public GameObject[] torretasPrefab;
    internal byte indexTorreta = 0;
    public float[] precioTorreta;
    public float alturaTorreta = 1.1f;
    public AudioClip[] fireSound;
}
[System.Serializable]
public class Habilidad22
{
    // para banear Dinero
    public GameObject disparoPrefab;
    public float bulletSpeed;
    public float damage;
    public float coolDownTime;
    internal bool active = true;
    public float moneyPerShoot;
    public float time;
    public AudioClip[] fireSound;
}

public class Ataque : MonoBehaviour
{
    // Estadisticas habilidades
    [Header("Para Escopeta")]
    public Habilidad01 hab01;
    [Header("Para Martillazo")]
    public Habilidad02 hab02;
    [Header("Para Decoy")]
    public Habilidad11 hab11;
    [Header("Para Trolleada en Area")]
    public Habilidad12 hab12;
    [Header("Para Torretas")]
    public Habilidad21 hab21;
    [Header("Para banearDinero")]
    public Habilidad22 hab22;

    public Transform Disparador;
    private DrossBehaviour Dross;
    public AudioSource FireSource;
    [Range(0, 1)]
    public float probDialogDross;
    public AudioSource DialogSource;
    public Image MiraFranco;
    public Animator guiAnim;


    // Use this for initialization
    void Start()
    {
        hab21.active = new bool[hab21.torretasPrefab.Length];
        Dross = GetComponent<DrossBehaviour>();
        for (int i = 0; i < hab21.active.Length; i++)
        {
            hab21.active[i] = true;
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (Dross.curCharacter == 2)
        {
            if (Input.GetButtonDown("Turret"))
            {
                if (hab21.indexTorreta == 2)
                    hab21.indexTorreta = 0;
                else
                    hab21.indexTorreta++;
                Dross.setTorretaImage();
            }
            else if (Input.GetButtonDown("PutTorreta"))
            {
                PosicionarTorreta();
            }
            else
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    // scroll up
                    if (hab21.indexTorreta == 2)
                        hab21.indexTorreta = 0;
                    else
                        hab21.indexTorreta++;
                    Dross.setTorretaImage();
                }
                else if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {

                    // scroll down
                    if (hab21.indexTorreta == 0)
                        hab21.indexTorreta = 2;
                    else
                        hab21.indexTorreta--;
                    Dross.setTorretaImage();
                }
            }
        }
        if (GetComponent<FirstPersonController>().canMoveBody)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                switch (Dross.curCharacter)
                {
                    case 0:
                        if (hab01.active)
                        {
                            CastARangeAtackEscopeta(hab01.disparoPrefab, hab01.bulletSpeed, pcIncrement(hab01.damage, Dross.porcentOfDamageIncrement), hab01.time, 1); // Escopetazo
                            hab01.active = false;
                            StartCoroutine(CoolDownsRutine(hab01.coolDownTime, 1));
                            Dross.ArmaDerecha.SetBool("Attacking", true);
                            FireSource.PlayOneShot(hab01.fireSound[UnityEngine.Random.Range(0, hab01.fireSound.Length)]);
                            guiAnim.SetBool("Attacking", true);
                            if (Random.Range(0f, 1f) <= probDialogDross)
                            {
                                if (!DialogSource.isPlaying)
                                {
                                    DialogSource.clip = Dross.sonidosPersonajes[0].principalAttackSounds[Random.Range(0, Dross.sonidosPersonajes[0].principalAttackSounds.Length)];
                                    DialogSource.Play();
                                }
                            }
                        }
                        break;
                    case 1: // setear la carga
                        if (hab12.active)
                        {
                            hab12.timerInsult = hab12.timeToConsiderPress;
                        }
                        break;
                    case 2:
                        if (hab22.active)
                        {
                            CastARangeAtack(hab22.disparoPrefab, hab22.bulletSpeed, pcIncrement(hab22.damage, Dross.porcentOfDamageIncrement), hab22.time, 2);
                            hab22.active = false;
                            StartCoroutine(CoolDownsRutine(hab22.coolDownTime, 22));
                            Dross.ArmaDerecha.SetBool("Attacking", true);
                            guiAnim.SetBool("Attacking", true);
                            FireSource.PlayOneShot(hab22.fireSound[UnityEngine.Random.Range(0, hab22.fireSound.Length)]);
                            if (Random.Range(0f, 1f) <= probDialogDross)
                            {
                                if (!DialogSource.isPlaying)
                                {
                                    DialogSource.clip = Dross.sonidosPersonajes[2].principalAttackSounds[UnityEngine.Random.Range(0, Dross.sonidosPersonajes[2].principalAttackSounds.Length)];
                                    DialogSource.Play();
                                }
                            }
                        }
                        break;
                }

            }
            else if (Input.GetButtonDown("Fire2"))
            {
                switch (Dross.curCharacter)
                {
                    case 0:
                        // reproducir animacion
                        if (hab02.active)
                        {
                            MeleAtack(); // Martillazo
                            hab02.active = false;
                            StartCoroutine(CoolDownsRutine(hab02.coolDownTime, 2));
                            Dross.ArmaIzquierda.SetBool("Attacking", true);
                            FireSource.PlayOneShot(hab02.fireSound[UnityEngine.Random.Range(0, hab02.fireSound.Length)]);
                            if (Random.Range(0f, 1f) <= probDialogDross)
                            {
                                if (!DialogSource.isPlaying)
                                {
                                    DialogSource.clip = Dross.sonidosPersonajes[0].secundaryAttackSounds[UnityEngine.Random.Range(0, Dross.sonidosPersonajes[0].secundaryAttackSounds.Length)];
                                    DialogSource.Play();
                                }
                            }
                        }
                        break;
                    case 1:
                        if (hab11.active)
                        {
                            DecoyAtack(hab11.Decoy, pcIncrement(hab11.damage, Dross.porcentOfDamageIncrement), hab11.radio, hab11.timeToBoom);
                            hab11.active = false;
                            StartCoroutine(CoolDownsRutine(hab11.coolDownTime, 11));
                            FireSource.PlayOneShot(hab11.fireSound[UnityEngine.Random.Range(0, hab11.fireSound.Length)]);
                            if (Random.Range(0f, 1f) <= 1)
                            {
                                if (!DialogSource.isPlaying)
                                {
                                    DialogSource.clip = Dross.sonidosPersonajes[1].secundaryAttackSounds[UnityEngine.Random.Range(0, Dross.sonidosPersonajes[1].secundaryAttackSounds.Length)];
                                    DialogSource.Play();
                                }
                            }
                        }
                        break;
                    case 2:
                        HacerZoom();
                        break;
                }

            }
            else if (Input.GetButtonUp("Fire1"))
            {
                switch (Dross.curCharacter)
                {

                    case 1:
                        if (hab12.active)
                        {
                            // esparcir particulas 
                            // reproducir audio :3
                            float realDamage = 0;
                            float realRadio = 0;
                            if (hab12.timerInsult < 0)
                            {
                                if (-hab12.timerInsult > hab12.maxMultiplier)
                                {
                                    hab12.timerInsult = -hab12.maxMultiplier;
                                }
                                realDamage = hab12.damage + (hab12.damage * (hab12.maxDamage * ((-hab12.timerInsult) / hab12.maxMultiplier)));
                                realRadio = hab12.radioEfecto + (hab12.radioEfecto * (hab12.maxRadio * ((-hab12.timerInsult) / hab12.maxMultiplier)));
                                FireSource.PlayOneShot(hab12.fireSoundCharged[UnityEngine.Random.Range(0, hab12.fireSoundCharged.Length)]);
                                Dross.ArmaDerecha.SetBool("HoldingAttack", false);
                                Dross.ArmaIzquierda.SetBool("HoldingAttack", false);
                            }
                            else
                            {
                                realDamage = hab12.damage;
                                realRadio = hab12.radioEfecto;
                                Dross.ArmaDerecha.SetBool("Attacking", true);
                                Dross.ArmaIzquierda.SetBool("Attacking", true);
                                FireSource.PlayOneShot(hab12.fireSound[UnityEngine.Random.Range(0, hab12.fireSound.Length)]);
                            }
                            AtaqueArea(transform.position, realRadio, pcIncrement(realDamage, Dross.porcentOfDamageIncrement), 3);
                            hab12.active = false;
                            StartCoroutine(CoolDownsRutine(hab12.coolDownTime, 12));

                            if (Random.Range(0f, 1f) <= probDialogDross)
                            {
                                if (!DialogSource.isPlaying)
                                {
                                    DialogSource.clip = Dross.sonidosPersonajes[1].principalAttackSounds[UnityEngine.Random.Range(0, Dross.sonidosPersonajes[1].principalAttackSounds.Length)];
                                    DialogSource.Play();
                                }
                            }
                        }
                        break;
                }
            }
            else if (Input.GetButton("Fire1"))
            {
                switch (Dross.curCharacter)
                {
                    case 1:
                        if (hab12.active)
                        {
                            hab12.timerInsult -= Time.deltaTime;
                            if (hab12.timerInsult < 0)
                            {
                                Dross.ArmaDerecha.SetBool("HoldingAttack", true);
                                Dross.ArmaIzquierda.SetBool("HoldingAttack", true);
                            }
                        }
                        break;
                }
            }

        }
    }

    internal void HacerZoom()
    {

        Master.camaraDross.fieldOfView = MiraFranco.enabled ? 60 : 20;
        Dross.ArmaDerecha.gameObject.GetComponent<SpriteRenderer>().enabled = MiraFranco.enabled;
        Dross.ArmaIzquierda.gameObject.GetComponent<SpriteRenderer>().enabled = MiraFranco.enabled;
        Dross.Mira.enabled = MiraFranco.enabled;
        MiraFranco.enabled = !MiraFranco.enabled;
        Dross.apuntando = MiraFranco.enabled;

    }

    internal void AtaqueArea(Vector3 position, float radioAlcance, float damage, byte atackType)
    {

        Collider[] enemies = Physics.OverlapSphere(position, radioAlcance);
        foreach (Collider enemy in enemies)
        {
            if (enemy.gameObject.transform.tag == "Enemy")
            {
                enemy.gameObject.GetComponent<Mortal>().getHit(damage, gameObject, atackType);
            }
        }
    }

    private void PosicionarTorreta()
    {
        switch (hab21.indexTorreta)
        {
            case 0:
                if (hab21.active[0])
                {
                    if (putTorreta(hab21.indexTorreta))
                    {
                        hab21.active[0] = false;
                        StartCoroutine(CoolDownsRutine(hab21.coolDownTime, 210));
                        FireSource.PlayOneShot(hab21.fireSound[UnityEngine.Random.Range(0, hab21.fireSound.Length)]);
                        if (Random.Range(0f, 1f) <= probDialogDross)
                        {
                            if (!DialogSource.isPlaying)
                            {
                                DialogSource.clip = Dross.sonidosPersonajes[2].secundaryAttackSounds[UnityEngine.Random.Range(0, Dross.sonidosPersonajes[2].secundaryAttackSounds.Length)];
                                DialogSource.Play();
                            }
                        }
                    }
                }
                break;
            case 1:
                if (hab21.active[1])
                {
                    if (putTorreta(hab21.indexTorreta))
                    {
                        hab21.active[1] = false;
                        StartCoroutine(CoolDownsRutine(hab21.coolDownTime, 211));
                        FireSource.PlayOneShot(hab21.fireSound[UnityEngine.Random.Range(0, hab21.fireSound.Length)]);
                        if (Random.Range(0f, 1f) <= probDialogDross)
                        {
                            if (!DialogSource.isPlaying)
                            {
                                DialogSource.clip = Dross.sonidosPersonajes[2].secundaryAttackSounds[UnityEngine.Random.Range(0, Dross.sonidosPersonajes[2].secundaryAttackSounds.Length)];
                                DialogSource.Play();
                            }
                        }
                    }
                }
                break;
            case 2:
                if (hab21.active[2])
                {
                    if (putTorreta(hab21.indexTorreta))
                    {
                        hab21.active[2] = false;
                        StartCoroutine(CoolDownsRutine(hab21.coolDownTime, 212));
                        FireSource.PlayOneShot(hab21.fireSound[UnityEngine.Random.Range(0, hab21.fireSound.Length)]);
                        if (Random.Range(0f, 1f) <= probDialogDross)
                        {
                            if (!DialogSource.isPlaying)
                            {
                                DialogSource.clip = Dross.sonidosPersonajes[2].secundaryAttackSounds[UnityEngine.Random.Range(0, Dross.sonidosPersonajes[2].secundaryAttackSounds.Length)];
                                DialogSource.Play();
                            }
                        }
                    }
                }
                break;
        }

    }

    private bool putTorreta(byte turret)
    {
        if (Master.payingMoney(hab21.precioTorreta[turret]))
        {
            GameObject torretaInstance = (GameObject)Instantiate(hab21.torretasPrefab[turret], new Vector3(transform.position.x,
                transform.position.y + hab21.alturaTorreta, transform.position.z), transform.rotation);
            torretaInstance.GetComponentInChildren<ShaderXDXD>().Spawn();
            return true;
        }
        else
        {
            Debug.Log("No hay dinero para comprar la torreta");
        }
        return false;
    }

    private void DecoyAtack(GameObject Decoy, float damage, float radio, float timeToBoom)
    {
        StartCoroutine(InvisibleDross(timeToBoom));
        GameObject decoyInstance = (GameObject)Instantiate(Decoy, transform.position, transform.rotation);
        decoyInstance.GetComponentInChildren<ShaderXDXD>().Spawn();
        decoyInstance.GetComponent<DecoyScript>().damage = damage;
        decoyInstance.GetComponent<DecoyScript>().radius = radio;
        decoyInstance.GetComponent<DecoyScript>().timeToBoom = timeToBoom;
        decoyInstance.GetComponent<DecoyScript>().Begin();
    }

    IEnumerator InvisibleDross(float timeToBoom)
    {
        Dross.ChangeSpeed(hab11.newWSpeed, hab11.newRSpeed);
        ChangeVisibilityDross(true);
        yield return new WaitForSeconds(timeToBoom);
        Dross.ChangeSpeed(Dross.WalkSpeed, Dross.RunSpeed);
        ChangeVisibilityDross(false);
    }

    internal void ChangeVisibilityDross(bool invisible)
    {
        //Dross.Arma.GetComponent<SpriteRenderer>().enabled = !invisible;// o cualquier otro metodo de desvanecido
    }

    private void MeleAtack()
    {
        Ray dirRay = Master.camaraDross.ScreenPointToRay(Input.mousePosition);
        RaycastHit deteRay;
        if (Physics.Raycast(dirRay, out deteRay, hab02.alcanceMele))
        {
            if (deteRay.transform.tag == "Enemy")
            {
                deteRay.transform.gameObject.GetComponent<Mortal>().getHit(pcIncrement(hab02.damage, Dross.porcentOfDamageIncrement), gameObject, 0);
                Dross.recoverHealth(hab02.roboDeVidaPorGolpe);
            }
        }
    }

    private void CastARangeAtack(GameObject disparoPrefab, float bulletSpeed, float damage, float time, byte atackType)
    {
        GameObject bala = (GameObject)Instantiate(disparoPrefab, Disparador.position, new Quaternion(0, 0, 0, 0));
        bala.GetComponent<Rigidbody>().AddForce(Master.camaraDross.transform.forward * bulletSpeed, ForceMode.Impulse);
        bala.GetComponent<OnBalazo>().SetBala(damage, "Enemy", gameObject, time, atackType);
    }
    private void CastARangeAtackEscopeta(GameObject disparoPrefab, float bulletSpeed, float damage, float time, byte atackType)
    {
        GameObject bala = (GameObject)Instantiate(disparoPrefab, Disparador.position, new Quaternion(0, 0, 0, 0));
        bala.GetComponent<OnBalazoEscopeta>().SetBala(damage, "Enemy", gameObject, time, atackType, bulletSpeed);
    }
    IEnumerator CoolDownsRutine(float time, byte powerPosition)
    {
        yield return new WaitForSeconds(time);
        switch (powerPosition)
        {
            case 1:
                hab01.active = true;
                break;
            case 2:
                hab02.active = true;
                break;
            case 11:
                hab11.active = true;
                break;
            case 12:
                hab12.active = true;
                break;
            case 210:
                hab21.active[0] = true;
                break;
            case 211:
                hab21.active[1] = true;
                break;
            case 212:
                hab21.active[2] = true;
                break;
            case 22:
                hab22.active = true;
                break;
        }
    }
    internal static float pcIncrement(float numero, float porcentaje)
    {
        return numero * porcentaje + numero;
    }
}
