using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    GameObject player;
    Rigidbody ball;
    bool canPickUp, thrown;
    AudioSource keyPickupSound;
    float colliderOffTime, colliderOffTimeLeft;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        ball = GetComponent<Rigidbody>();
        canPickUp = true;
        keyPickupSound = GetComponent<AudioSource>();
        colliderOffTime = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (colliderOffTimeLeft <= 0)
        {
            ball.GetComponent<Collider>().enabled = true;
        }
        else
            colliderOffTimeLeft -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && canPickUp)
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
                colliderOffTimeLeft = colliderOffTime;
                ball.transform.parent = player.transform;
                ball.transform.rotation = player.transform.rotation;
                ball.transform.position += new Vector3(0, 0.5f, 0);
                ball.transform.localScale = new Vector3(0, 0, 0);
                canPickUp = false;
                thrown = true;
            }
        }
        else if (other.CompareTag("Environment"))
        {
            canPickUp = true;
            if (thrown)
            {
                //play sound
                GameObject.Find("GlobalControl").GetComponent<GlobalController>().AlertCows(gameObject);
                Debug.Log("Sound made");
            }
            thrown = false;
        }
    }

    public void ReleaseMe()
    {
        if (player.transform.childCount >= 2)
        {
            if (ball.tag == "Key")
            {
                transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            } else if (ball.tag == "Rock")
            {
                ball.transform.localScale = new Vector3(0.0015f, 0.0015f, 0.0015f);
            }
            ball.useGravity = true;
            ball.isKinematic = false;
            ball.transform.parent = null;
            ball.transform.rotation = player.transform.rotation;
            ball.GetComponent<Collider>().enabled = false;
            ball.AddForce(transform.forward * 700);
        }
    }
}
