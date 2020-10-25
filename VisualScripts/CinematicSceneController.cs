using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(AudioSource))]
public class CinematicSceneController : MonoBehaviour
{
    public bool palMenu;
    public GameObject[] vignets;
    public AudioClip[] soundsPerVignet;
    public AudioClip[] backTracks;
    public int[] framesDeCambio;
    internal int indexBackTrack;
    public float[] timePerVignet;
    public float[] delayPerSound;
    public Text PCTPC;
    public Text escText;
    public int inicioEsc;
    public int finEsc;
    public float speedDesFade;
    private AudioSource audSource;
    public AudioSource BackTrackAudSource;
    private int indexVignet;
    private Vector3 inicialPosiCamara;
    private bool moving = false;
    private float lerp = 0;
    private bool canChange;
    private bool beginDesFadeText;
    private bool changingBackTrack;


    // Use this for initialization
    void Start()
    {
        indexBackTrack = -1;
        changeBackTrack();
        audSource = GetComponent<AudioSource>();
        indexVignet = -1;
        moveToNextVignet();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (palMenu)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                SceneManager.LoadScene(2);
            }
        }

        if (moving)
        {
            lerp += Time.deltaTime;
            transform.position = Vector3.Lerp(inicialPosiCamara, vignets[indexVignet].transform.GetChild(0).position, lerp);
            if (lerp >= 1)
            {
                lerp = 0;
                moving = false;
                vignets[indexVignet].GetComponent<Animator>().SetBool("Play", true);
                Invoke("ActiveChangeVignet", timePerVignet[indexVignet]);
                Invoke("ReproduceSound", delayPerSound[indexVignet]);
            }
        }
        else
        {
            if (Input.anyKeyDown && canChange && !audSource.isPlaying)
            {
                moveToNextVignet();
            }
        }
        if (beginDesFadeText)
        {
            PCTPC.color = new Color(PCTPC.color.r, PCTPC.color.g, PCTPC.color.b, PCTPC.color.a + Time.deltaTime * speedDesFade);
            if (PCTPC.color.a >= 1)
                beginDesFadeText = false;
        }
        if (changingBackTrack)
        {
            BackTrackAudSource.volume -= Time.deltaTime;
            if (BackTrackAudSource.volume <= 0)
            {
                changeBackTrack();
            }
        }
    }
    private void ReproduceSound()
    {
        audSource.clip = soundsPerVignet[indexVignet];
        audSource.Play();
    }
    private void ActiveChangeVignet()
    {
        canChange = true;
        beginDesFadeText = true;
    }

    internal void moveToNextVignet()
    {
        PCTPC.color = new Color(PCTPC.color.r, PCTPC.color.g, PCTPC.color.b, 0);
        canChange = false;
        indexVignet++;
        if (indexVignet == vignets.Length)// si es la ultima viñeta
        {
            if (palMenu)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                SceneManager.LoadScene(2);
            }
        }
        if (indexBackTrack != backTracks.Length - 1)//cambio de backTrack
        {
            if (indexVignet == framesDeCambio[indexBackTrack])
            {
                changingBackTrack = true;
            }
        }
        if (vignets[indexVignet].GetComponent<Vignet>().popDialogos.Length != 0) // para popDialogos
        {
            foreach (PopDialogo popD in vignets[indexVignet].GetComponent<Vignet>().popDialogos)
            {
                StartCoroutine(popDialogoRutine(popD.tiempoDesdeInicioSCN, popD.dialogo));
            }
        }

        if (indexVignet == inicioEsc || indexVignet == finEsc)
        {
            escText.enabled = !escText.enabled;
        }
        inicialPosiCamara = transform.position;
        moving = true;
    }

    private void changeBackTrack()
    {
        changingBackTrack = false;
        indexBackTrack++;
        BackTrackAudSource.clip = backTracks[indexBackTrack];
        BackTrackAudSource.volume = 1;
        BackTrackAudSource.Play();
    }
    IEnumerator popDialogoRutine(float time, GameObject popDia)
    {
        yield return new WaitForSeconds(time);
        popDia.SetActive(true);
    }
}
