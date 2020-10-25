using UnityEngine;
using System.Collections;
using System;

public class Stalker : Monster
{
    private bool canReproduce;

    internal override void SpecificDieProcess()
    {

    }

    // Use this for initialization
    void Start()
    {
        canReproduce = true;
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        NormalMonsterBehaviour(true);
    }
    internal override void playAtackAnimation()
    {
        if (canReproduce)
        {
            base.playAtackAnimation();
            canReproduce = false;
            Invoke("returnReproduce", 1f);
        }
    }
    void returnReproduce()
    {
        canReproduce = true;
    }
}
