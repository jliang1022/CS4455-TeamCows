using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{
    public GameObject player;
    public RawImage displayKey;
    public RawImage displayRock;
    Transform ball;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.childCount > 1)
        {
            ball = player.transform.GetChild(1);
            if (ball.gameObject.tag == "Key")
            {
                displayKey.enabled = true;
                displayRock.enabled = false;
            }
            if (ball.gameObject.tag == "Rock")
            {
                displayRock.enabled = true;
                displayKey.enabled = false;
            }
        } else
        {
            displayKey.enabled = false;
            displayRock.enabled = false;
        }
    }
}
