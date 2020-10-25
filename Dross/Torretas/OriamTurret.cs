using UnityEngine;
using System.Collections;
using System;

public class OriamTurret : Torreta
{
    internal override void dieProccessEspecifico()
    {

    }

    internal override void getHitMegaSpecific()
    {

    }

    internal override void SpawnMegaEspecifico()
    {

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null && !Target.GetComponent<Monster>().die)
        {
            // if (pointToEnemy())
            if (!atacando)
                if (tryToAtack(gameObject))
                {
                    scriptRender.setAnimation(AnimationTypes.attack, false);
                }
        }
    }


}
