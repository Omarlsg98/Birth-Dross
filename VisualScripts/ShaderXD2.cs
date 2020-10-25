using UnityEngine;
using System.Collections;

public class ShaderXD2 : ShaderXDXD
{

    public bool ladoIzquierdo;

    // Update is called once per frame
    void Update()
    {
        transform.position = Posicionador.transform.position;
        //seguimiento forward Sprite
        rotarToDross();
        SelectPose();
    }
    internal override void SelectPose()
    {
        //Posicionamiento iMagen
        Vector3 dirToDross = Vector3.Normalize(Master.originalDross.transform.position - Posicionador.transform.position);
        float angle = AngleBetweenVector2(Posicionador.transform.forward, dirToDross);
        byte index = 0;
        if (angle < 0f)
            angle += 360;

        if (angle <= 45)
        {
            index = 0; // frontal
        }
        else if (angle <= 135)
        {
            index = 1; // izquierda
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (angle <= 225)
        {
            index = 2; //atras
        }
        else if (angle <= 315)
        {
            index = 1; // Derecha
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (angle > 315)
        {
            index = 0;
        }
        AnimPose actualAnimacion = animacion[curAnimacion];
        Pose actualPose = actualAnimacion.poses[index];
        Sprite spriteActual = actualPose.frames[frameActual];
        GetComponent<SpriteRenderer>().sprite = spriteActual;

    }
}
