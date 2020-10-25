using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Monster : Mortal
{
    public AudioClip[] Dialogos;
    public AudioClip[] attackSound;
    internal bool die;
    internal Vector3 curWayPoint;
    internal NavMeshAgent navAgent;
    public float radioToTryAtack;
    public GameObject particulas;
    internal bool siguiendoADross;
    internal bool atacando;
    internal bool atacandoNexo;
    public float speed;
    public float speedWhileAttacking = 0.1f;
    public Transform Disparador;
    [Header("Para Unidades Range")]
    public GameObject disparoPrefab;
    public float bulletSpeed;
    internal bool atacarTorre;
    internal GameObject curTorre;
    public float time;
    [Header("Para Critics")]
    [Range(0f, 1f)]
    public float criticChance;
    public GameObject inCriticStateVis;
    internal bool inCriticState = false;
    [Range(0f, 1f)]
    public float probabilidadDialogo = 0.1f;

    internal override void dieProcess()
    {
        barraVida.gameObject.transform.parent.gameObject.SetActive(false);
        Master.enemiesInSceneCount--;
        inCriticStateVis.SetActive(false);
        die = true;
        particulas.SetActive(true);
        gameObject.layer = 0;
        GetComponent<Collider>().enabled = false;
        navAgent.speed = 0;
        scriptRender.setAnimation(AnimationTypes.die, false);
        SpecificDieProcess();

    }
    internal void Destroy()
    {
        Destroy(transform.parent.gameObject);
    }

    internal abstract void SpecificDieProcess();

    internal override void SpawnSpecifico()
    {
        gameObject.layer = 8;
        curWayPoint = Master.nexo.transform.position;
    }

    internal void NormalMonsterBehaviour(bool stopWhileAttaking)
    {
        ReproducirDialogo();
        FollowDrossOnPresenceDetection();
        NormalAttackBehaviour(stopWhileAttaking);
        curWayPoint.y = transform.position.y;
        navAgent.SetDestination(curWayPoint);
    }
    internal void ReproducirDialogo()
    {
        if (!die)
            if (!audSource.isPlaying)
            {
                if (UnityEngine.Random.Range(0f, 1f) <= probabilidadDialogo)
                    audSource.clip = Dialogos[UnityEngine.Random.Range(0, Dialogos.Length)];
                audSource.Play();
            }
    }

    internal void NormalAttackBehaviour(bool stopWhileAttaking)
    {
        if (!die)
        {
            if (atacando)
            {
                if (!stopWhileAttaking)
                    navAgent.speed = speedWhileAttacking;
                else
                    if (!siguiendoADross)
                    navAgent.speed = speedWhileAttacking;
            }
            else
            {
                navAgent.speed = speed;
                if (siguiendoADross)
                {
                    curWayPoint = Master.Dross.transform.position;
                    tryToAtack(Master.Dross.transform.position, "Player");
                }
                else
                {
                    if (atacarTorre)
                    {
                        if (curTorre != null)
                        {
                            curWayPoint = curTorre.transform.position;
                            tryToAtack(curTorre.transform.position, "Turret");
                        }
                        else
                        {
                            atacarTorre = false;
                            curWayPoint = Master.nexo.transform.position;
                        }
                    }
                    else
                    {
                        curWayPoint = Master.nexo.transform.position;
                        tryToAtack(Master.nexo.transform.position, "Nexo");
                    }
                }
            }
        }
    }

    internal void FollowDrossOnPresenceDetection()
    {
        if (!atacandoNexo)
        {
            if (Vector3.Distance(transform.position, Master.Dross.transform.position) <= radioToTryAtack && !atacarTorre)
            {
                if (Vector3.Distance(transform.position, Master.Dross.transform.position) <= Vector3.Distance(transform.position, Master.nexo.transform.position))
                {
                    siguiendoADross = true;
                }
            }
            else if (Vector3.Distance(transform.position, Master.Dross.transform.position) >= radioToTryAtack * 2.5f)
            {
                if (siguiendoADross)
                    siguiendoADross = false;
            }
        }
    }

    internal void tryToAtack(Vector3 Target, string tagTarget)
    {
        Vector3 dirToTarget = Target - Disparador.position;
        RaycastHit deteRay;
        if (Physics.Raycast(Disparador.position, dirToTarget.normalized, out deteRay, alcanceMele))
        {
            if (deteRay.transform.tag == tagTarget)
            {
                if (Vector3.Distance(transform.position, Target) < alcanceMele)
                {

                    if (!disparo_Mele)
                    {
                        //range
                        GameObject bala = (GameObject)Instantiate(disparoPrefab, Disparador.position, new Quaternion(0, 0, 0, 0));
                        bala.GetComponent<Rigidbody>().AddForce(dirToTarget.normalized * bulletSpeed, ForceMode.Impulse);
                        bala.GetComponent<OnBalazo>().SetBala(damage, tagTarget, gameObject, time, 1);
                        atacando = true;
                        Invoke("ActivateAtack", coolDownAtackTime);
                    }
                    else
                    {
                        //mele
                        // reproducir animacion
                        deteRay.transform.gameObject.GetComponent<Mortal>().getHit(damage, gameObject, 0);
                        atacando = true;
                        Invoke("ActivateAtack", coolDownAtackTime);
                    }
                    if (tagTarget == "Nexo")
                    {
                        atacandoNexo = true;
                        Master.nexo.GetComponent<Nexo>().Target = gameObject;
                    }
                    else
                    {
                        if (Vector3.Distance(transform.position, Master.nexo.transform.position) <= Master.nexo.GetComponent<Mortal>().alcanceMele)
                        {
                            Master.nexo.GetComponent<Nexo>().Target = gameObject;
                        }
                    }
                    audSource.PlayOneShot(attackSound[UnityEngine.Random.Range(0, attackSound.Length)]);
                    playAtackAnimation();
                }
            }
        }
    }

    internal virtual void playAtackAnimation()
    {
        scriptRender.setAnimation(AnimationTypes.attack, false);
    }

    internal override void getHitSpecific(GameObject attacker, byte atackType)
    {
        if (attacker == Master.Dross)
        {
            siguiendoADross = true;
            atacandoNexo = false;
            if (inCriticState)
            {
                if (atackType == 0)
                {
                    dieProcess();
                }
            }
        }
        else if (attacker.transform.tag == "Turret" && !atacandoNexo && !siguiendoADross)
        {
            atacarTorre = true;
            curTorre = attacker;
        }
    }
    internal override void getHit(float damage, GameObject attacker, byte atackType)
    {
        getHitSpecific(attacker, atackType);
        damage = damage * UnityEngine.Random.Range(0.6f, 1.3f);
        if (!inCriticState)
        {
            if (UnityEngine.Random.Range(0f, 1f) <= criticChance)
            {
                damage *= UnityEngine.Random.Range(2f, 3f);
                inCriticState = true;
                inCriticStateVis.SetActive(true);
            }
        }
        curHP -= (damage - (armor * damage));
        if (barraVida != null)
            barraVida.modBarra(maxHP, curHP);
        if (!die)
        {
            if (curHP <= 0)
            {
                dieProcess();
                audSource.PlayOneShot(muerteSound[UnityEngine.Random.Range(0, muerteSound.Length)]);
            }
            else
            {
                audSource.PlayOneShot(getHitSound[UnityEngine.Random.Range(0, getHitSound.Length)]);
            }
        }
    }
    internal void ActivateAtack()
    {
        atacando = false;
    }
}
