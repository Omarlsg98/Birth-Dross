using UnityEngine;
using System.Collections;

public class CaidaDetectScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerStay(Collider coll)
    {
        print(" entra y : ");
        if (coll.transform.tag == "Player" || coll.transform.tag == "Enemy")
        {
            print("sdjfkjsdhfjksd");
            coll.gameObject.GetComponent<Mortal>().dieProcess();
        }

    }
}
