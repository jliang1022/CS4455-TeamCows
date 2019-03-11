using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : MonoBehaviour
{
    public GameObject player;
    public GameObject objToThrow;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void ThrowBall()
    {
        foreach (Transform child in player.transform)
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