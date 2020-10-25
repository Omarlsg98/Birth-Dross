using UnityEngine;
using System.Collections;

public class Arma : MonoBehaviour {

    public void FalseArmaAtack()
    {
        GetComponent<Animator>().SetBool("Attacking", false);
    }

}
