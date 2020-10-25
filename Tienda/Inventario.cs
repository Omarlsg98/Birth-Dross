using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class Inventario : MonoBehaviour
{

    private Item[] InventarioL = new Item[3];
    // public GameObject[] ItemPrueba;
    public Image[] Iconos = new Image[3];
    private Color InicialColor;

    // Use this for initialization
    void Start()
    {
        InicialColor = Iconos[1].color;
        //foreach (GameObject itemObject in ItemPrueba)
        //{
        //    GameObject item = Instantiate(itemObject) as GameObject;
        //    AddItem(item.GetComponent<Item>());
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Item1"))
        {
            UseItem(0);
        }
        else if (Input.GetButtonDown("Item2"))
        {
            UseItem(1);
        }
        else if (Input.GetButtonDown("Item3"))
        {
            UseItem(2);
        }
    }
    private void UseItem(int i)
    {
        if (InventarioL[i] != null)
        {
            InventarioL[i].Use();
            Iconos[i].sprite = null;
            Iconos[i].color = InicialColor;
        }
    }

    internal bool haveCapacity()
    {
        foreach (Item item in InventarioL)
        {
            if (item == null)
            {
                return true;
            }
        }
        return false;
    }

    internal bool AddItem(Item item)
    {
        for (int i = 0; i < InventarioL.Length; i++)
        {
            if (InventarioL[i] == null)
            {
                if (Master.payingMoney(item.GetComponent<Item>().price))
                {
                    GameObject gameItem = Instantiate(item.gameObject);
                    InventarioL[i] = gameItem.GetComponent<Item>();
                    ChangeInGui(i);
                }
                return true;
            }
        }
        return false;
    }

    internal void ChangeInGui(int posiItem)
    {
        Iconos[posiItem].sprite = InventarioL[posiItem].Icono;
        Iconos[posiItem].color = Color.white;
    }
}

