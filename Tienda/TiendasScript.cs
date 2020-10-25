using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using System;

public class TiendasScript : MonoBehaviour
{
    public GameObject[] ItemsSpaces;
    public Text[] nameSpaces;
    public Text[] priceSpaces;
    public Text titleText;
    public Text ActualPage;
    private int PageActual;
    internal GameObject[] ItemsOnSale;
    internal Inventario inventario;


    // Use this for initialization
    void Start()
    {
        inventario = Master.Dross.GetComponent<Inventario>();
    }

    // Update is called once per frame
    void Update()
    {
 
    }

    internal void EnterExitTienda(bool salirEntrar) // entrar es true
    {
        Master.Dross.GetComponent<DrossBehaviour>().inTienda = salirEntrar;
        Master.Dross.GetComponent<FirstPersonController>().m_MouseLook.SetCursorLock(!salirEntrar);
        Master.Dross.GetComponent<FirstPersonController>().canMoveBody = !salirEntrar;
        Master.Dross.GetComponent<FirstPersonController>().canMoveCamara = !salirEntrar;
        transform.GetChild(0).gameObject.SetActive(salirEntrar);
    }

    internal void SetTienda(GameObject[] Items, string nameShop)
    {
        EnterExitTienda(true);
        PageActual = 0;
        ActualPage.text = "" + (PageActual + 1);
        transform.GetChild(0).gameObject.SetActive(true);
        ItemsOnSale = Items;
        titleText.text = nameShop;
        loadPage();
    }
    internal void loadPage()
    {
        int index = PageActual * 3;
        for (int i = 0; i < ItemsSpaces.Length; i++)
        {
            if (index > ItemsOnSale.Length - 1)
            {
                ItemsSpaces[i].GetComponent<Image>().color = Color.clear;
                nameSpaces[i].text = "";
                priceSpaces[i].text = "";
                break;
            }
            ItemsSpaces[i].GetComponent<Image>().color = Color.white;
            ItemsSpaces[i].GetComponent<Image>().sprite = ItemsOnSale[index].GetComponent<Item>().Icono;
            nameSpaces[i].text = ItemsOnSale[index].GetComponent<Item>().nombreItem;
            priceSpaces[i].text = ItemsOnSale[index].GetComponent<Item>().price.ToString();
            index++;
        }
    }

    public void DoMethod1()
    {
        inventario.AddItem(ItemsOnSale[0 + (PageActual * 3)].GetComponent<Item>());
    }
    public void DoMethod2()
    {
        inventario.AddItem(ItemsOnSale[1 + (PageActual * 3)].GetComponent<Item>());
    }
    public void DoMethod3()
    {
        inventario.AddItem(ItemsOnSale[2 + (PageActual * 3)].GetComponent<Item>());
    }
    public void changePage(bool atrasAdelante)
    {
        if (atrasAdelante) // true adelante
        {
            if ((PageActual + 1) < (ItemsOnSale.Length / 3f))
            {
                PageActual++;
                ActualPage.text = "" + (PageActual + 1);
                loadPage();
            }
        }
        else
        {
            print("si llego aca");
            if (PageActual != 0)
            {
                PageActual--;
                ActualPage.text = "" + (PageActual + 1);
                loadPage();
            }
        }
    }
}
