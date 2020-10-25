using UnityEngine;
using System.Collections;
using System;
[RequireComponent(typeof(AudioSource))]
public abstract class Mortal : MonoBehaviour
{

    public bool disparo_Mele; // disparo False, mele true
    public float maxHP;
    internal float curHP;
    public float damage;
    [Range(0, 1)]
    public float armor;
    public float alcanceMele;
    [Tooltip("Entre Menos Mejor")]
    public float coolDownAtackTime;
    public BarraScript barraVida;
    internal ShaderXDXD scriptRender;
    [Header("Sounds")]
    internal AudioSource audSource;
    public AudioClip[] muerteSound;
    public AudioClip[] getHitSound;


    internal void Spawn(ShaderXDXD render)
    {
        scriptRender = render;
        Spawn();
    }
    //spawn provisional
    internal void Spawn()
    {
        audSource = GetComponent<AudioSource>();
        curHP = maxHP;
        if (barraVida != null)
            barraVida.modBarra(maxHP, curHP);
        SpawnSpecifico();
    }

    internal virtual void getHit(float damage, GameObject attacker, byte atackType)
    {
        getHitSpecific(attacker, atackType);
        damage = damage * UnityEngine.Random.Range(0.6f, 1.3f);
        curHP -= (damage - (armor * damage));
        if (barraVida != null)
            barraVida.modBarra(maxHP, curHP);
        if (curHP <= 0)
        {
            dieProcess();
            if (muerteSound != null)
                audSource.PlayOneShot(muerteSound[UnityEngine.Random.Range(0, muerteSound.Length)]);
        }
        else
        {
            if (getHitSound != null)
            {
                audSource.PlayOneShot(getHitSound[UnityEngine.Random.Range(0, getHitSound.Length)]);

            }
        }
    }

    internal abstract void getHitSpecific(GameObject attacker, byte atackType);
    internal abstract void dieProcess();
    internal abstract void SpawnSpecifico();
}
