using UnityEngine;
using System.Collections;

public class DamageBoost : Consumible
{
    [Range(0f, 5f)]
    public float porcentualDamageAument;

    internal override void EndEffect()
    {
        Dross.porcentOfDamageIncrement = 0;
        base.EndEffect();
    }
    internal override void ConsumibleUse()
    {
        base.ConsumibleUse();
        Dross.porcentOfDamageIncrement = porcentualDamageAument;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
