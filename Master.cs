using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;

[Serializable]
public class Convoy
{
    public GameObject[] monsters;
    public int NExtraMonsters;
    public GameObject[] posibleExtraMonsters;
    public ConvoyDificultad dificultad = ConvoyDificultad.noob;
}
[Serializable]
public class Wave
{

    public int numOfSpawnsActive;
    public int numOfConvoys;
    public float spawnTimeConvoys;
    public float spawnTime;
    [Header("Para probabilidad de convoys :3")]
    [Header("Suma igual a 1")]
    [Range(0, 1)]
    public float probNoob;
    [Range(0, 1)]
    public float probFacil;
    [Range(0, 1)]
    public float probNormal;
    [Range(0, 1)]
    public float probNormalDificil;
    [Range(0, 1)]
    public float probDificil;
    [Range(0, 1)]
    public float probMuyDificil;
    [Range(0, 1)]
    public float probDios;

}
public enum ConvoyDificultad
{
    noob = 0,
    facil = 1,
    normal = 2,
    normal_dificil = 3,
    dificil = 4,
    muy_Dificil = 5,
    dios = 6
}

[Serializable]
public class Spawner
{
    public GameObject Spawn;
    public bool Izq_Derecha; // derecha true izquierda false
}
public class ConvoyMaster
{
    internal List<Convoy> convoyesNoob = new List<Convoy>();
    internal List<Convoy> convoyesFacil = new List<Convoy>();
    internal List<Convoy> convoyesNormal = new List<Convoy>();
    internal List<Convoy> convoyesNormalDificil = new List<Convoy>();
    internal List<Convoy> convoyesDificil = new List<Convoy>();
    internal List<Convoy> convoyesMuyDificil = new List<Convoy>();
    internal List<Convoy> convoyesDios = new List<Convoy>();

    public void ClearAll()
    {
        convoyesNoob.Clear();
        convoyesFacil.Clear();
        convoyesNormal.Clear();
        convoyesNormalDificil.Clear();
        convoyesDificil.Clear();
        convoyesMuyDificil.Clear();
        convoyesDios.Clear();
    }
}


public class Master : MonoBehaviour
{
    // las variables estaticas tal ves generan problemas en el reloadScene :// 
    // tal ves por el metodo find GameObje
    public static GameObject Dross;
    public static GameObject originalDross;
    public static Camera camaraDross;
    public static GameObject nexo;
    public static float DrossMoney;
    public static Text MoneyTxt;
    public static TiendasScript tienda;
    //MosnterSpawn System
    internal static bool enWave;
    internal static List<GameObject> enemiesInScene = new List<GameObject>();
    internal static int enemiesInSceneCount;
    [Header("Informacion Oleadas")]
    public Wave[] waves;
    public float timeBetweenWaves;
    internal Wave CurWave;
    [Header("Informacion Monstruos y Spawns")]
    public Spawner[] Spawners;
    private List<GameObject> actualSpawners = new List<GameObject>();
    public Convoy[] convoyes;
    internal ConvoyMaster convoyMaster = new ConvoyMaster();
    [Header("Parte Visual")]
    public WaveFinish waveF;
    public TextCounter txtCount;
    public AudioClip finishWaveClip;
    public AudioClip beginWaveClip;
    public Text curWaveDisplay;
    public GameObject MenuPausa;
    //Money
    [Header("Luka")]
    public Text MoneyVisualizer;
    public float InicialDrossMoney;
    [Header("Fog")]
    public float onStartFog;
    public float onEndFog;
    public float fogDensity;
    [Header("Cambio rapido de wave")]
    private bool waveAdelantada = false;
    private float curTimeSinceFWave;
    private float forwardTime;
    [Header("Music")]
    private AudioSource audSource;
    public AudioClip[] canciones;
    internal int curindexWave = 0;
    private bool spawnear;
    internal static Animator coin;
    public Animator coinN;
    public static GameObject soundLayer1;


