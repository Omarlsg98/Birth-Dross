using UnityEngine;
using System.Collections;

public class PotiVidaRegen : Consumible
{

    public float healthRestorePerCall;
    internal IEnumerator cura;

    internal override void EndEffect()
    {
        StopCoroutine(cura);
        base.EndEffect();
    }
    internal override void ConsumibleUse()
    {
        base.ConsumibleUse();
        cura = curacion();
        StartCoroutine(cura);
    }
    IEnumerator curacion()
    {
        for (;;)
        {
            Dross.recoverHealth(healthRestorePerCall);
            yield return new WaitForSeconds(0.5f);
        }
    }


}
