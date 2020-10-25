using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;

public abstract class Consumible : Item
{
    public float duracion;
	public AudioClip[] sound;
    internal DrossBehaviour Dross;
	internal DrossBehaviour audioSourceHelper;

    internal override void Use()
    {
        Dross = Master.Dross.GetComponent<DrossBehaviour>();
        ConsumibleUse();
    }

    internal virtual void ConsumibleUse()
    {
        //Master.Dross.GetComponent<AudioSource>().PlayOneShot(Sound);

		if (sound.Length > 0) 
			Master.soundLayer1.GetComponent<AudioSource> ().PlayOneShot (sound [Random.Range (0, sound.Length)]);
		
			Invoke ("EndEffect", duracion);

    }

    internal virtual void EndEffect()
    {
        Destroy(gameObject);
    }
}
