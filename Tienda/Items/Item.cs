using UnityEngine;
using System.Collections;
using System;

public abstract class Item : MonoBehaviour
{
    public Sprite Icono;
    public string nombreItem;
    public float price;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    internal abstract void Use();
}
