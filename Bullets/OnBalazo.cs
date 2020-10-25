using UnityEngine;
using System.Collections;

public class OnBalazo : MonoBehaviour
{

    private float damage;
    private string tagTarget;
    private GameObject attacker;
    private float time;
    private byte atackType;

    public byte AtackType
    {
        get
        {
            return atackType;
        }

        set
        {
            atackType = value;
        }
    }

    public float Time
    {
        get
        {
            return time;
        }

        set
        {
            time = value;
        }
    }

    public GameObject Attacker
    {
        get
        {
            return attacker;
        }

        set
        {
            attacker = value;
        }
    }

    public string TagTarget
    {
        get
        {
            return tagTarget;
        }

        set
        {
            tagTarget = value;
        }
    }

    public float Damage
    {
        get
        {
            return damage;
        }

        set
        {
            damage = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, time);
    }
    // Update is called once per frame
    void Update()
    {

    }
    internal virtual void SetBala(float damage, string tagTarget, GameObject attacker, float time, byte atackType)
    {
        this.damage = damage;
        this.tagTarget = tagTarget;
        this.attacker = attacker;
        this.time = time;
        this.atackType = atackType;
    }

    void OnTriggerEnter(Collider obje)
    {
        if (obje.transform.tag == tagTarget)
        {
            obje.gameObject.GetComponent<Mortal>().getHit(damage, attacker, atackType);
            Destroy(gameObject);
        }
        else if (obje.transform.tag == "Escenario" || obje.transform.tag == "Nexo" || obje.transform.tag == "Turret")
        {
            Destroy(gameObject);
        }
    }
}
