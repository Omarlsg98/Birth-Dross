using UnityEngine;
using System.Collections;

public class OnBalazoSostenido : OnBalazo {

	// Use this for initialization
	void Start () {
	
	}
	internal void SeDispara()
    {
        Destroy(gameObject, Time);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
