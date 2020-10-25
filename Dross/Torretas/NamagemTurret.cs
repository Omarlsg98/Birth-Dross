using UnityEngine;
using System.Collections;
using System;

public class NamagemTurret : Torreta
{
    private GameObject curBullet = null;
    public float maxSize = 1;
    [Tooltip("Con respecto a el scale en X")]
    public int particionDeTiempo = 10;

    internal override void getHitMegaSpecific()
    {

    }

    internal override void SpawnMegaEspecifico()
    {
        atacando = true;
        SpawnNewBullet();
        Invoke("ActivateAtack", coolDownAtackTime);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {
            if (pointToEnemy())
                if (!atacando)
                    tryToAtack();
        }
    }
    private bool pointToEnemy()
    {
        Vector3 dirToTarget = Vector3.Normalize(Target.transform.position - transform.position);
        float angle = Mathf.Atan2(dirToTarget.x, dirToTarget.z) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1f);
        transform.rotation = Quaternion.Slerp(transform.parent.rotation, rotation, 1f);
        RaycastHit deteRay;

        if (Physics.Raycast(Disparador.position, Disparador.forward, out deteRay, alcanceMele))
        {
            if (deteRay.transform.gameObject == target)
            {
                return true;
            }
        }
        return false;

    }
    IEnumerator LoadAtackrutine()
    {
        float inicialScale = curBullet.transform.localScale.x;
        while (maxSize - curBullet.transform.localScale.x > 0)
        {
            curBullet.transform.localScale += Vector3.one * ((maxSize - inicialScale) / particionDeTiempo);
            yield return new WaitForSeconds(coolDownAtackTime / particionDeTiempo);
        }
    }

    private void SpawnNewBullet()
    {
        //StopCoroutine((LoadAtackrutine()));
        curBullet = (GameObject)Instantiate(disparoPrefab, Disparador.position, new Quaternion(0, 0, 0, 0));
        StartCoroutine(LoadAtackrutine());
    }

    internal void tryToAtack()
    {
        if (Vector3.Distance(transform.position, Target.transform.position) < alcanceMele)
        {
            //range
            Vector3 dirToTarget = Target.transform.position - Disparador.position;
            curBullet.GetComponent<Rigidbody>().AddForce(dirToTarget.normalized * bulletSpeed, ForceMode.Impulse);
            curBullet.GetComponent<OnBalazo>().SetBala(damage, "Enemy", gameObject, time, 1);
            curBullet.GetComponent<OnBalazoSostenido>().SeDispara();
            atacando = true;
            audSource.PlayOneShot(attackSound[UnityEngine.Random.Range(0, attackSound.Length)]);
            Invoke("ActivateAtack", coolDownAtackTime);
            SpawnNewBullet();
        }
    }

    internal override void dieProccessEspecifico()
    {
        Destroy(curBullet);
    }
}
