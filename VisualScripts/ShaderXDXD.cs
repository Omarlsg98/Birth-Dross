using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class Pose
{
    public Sprite[] frames;

    public Pose(int nFrames)
    {
        frames = new Sprite[nFrames];
    }
}
[Serializable]
public class AnimPose
{
    public Pose[] poses;
    public AnimationTypes animationType;

    public AnimPose(int nPoses, int nFrames)
    {
        poses = new Pose[nPoses];
        for (int j = 0; j < nPoses; j++)
        {
            poses[j] = new Pose(nFrames);
        }
    }
}
[Serializable]
public class SpriteToAnim
{
    public Texture2D SpriteSheet;
    public AnimationTypes animationType;
    public int nFrames;
}
public enum AnimationTypes { movement = 0, attack = 1, hit = 2, die = 3 }
public class ShaderXDXD : MonoBehaviour
{
    internal AnimPose[] animacion;
    public AnimPose[] animacionesSinSS;
    public SpriteToAnim[] animacionsConSS;
    public int nPoses;
    public GameObject Posicionador;
    public GameObject alternativePosi;
    internal byte frameActual;
    public float TimeBetweenFrames;
    internal byte curAnimacion;
    public bool loopActualAnimation = true;


    public byte CurAnimacion
    {
        get
        {
            return curAnimacion;
        }

        set
        {
            curAnimacion = value;
            frameActual = 0;
        }
    }



    // Use this for initialization
    internal void Spawn()
    {
        Posicionador.GetComponent<Mortal>().Spawn(this);
        Load();
        setAnimation(AnimationTypes.movement, true);
        frameActual = 0;
        StartCoroutine(AnimationRutine());
    }

    internal void Load()
    {
        animacion = new AnimPose[animacionsConSS.Length + animacionesSinSS.Length];
        for (int j = 0; j < animacionsConSS.Length; j++)
        {
            animacion[j] = new AnimPose(nPoses, animacionsConSS[j].nFrames);
            animacion[j].animationType = animacionsConSS[j].animationType;
        }
        for (int j = animacionsConSS.Length; j < animacionsConSS.Length + animacionesSinSS.Length; j++)
        {
            animacion[j] = animacionesSinSS[j - animacionsConSS.Length];
        }

        for (int k = 0; k < animacionsConSS.Length; k++)
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>(animacionsConSS[k].SpriteSheet.name);
            for (int i = 0; i < animacionsConSS[k].nFrames; i++)
            {
                for (int j = 0; j < nPoses; j++)
                {
                    animacion[k].poses[j].frames[i] = sprites[(i * 4) + j];
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Posicionador.transform.position;
        //seguimiento forward Sprite
        rotarToDross();
        SelectPose();
    }

    internal void rotarToDross()
    {
        Vector3 dirToDross = Vector3.Normalize(Master.originalDross.transform.position - transform.position);
        float angle = Mathf.Atan2(dirToDross.x, dirToDross.z) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1f);
    }

    internal virtual void SelectPose()
    {
        //Posicionamiento iMagen
        Vector3 dirToDross;
        float angle;
        if (alternativePosi == null)
        {
            dirToDross = Vector3.Normalize(Master.originalDross.transform.position - Posicionador.transform.position);
            angle = AngleBetweenVector2(Posicionador.transform.forward, dirToDross);
        }
        else
        {
            dirToDross = Vector3.Normalize(Master.originalDross.transform.position - alternativePosi.transform.position);
            angle = AngleBetweenVector2(alternativePosi.transform.forward, dirToDross);
        }
        byte index = 0;
        if (angle < 0f)
            angle += 360;

        if (angle <= 45)
            index = 0; // frontal
        else if (angle <= 135)
            index = 1; // izquierda
        else if (angle <= 225)
            index = 2; //atras
        else if (angle <= 315)
            index = 3; // Derecha
        else if (angle > 315)
            index = 0;
        GetComponent<SpriteRenderer>().sprite = animacion[curAnimacion].poses[index].frames[frameActual];
    }

    internal void setAnimation(AnimationTypes animType, bool loop, float time)
    {

        setAnimation(animType, loop);
        TimeBetweenFrames = time;
    }
    internal void setAnimation(AnimationTypes animType, bool loop)
    {
        curAnimacion = GetIndexByName(animType);
        loopActualAnimation = loop;
        frameActual = 0;
    }

    private byte GetIndexByName(AnimationTypes animType)
    {
        List<byte> posibleIndexToReturn = new List<byte>();
        for (byte i = 0; i < animacion.Length; i++)
        {
            if (animacion[i].animationType == animType)
            {
                posibleIndexToReturn.Add(i);
            }
        }
        if (posibleIndexToReturn.Count > 0)
        {
            return posibleIndexToReturn[UnityEngine.Random.Range(0, posibleIndexToReturn.Count)];
        }
        throw new Exception("Tipo de la animacion no existe paps");
    }

    internal IEnumerator AnimationRutine()
    {
        float TimeDef = TimeBetweenFrames;
        for (;;)
        {
            if (frameActual == animacion[curAnimacion].poses[0].frames.Length - 1)
            {
                frameActual = 0;
                if (!loopActualAnimation)
                {
                    if (Posicionador.GetComponent<Monster>() != null)
                    {
                        if (!Posicionador.GetComponent<Monster>().die)
                            setAnimation(AnimationTypes.movement, true, TimeDef);
                        else
                            frameActual = (byte)(animacion[curAnimacion].poses[0].frames.Length - 1);
                    }
                    else
                    {

                        setAnimation(AnimationTypes.movement, true, TimeDef);
                    }
                }
            }
            else
            {
                frameActual++;
            }
            yield return new WaitForSeconds(TimeBetweenFrames);
        }
    }

    internal static float AngleBetweenVector2(Vector3 from, Vector3 to)
    {
        // the vector that we want to measure an angle from
        Vector3 referenceForward = from;/* some vector that is not Vector3.up */
                                        // the vector perpendicular to referenceForward (90 degrees clockwise)
                                        // (used to determine if angle is positive or negative)
        Vector3 referenceRight = Vector3.Cross(Vector3.up, referenceForward);
        // the vector of interest
        Vector3 newDirection = to;/* some vector that we're interested in */
                                  // Get the angle in degrees between 0 and 180
        float angle = Vector3.Angle(newDirection, referenceForward);
        // Determine if the degree value should be negative.  Here, a positive value
        // from the dot product means that our vector is on the right of the reference vector   
        // whereas a negative value means we're on the left.
        float sign = Mathf.Sign(Vector3.Dot(newDirection, referenceRight));
        return sign * angle;
    }
}
