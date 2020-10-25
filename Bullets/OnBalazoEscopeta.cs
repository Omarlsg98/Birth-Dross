using UnityEngine;
using System.Collections;

public class OnBalazoEscopeta : OnBalazo
{

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, Time);
    }

    // Update is called once per frame
    void Update()
    {

    }
    internal  void SetBala(float damage, string tagTarget, GameObject attacker, float time, byte atackType,float bulletSpeed)
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<OnBalazo>().SetBala(damage, tagTarget, attacker, time, atackType);
            Vector3 dirFuerza = Master.camaraDross.transform.forward+Master.camaraDross.transform.right * Random.Range(-0.3f, 0.3f) + Master.camaraDross.transform.up * Random.Range(-0.25f, 0.25f);
            child.GetComponent<Rigidbody>().AddForce(dirFuerza.normalized * bulletSpeed, ForceMode.Impulse);
        }
        transform.DetachChildren();
    }
}
