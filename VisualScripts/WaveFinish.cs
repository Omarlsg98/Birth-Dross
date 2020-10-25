using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveFinish : MonoBehaviour
{
    private bool oleadaTerminada;
    private Text txt;
    public float velocidadFade = 2;
    public float velocidadDesFade = 2; // :v
    public AudioSource audioR;
    internal AudioClip Clip;
    public float timeVisible;
    [Range(0, 0.1f)]
    public float refreshTime;

    void Start()
    {
        txt = this.GetComponent<Text>();

        if (audioR == null)
            audioR = GetComponent<AudioSource>();

    }

    internal IEnumerator FadeRutine(string TextToDisplay, AudioClip ClipToReproduce)
    {
        txt.text = TextToDisplay;
        Clip = ClipToReproduce;
        oleadaTerminada = true;
        float timeToWait = 0;
        bool permanecer = true;
        while (permanecer)
        {
            if (oleadaTerminada)
            {
                if (!audioR.isPlaying)
                    audioR.PlayOneShot(Clip);
                if (txt.color.a < 1)
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
                if (txt.color.a > 0)
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
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, txt.color.a - timeToWait * velocidadFade);
        }
        else
        {
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, txt.color.a + timeToWait * velocidadDesFade);
        }
    }
}
