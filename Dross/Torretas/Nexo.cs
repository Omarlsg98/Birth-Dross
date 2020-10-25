using UnityEngine;
using System.Collections;
using System;

public class Nexo : Mortal
{
    public Transform Disparador;
    public Transform RotadorDisparador;
    public float bulletSpeed;
    public GameObject disparoPrefab;
    internal bool atacando;
    internal GameObject target;
    public float time;
    public AudioClip[] attackSound;

    public GameObject Target
    {
        get
        {
            return target;
        }
        set
        {
            if (Target != null && !Target.GetComponent<Monster>().die)
            {
                if (Vector3.Distance(transform.position, Target.transform.position) >= alcanceMele)
                //  si antiguo target fuera de alcance
                {
                    target = value;
                }
            }
            else
            {
                target = value;
            }
        }
    }

    internal override void dieProcess()
    {
        Master.Dross.GetComponent<Mortal>().dieProcess();
    }

    internal override void getHitSpecific(GameObject attacker, byte AtackType)
    {
        // efecto rojo o algo que avise el golpe al nexo
        barraVida.gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("Attacking", true);
    }

    internal override void SpawnSpecifico()
    {
        target = null;
    }

    // Use this for initialization
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        TurretAtack(gameObject);
    }

    internal void TurretAtack(GameObject Attacker)
    {
        if (Target != null && !Target.GetComponent<Monster>().die)
        {
            // if (pointToEnemy())
            if (!atacando)
                tryToAtack(Attacker);
        }
    }

    private bool pointToEnemy()
    {
        RaycastHit deteRay;
        if (Physics.Raycast(Disparador.position, Disparador.forward, out deteRay, alcanceMele))
        {
            if (deteRay.transform.gameObject == target)
            {
                return true;
            }
        }
        return false;

    }
    //private void rotateDisparador()
    //{
    //    Vector3 dirToEnemy = target.transform.position - RotadorDisparador.position;
    //    RotadorDisparador.rotation = Quaternion.FromToRotation(RotadorDisparador.transform.forward, dirToEnemy.normalized);
    //}

    internal bool tryToAtack(GameObject Attacker)
    {
        if (Vector3.Distance(transform.position, Target.transform.position) < alcanceMele)
        {
            //range
            Vector3 dirToTarget = Target.transform.position - Disparador.position;
            GameObject bala = (GameObject)Instantiate(disparoPrefab, Disparador.position, new Quaternion(0, 0, 0, 0));
            bala.GetComponent<Rigidbody>().AddForce(dirToTarget.normalized * bulletSpeed, ForceMode.Impulse);
            bala.GetComponent<OnBalazo>().SetBala(damage, "Enemy", Attacker, time, 0);
            atacando = true;
            audSource.PlayOneShot(attackSound[UnityEngine.Random.Range(0, attackSound.Length)]);
            Invoke("ActivateAtack", coolDownAtackTime);
            return true;
        }
        return false;
    }
    internal bool tryToAtack(GameObject Attacker, GameObject bulletPrefab, float fireSpeed, float damageInc, float time, AudioClip[] attackSounds)
    {
        if (Vector3.Distance(transform.position, Target.transform.position) < alcanceMele)
        {
            //range
            Vector3 dirToTarget = Target.transform.position - Disparador.position;
            GameObject bala = (GameObject)Instantiate(bulletPrefab, Disparador.position, new Quaternion(0, 0, 0, 0));
            bala.GetComponent<Rigidbody>().AddForce(dirToTarget.normalized * fireSpeed, ForceMode.Impulse);
            bala.GetComponent<OnBalazo>().SetBala(damage, "Enemy", Attacker, time, 2);
            atacando = true;
            audSource.PlayOneShot(attackSounds[UnityEngine.Random.Range(0, attackSounds.Length)]);
            Invoke("ActivateAtack", coolDownAtackTime);
            return true;
        }
        return false;
    }
    internal void ActivateAtack()
    {
        atacando = false;
    }
}
