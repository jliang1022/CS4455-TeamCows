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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ThrowBall();
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
        if (objToThrow != null)
        {
            BallScript ballscript = objToThrow.GetComponent<BallScript>();
            ballscript.ReleaseMe();
        }
    }
}