using UnityEngine;
using System.Collections;

public class BarraScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    internal void modBarra(float max, float actual)
    {
        if (actual > max)
        {
            actual = max;
        }
        else if (actual <= 0)
        {
            actual = 0;
        }
        transform.localScale = new Vector3((actual / max), transform.localScale.y, transform.localScale.z);
    }
    void Update()
    {
        //seguimiento forward Sprite
        Vector3 dirToDross = Vector3.Normalize(Master.originalDross.transform.position - transform.position);
        float angle = Mathf.Atan2(dirToDross.x, dirToDross.z) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1f);
        transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, rotation, 1f);
    }
}