    void Awake()
    {
        originalDross = GameObject.Find("Dross");
        soundLayer1 = GameObject.Find("soundLayer1");
        Dross = originalDross;
        camaraDross = GameObject.Find("CamaraDross").GetComponent<Camera>();
        nexo = GameObject.Find("Nexo");
        tienda = GameObject.Find("Tienda").GetComponent<TiendasScript>();
        DrossMoney = InicialDrossMoney;
        MoneyTxt = MoneyVisualizer;
        audSource = GetComponent<AudioSource>();
        coin = coinN;
    }
    void Start()
    {
        //RenderSettings.fogDensity = fogDensity;
        ClasificarConvoys();
        waveAdelantada = false;
        enemiesInSceneCount = 0;
        enemiesInScene.Clear();
        refreshMoneyState();
        actualSpawners.Clear();
        enWave = false;
        StartCoroutine(WaveRutine());
        StartCoroutine(BackTrackRutine());
    }

    private void ClasificarConvoys()
    {
        convoyMaster.ClearAll();
        foreach (Convoy conv in convoyes)
        {
            switch (conv.dificultad)
            {
                case ConvoyDificultad.noob:
                    convoyMaster.convoyesNoob.Add(conv);
                    break;
                case ConvoyDificultad.facil:
                    convoyMaster.convoyesFacil.Add(conv);
                    break;
                case ConvoyDificultad.normal:
                    convoyMaster.convoyesNormal.Add(conv);
                    break;
                case ConvoyDificultad.normal_dificil:
                    convoyMaster.convoyesNormalDificil.Add(conv);
                    break;
                case ConvoyDificultad.dificil:
                    convoyMaster.convoyesDificil.Add(conv);
                    break;
                case ConvoyDificultad.muy_Dificil:
                    convoyMaster.convoyesMuyDificil.Add(conv);
                    break;
                case ConvoyDificultad.dios:
                    convoyMaster.convoyesDios.Add(conv);
                    break;
            }
        }
    }

    private IEnumerator BackTrackRutine()
    {

        for (;;)
        {
            int index = Random.Range(0, canciones.Length);
            audSource.clip = canciones[index];
            audSource.Play();
            yield return new WaitForSeconds(canciones[index].length);
        }
    }

