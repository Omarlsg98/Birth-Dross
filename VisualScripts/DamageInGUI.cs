using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageInGUI : MonoBehaviour
{
    public Image otroCoso;

    internal void DamageInScreen(float actual, float max)
    {
        if (actual > max)
        {
            actual = max;
        }
        else if (actual <= 0)
        {
            actual = 0;
        }
        float cociente = actual / max;
        GetComponent<Image>().material.SetFloat("_Cutoff", cociente <= 0.08f ? 0.02f : cociente);
        otroCoso.color = new Color(otroCoso.color.r, otroCoso.color.g, otroCoso.color.b, 1 - cociente);
    }
}
