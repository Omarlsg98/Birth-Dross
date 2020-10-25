using UnityEngine;
using System.Collections;
using System;

public class NormalMonster : Monster
{
    internal override void SpecificDieProcess()
    {

    }

    // Use this for initialization
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        NormalMonsterBehaviour(false);
    }
}
