using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : MonoBehaviour
{
    public GameObject rightArm;
    public GameObject objToThrow;

    // Start is called before the first frame update
    void Start()
    {
        rightArm = GameObject.FindGameObjectWithTag("ThrowingArm");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ThrowBall()
    {
        foreach (Transform child in rightArm.transform)
        {
            if (child.gameObject.tag == "Key" || child.gameObject.tag == "Rock")
            {
                objToThrow = child.gameObject;
            }
        }
        BallScript ballscript = objToThrow.GetComponent<BallScript>();
        ballscript.ReleaseMe();
    }
}
