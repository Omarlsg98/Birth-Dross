using UnityEngine;
using System.Collections;
using System;

public class DecoyScript : Mortal
{

    internal float timeToBoom;
    internal float radius;
    public GameObject Explosion;
    public AudioClip explosionSound;

    internal void Begin()
    {
        maxHP = Master.originalDross.GetComponent<Mortal>().maxHP;
        armor = Master.originalDross.GetComponent<Mortal>().armor;
        Master.Dross = gameObject;
        StartCoroutine(Explotar());
    }

    IEnumerator Explotar()
    {
        yield return new WaitForSeconds(timeToBoom - 0.4f);
        Explosion.SetActive(true);
        audSource.clip = explosionSound;
        audSource.Play();
        yield return new WaitForSeconds(0.4f);
        ExplotarMethod();
    }

    private void ExplotarMethod()
    {
        // animacion
        Master.originalDross.GetComponent<Ataque>().AtaqueArea(transform.position, radius, damage, 4);
        Master.Dross = Master.originalDross;
        Destroy(gameObject);
    }

    internal override void getHitSpecific(GameObject attacker, byte atackType)
    {

    }

    internal override void dieProcess()
    {
        ExplotarMethod();
    }

    internal override void SpawnSpecifico()
    {
        curHP = Master.originalDross.GetComponent<Mortal>().curHP;
    }
}
