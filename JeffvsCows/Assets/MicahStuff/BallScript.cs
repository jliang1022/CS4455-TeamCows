using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public GameObject player;
    public Rigidbody ball;
    public bool ballJustThrown;
    AudioSource keyPickupSound;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        ball = GetComponent<Rigidbody>();
        ballJustThrown = false;
        keyPickupSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ball.velocity == Vector3.zero)
        {
            ballJustThrown = false;
            ball.GetComponent<Collider>().enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !ballJustThrown)
        {
            if (player.transform.childCount < 2)
            {
                if (this.tag == "Key")
                {
                    keyPickupSound.Play();
                }
                ball.useGravity = false;
                ball.isKinematic = true;
                ball.GetComponent<Collider>().enabled = false;
                ball.transform.parent = player.transform;
                ball.transform.rotation = player.transform.rotation;
                ball.transform.position += new Vector3(0, 0.5f, 0);
                ball.transform.localScale = new Vector3(0, 0, 0);
            }
        }
    }

    public void ReleaseMe()
    {
        if (player.transform.childCount >= 2)
        {
            if (ball.tag == "Key")
            {
                this.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            } else if (ball.tag == "Rock")
            {
                ball.transform.localScale = new Vector3(0.0015f, 0.0015f, 0.0015f);
            }
            ball.useGravity = true;
            ball.isKinematic = false;
            ball.transform.parent = null;
            ball.transform.rotation = player.transform.rotation;
            ball.AddForce(transform.forward * 700);
        }
    }
}
