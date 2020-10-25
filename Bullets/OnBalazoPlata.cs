using UnityEngine;
using System.Collections;

public class OnBalazoPlata : OnBalazo
{
    public AudioClip coinSound;

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, Time);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider obje)
    {
        if (obje.transform.tag == TagTarget)
        {
            obje.gameObject.GetComponent<Mortal>().getHit(Damage, Attacker, AtackType);
            Master.AddMoney(Master.Dross.GetComponent<Ataque>().hab22.moneyPerShoot);
            Master.coin.SetBool("Attacking", true);
            Master.coin.gameObject.GetComponent<AudioSource>().PlayOneShot(coinSound);
            Destroy(gameObject);
        }
        else if (obje.transform.tag == "Escenario" || obje.transform.tag == "Nexo")
        {
            Destroy(gameObject);
        }
    }
}
