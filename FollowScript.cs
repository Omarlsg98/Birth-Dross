using UnityEngine;
using System.Collections;

public class FollowScript : MonoBehaviour
{

    public Transform Target;

    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Target.position.x, transform.position.y, Target.position.z);
		//transform.rotation = new Quaternion (Target.rotation.y+90, 0,Target.rotation.z,0); 

	}   
}
