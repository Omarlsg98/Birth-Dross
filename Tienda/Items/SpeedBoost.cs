using UnityEngine;
using System.Collections;
using System;

public class SpeedBoost : Consumible
{
    [Range(0f, 5f)]
    public float porcentualSpeedAument;

    internal override void EndEffect()
    {
        Dross.ChangeSpeed(Dross.WalkSpeed, Dross.RunSpeed);
        base.EndEffect();
    }
    internal override void ConsumibleUse()
    {
        base.ConsumibleUse();
        Dross.ChangeSpeed(Dross.WalkSpeed + (Dross.WalkSpeed * porcentualSpeedAument), Dross.RunSpeed + (Dross.RunSpeed * porcentualSpeedAument));
    }

}
