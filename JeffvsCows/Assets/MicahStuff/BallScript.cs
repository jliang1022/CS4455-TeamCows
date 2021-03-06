﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    GameObject player;
    Rigidbody ball;
    bool canPickUp, thrown;
    AudioSource sound;
    float colliderOffTime, colliderOffTimeLeft;
    Vector3 defaultPos;
    Quaternion defaultRot;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        ball = GetComponent<Rigidbody>();
        canPickUp = true;
        sound = GetComponent<AudioSource>();
        colliderOffTime = 0.5f;
        defaultPos = transform.position;
        defaultRot = transform.rotation;
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
        if (other.CompareTag("Player") && canPickUp)
        {
            if (player.transform.childCount < 2)
            {
                if (CompareTag("Key"))
                {
                    sound.Play();
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
            if (thrown && gameObject.CompareTag("Rock"))
            {
                sound.Play();
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
            }
            else if (ball.tag == "Rock")
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

    public void ReturnBall()
    {
        gameObject.SetActive(true);
        ball.GetComponent<Collider>().enabled = true;
        if (ball.tag == "Key")
        {
            transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }
        else if (ball.tag == "Rock")
        {
            ball.transform.localScale = new Vector3(0.0015f, 0.0015f, 0.0015f);
        }
        ball.useGravity = true;
        ball.isKinematic = false;
        ball.transform.parent = null;
        ball.transform.position = defaultPos;
        ball.transform.rotation = defaultRot;
        Debug.Log(defaultPos);
    }
}
