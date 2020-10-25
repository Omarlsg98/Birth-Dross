using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeB : MonoBehaviour
{
    public float velocidadFade = 2;
    public float velocidadDesFade = 2; // :v
    public float timeVisible;
    public float refreshTime;
    public Image imagen;
    internal IEnumerator rutina;

    internal void letsDoFade()
    {
        if (rutina != null)
            StopCoroutine(rutina);
        if (imagen != null)
            imagen = this.GetComponent<Image>();
        rutina = FadeRutine();
        StartCoroutine(rutina);
    }

    internal IEnumerator FadeRutine()
    {
        bool oleadaTerminada = true;
        float timeToWait = 0;
        bool permanecer = true;
        while (permanecer)
        {
            if (oleadaTerminada)
            {
                if (imagen.color.a < 0.5f)
                {
                    timeToWait = refreshTime;
                    Fade(false, timeToWait);
                }
                else
                {
                    timeToWait = timeVisible;
                    oleadaTerminada = false;
                }
            }
            else
            {
                if (imagen.color.a > 0)
                {
                    timeToWait = refreshTime;
                    Fade(true, timeToWait);
                }
                else
                {
                    oleadaTerminada = true;
                    permanecer = false;
                    timeToWait = 0;
                }
            }
            yield return new WaitForSeconds(timeToWait);
        }
    }
    internal void Fade(bool fade_desFade, float timeToWait)// true para desFade false para fade
    {
        if (fade_desFade)
        {
            imagen.color = new Color(imagen.color.r, imagen.color.g, imagen.color.b, imagen.color.a - timeToWait * velocidadFade);
        }
        else
        {
            imagen.color = new Color(imagen.color.r, imagen.color.g, imagen.color.b, imagen.color.a + timeToWait * velocidadDesFade);
        }
    }
}
