using UnityEngine;
using System.Collections;
using System;

public class MotherShip : Monster
{
    public GameObject[] Sons;

    internal override void SpecificDieProcess()
    {
        // hacer que salgan de a uno 
        Master.enemiesInScene.Remove(transform.parent.gameObject);
        foreach (GameObject monsterToSpawn in Sons)
        {
            GameObject monster = (GameObject)Instantiate(monsterToSpawn, transform.position, transform.rotation);
            Master.enemiesInScene.Add(monster);
            Master.enemiesInSceneCount++;
            monster.GetComponentInChildren<ShaderXDXD>().Spawn();
        }
        Invoke("Destroy", 1.7f);
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
