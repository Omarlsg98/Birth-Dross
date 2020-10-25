using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class Bala
{
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float damage;
    public AudioClip[] attackSound;
}

public class KnilTurret : Torreta
{
    public byte BulletsToPack;
    internal byte ActualBullet;
    public Bala balaPack;
    public Bala balaCP;
    public AudioClip[] attackSoundBullet;

    internal override void dieProccessEspecifico()
    {

    }

    internal override void getHitMegaSpecific()
    {

    }

    internal override void SpawnMegaEspecifico()
    {
        ActualBullet = 1;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null && !target.GetComponent<Monster>().die)
        {
            if (!atacando)
            {

                if (ActualBullet == BulletsToPack)
                {
                    // bala 1 ( caldo De posho) 
                    disparoPrefab = balaPack.bulletPrefab;
                    bulletSpeed = balaPack.bulletSpeed;
                    damage = balaPack.damage;
                    ActualBullet = 1;
                    attackSoundBullet = balaPack.attackSound;
                }
                else
                {
                    disparoPrefab = balaCP.bulletPrefab;
                    bulletSpeed = balaCP.bulletSpeed;
                    damage = balaCP.damage;
                    ActualBullet++;
                    attackSoundBullet = balaPack.attackSound;
                }
                if (tryToAtack(gameObject, disparoPrefab, bulletSpeed, damage, time, attackSoundBullet))
                {
                    scriptRender.setAnimation(AnimationTypes.attack, false);
                }
            }
        }
    }
}
