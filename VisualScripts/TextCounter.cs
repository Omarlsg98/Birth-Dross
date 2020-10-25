using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TextCounter : MonoBehaviour
{

    internal float TimetoCount;
    private Text txt;
    public Text txtFather;
    //cambiar de color dependiendo del tiempo :3
    void Start()
    {
        txt = GetComponent<Text>();
        if (txtFather == null)
            txtFather = GetComponentInParent<Text>();
    }
    internal void StartToCount(float time)
    {
        TimetoCount = time;
        StartCoroutine(CountRutine());
    }

    private IEnumerator CountRutine()
    {
        txt.enabled = true;
        txtFather.enabled = true;
        while (TimetoCount > 0)
        {
            TimetoCount -= Time.deltaTime;
            txt.text = "" + (int)TimetoCount;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        txt.enabled = false;
        txtFather.enabled = false;
    }
}
