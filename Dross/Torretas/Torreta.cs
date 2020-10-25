using UnityEngine;
using System.Collections;
using System;

public abstract class Torreta : Nexo
{
    public float lifeTime;
    [Range(0, 0.5f)]
    public float refreshLifeTime;
    private float curLifeTime;
    public float detectionRange;
    public BarraScript BarraTiempo;

    internal override void dieProcess()
    {
        dieProccessEspecifico();
        Destroy(gameObject);
        Destroy(transform.parent.gameObject);
    }



    internal override void getHitSpecific(GameObject attacker, byte atackType)
    {
        getHitMegaSpecific();
    }

    internal override void SpawnSpecifico()
    {
        SpawnMegaEspecifico();
        disparo_Mele = false;
        target = null;
        transform.tag = "Turret";
        StartCoroutine(lifeTimeManager());
        StartCoroutine(TargetSelectionRutine());
    }

    IEnumerator lifeTimeManager()
    {
        curLifeTime = lifeTime;
        for (;;)
        {
            yield return new WaitForSeconds(refreshLifeTime);
            curLifeTime -= refreshLifeTime;
            BarraTiempo.modBarra(lifeTime, curLifeTime);
            if (curLifeTime <= 0)
            {
                dieProcess();
            }
        }
    }
    internal abstract void SpawnMegaEspecifico();
    internal abstract void getHitMegaSpecific();
    internal abstract void dieProccessEspecifico();

    internal void TargetSelection()
    {
        Collider[] posibleEnemies = Physics.OverlapSphere(transform.position, alcanceMele, 1 << 8); // 1<<8 detecta solo la capa 8 .-.( enemies)
        if (posibleEnemies.Length > 0)
        {
            float min = alcanceMele;// comienza en el mayor valor posible
            byte indexOfNearest = 0;
            for (int i = 0; i < posibleEnemies.Length; i++)
            {
                if (posibleEnemies[i].transform.tag == "Enemy")
                {
                    float distancia = Vector3.Distance(transform.position, posibleEnemies[i].transform.position);
                    if (distancia < min)
                    {
                        min = distancia;
                        indexOfNearest = (byte)i;
                    }
                }
            }
            target = posibleEnemies[indexOfNearest].gameObject;
        }
    }
    IEnumerator EmergencyTargetSelectionRutine()
    {
        for (;;)
        {
            if (target == null || target.GetComponent<Monster>().die)
                TargetSelection();
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator TargetSelectionRutine()
    {
        StartCoroutine(EmergencyTargetSelectionRutine());
        for (;;)
        {
            yield return new WaitForSeconds(1f);
            TargetSelection();
        }
    }
}
