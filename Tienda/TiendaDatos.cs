using UnityEngine;
using System.Collections;

public class TiendaDatos : MonoBehaviour
{
    public string nameShop;
    public GameObject[] itemsToSell;

    void Start()
    {

    }

    void Update()
    {

    }
    internal void setActualTienda()
    {
        Master.tienda.SetTienda(itemsToSell, nameShop);
    }
}
