using UnityEngine;
using System.Collections;

public class PotiVida : Consumible
{
    public float healthRestore;

    internal override void EndEffect()
    {
        base.EndEffect();
    }
    internal override void ConsumibleUse()
    {
        base.ConsumibleUse();
        Dross.recoverHealth(healthRestore);
    }
}