    public void ExitGame()
    {

        Application.Quit();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pausa(!MenuPausa.activeInHierarchy);
            if (Dross.GetComponent<DrossBehaviour>().inTienda)
                tienda.EnterExitTienda(false);
        }
        if (!enWave)
        {
            curTimeSinceFWave += Time.deltaTime;
            if (Input.GetButtonDown("StartWave"))
            {
                forwardTime = timeBetweenWaves - curTimeSinceFWave;
                waveAdelantada = true;
                txtCount.TimetoCount = 0;
                WaveBeginController();
            }
        }
        //  print(enemiesInSceneCount);
    }

    public void Pausa(bool DesPausarPausar)
    {
        Time.timeScale = DesPausarPausar ? 0 : 1;
        GetComponent<AudioSource>().volume = DesPausarPausar ? 0.4f : 0.8f;
        GetComponent<AudioSource>().pitch = DesPausarPausar ? 0.8f : 1f;
        Dross.GetComponent<FirstPersonController>().m_MouseLook.SetCursorLock(!DesPausarPausar);
        Dross.GetComponent<FirstPersonController>().canMoveBody = !DesPausarPausar;
        Dross.GetComponent<FirstPersonController>().canMoveCamara = !DesPausarPausar;
        MenuPausa.SetActive(DesPausarPausar);
    }

    IEnumerator WaveRutine()
    {
        StartCoroutine(SpawnRutine());
        foreach (Wave wave in waves)
        {
            if (waveAdelantada)
            {
                waveAdelantada = false;
                forwardTime = 0;
            }
            else
            {
                WaveBeginController();
            }
            if (spawnear)
            {
                yield return new WaitForSeconds(8f);
            }
            // spawnear = false;
            while (enWave && !waveAdelantada)
            {
                if (enemiesInSceneCount > 0) //si hay enemigos
                {
                    yield return new WaitForSeconds(2f); // verificar de nuevo en un segundo
                }
                else
                {
                    // si no hay enemigos se acabo la wave esperar tiempo entre Waves
                    WaveFinish();
                    yield return new WaitForSeconds(timeBetweenWaves);
                }
            }
        }
        finalWave();
    }

    private void WaveBeginController()
    {
        WaveBegin();
        spawnear = true;
    }

    private void finalWave()
    {
        //spawnear el boss :3 y sus subditos 
        SceneManager.LoadScene(4);
    }
    private void WaveBegin()
    {
        enWave = true;
        CurWave = waves[curindexWave];
        SpawnsActivation(CurWave.numOfSpawnsActive);
        curindexWave++;
        curWaveDisplay.text = curindexWave.ToString();
        StartCoroutine(waveF.FadeRutine(" Nueva Oleada", beginWaveClip));
    }
    private void WaveFinish()
    {

        for (int i = 0; i < enemiesInScene.Count; i++)
        {
            enemiesInScene[i].GetComponentInChildren<Monster>().Destroy();
        }
        enemiesInScene.Clear();
        enWave = false;
        SpawnsDesActivation();
        txtCount.StartToCount(timeBetweenWaves - 3); // chamboneado, arreglra esto .-.
        StartCoroutine(waveF.FadeRutine("Oleada Terminada", finishWaveClip));
    }

    private void SpawnsDesActivation()
    {
        foreach (Spawner spawn in Spawners)
        {
            GameObject spawner = spawn.Spawn;
            spawner.SetActive(false);
        }
        actualSpawners.Clear();
    }

    IEnumerator SpawnRutine()
    {
        for (;;)
        {
            if (spawnear)
            {
                for (int j = 0; j < CurWave.numOfConvoys; j++)
                {
                    int indexSpawn = Random.Range(0, actualSpawners.Count);
                    Convoy actualConvoy = SeleccionarConvoy();
                    for (int i = 0; i < (actualConvoy.monsters.Length + actualConvoy.NExtraMonsters); i++)
                    {
                        if (i != actualConvoy.monsters.Length + actualConvoy.NExtraMonsters - 1)
                        {
                            if (i < actualConvoy.monsters.Length) // para spawn monstruos basicos
                            {
                                spawnMonster(actualConvoy.monsters[i], indexSpawn);
                            }
                            else // para spawnear el componente aleatorio
                            {
                                int indxPXMonster = Random.Range(0, actualConvoy.posibleExtraMonsters.Length);
                                spawnMonster(actualConvoy.posibleExtraMonsters[indxPXMonster], indexSpawn);
                            }
                            yield return new WaitForSeconds(CurWave.spawnTime + (CurWave.spawnTime * Random.Range(-0.35f, 0.4f)));
                        }
                        else
                        {
                            yield return new WaitForSeconds(CurWave.spawnTimeConvoys + (CurWave.spawnTimeConvoys * Random.Range(-0.35f, 0.4f))); // Cuando se acaba el convoy
                        }
                    }
                }
                spawnear = false;
            }
            yield return new WaitForSeconds(8f); // esperando a comenzar a volver a spawnear
        }
    }

    private Convoy SeleccionarConvoy()
    {
        while (true)
        {
            float randNumber = Random.Range(0f, 1f);
            if (randNumber <= CurWave.probNoob)
            {
                return convoyMaster.convoyesNoob[Random.Range(0, convoyMaster.convoyesNoob.Count)];
            }
            else if (randNumber <= CurWave.probNoob + CurWave.probFacil)
            {
                return convoyMaster.convoyesFacil[Random.Range(0, convoyMaster.convoyesFacil.Count)];
            }
            else if (randNumber <= CurWave.probNoob + CurWave.probFacil + CurWave.probNormal)
            {
                return convoyMaster.convoyesNormal[Random.Range(0, convoyMaster.convoyesNormal.Count)];
            }
            else if (randNumber <= CurWave.probNoob + CurWave.probFacil + CurWave.probNormal + CurWave.probNormalDificil)
            {
                return convoyMaster.convoyesNormalDificil[Random.Range(0, convoyMaster.convoyesNormalDificil.Count)];
            }
            else if (randNumber <= CurWave.probNoob + CurWave.probFacil + CurWave.probNormal + CurWave.probNormalDificil + CurWave.probDificil)
            {
                return convoyMaster.convoyesDificil[Random.Range(0, convoyMaster.convoyesDificil.Count)];
            }
            else if (randNumber <= CurWave.probNoob + CurWave.probFacil + CurWave.probNormal + CurWave.probNormalDificil + CurWave.probDificil + CurWave.probMuyDificil)
            {
                return convoyMaster.convoyesMuyDificil[Random.Range(0, convoyMaster.convoyesMuyDificil.Count)];
            }
            else if (randNumber <= CurWave.probNoob + CurWave.probFacil + CurWave.probNormal + CurWave.probNormalDificil + CurWave.probDificil + CurWave.probMuyDificil + CurWave.probDios)
            {
                return convoyMaster.convoyesDios[Random.Range(0, convoyMaster.convoyesDios.Count)];
            }
        }
    }

    private void spawnMonster(GameObject monsterToSpawn, int indexSpawn)  // valores de 0 100 y que sean rangos para ver qeu monstruo se spawnea
    {
        GameObject monster = (GameObject)Instantiate(monsterToSpawn, actualSpawners[indexSpawn].transform.position, actualSpawners[indexSpawn].transform.rotation);
        enemiesInScene.Add(monster);
        enemiesInSceneCount++;
        monster.GetComponentInChildren<ShaderXDXD>().Spawn();
    }
    // utilizar las corutinas con referencias ( variables)
    private void SpawnsActivation(int numberofSpawnsActive)
    {
        if (numberofSpawnsActive == Spawners.Length)
        {
            foreach (Spawner spawn in Spawners)
            {
                GameObject spawner = spawn.Spawn;
                actualSpawners.Add(spawner);
            }
        }
        else
        {
            List<GameObject> intermLeft = new List<GameObject>();
            List<GameObject> intermRigth = new List<GameObject>();
            foreach (Spawner spawn in Spawners)
            {
                GameObject spawner = spawn.Spawn;
                if (spawn.Izq_Derecha)
                {
                    intermRigth.Add(spawner);
                }
                else
                {
                    intermLeft.Add(spawner);
                }
            }
            for (int i = 0; i < numberofSpawnsActive; i++)
            {
                if (i % 2 == 0)
                {
                    byte indexSpawnAdded = (byte)Random.Range(0, intermRigth.Count);
                    actualSpawners.Add(intermRigth[indexSpawnAdded]);
                    intermRigth.RemoveAt(indexSpawnAdded);
                }
                else
                {
                    byte indexSpawnAdded = (byte)Random.Range(0, intermLeft.Count);
                    actualSpawners.Add(intermLeft[indexSpawnAdded]);
                    intermLeft.RemoveAt(indexSpawnAdded);
                }
            }
        }
        foreach (GameObject spawner in actualSpawners)
        {
            spawner.SetActive(true);
        }

    }
    internal static bool payingMoney(float moneyToPay)
    {
        if (moneyToPay > DrossMoney)
        {
            return false;
        }
        else
        {
            DrossMoney -= moneyToPay;
            refreshMoneyState();
            return true;
        }
    }
    internal static void AddMoney(float moneyGet)
    {
        DrossMoney += moneyGet;
        refreshMoneyState();
    }
    internal static void refreshMoneyState()
    {
        MoneyTxt.text = "" + DrossMoney;
    }
}
