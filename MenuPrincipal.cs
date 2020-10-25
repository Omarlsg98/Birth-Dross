using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Jugar()
    {
        SceneManager.LoadScene(1);
    }
    public void ReJugar()
    {
        SceneManager.LoadScene(2);
    }
    public void Salir()
    {
        Application.Quit();
    }
}
