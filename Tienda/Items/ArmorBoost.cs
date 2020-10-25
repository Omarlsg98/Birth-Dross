using UnityEngine;
using System.Collections;

public class ArmorBoost : Consumible
{
    [Range(0f, 5f)]
    public float porcentualArmorAument;

    internal override void EndEffect()
    {
        Dross.armor = Dross.inicialArmor;
        base.EndEffect();
    }
    internal override void ConsumibleUse()
    {
        base.ConsumibleUse();
        Dross.armor += Dross.armor * porcentualArmorAument;
    }

}
