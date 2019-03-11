﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerController3D : MonoBehaviour
{
    public float moveSpeed, turnSpeed, gravityScale = 1f, dashTime = 1f, dashSpeed = 1f;
    public Vector3 moveDirection, velocity;
    float dashTimeLeft;
    GameObject objectHeld;
    CharacterController characterController;
    Direction faceDir;
    Animator anim;
    public GameObject rightArm;
    public GameObject objToThrow;
    AudioSource footsteps;
    AudioSource dodge;
    AudioSource[] playerSounds;

    enum Direction { North, East, South, West, NorthEast, NorthWest, SouthEast, SouthWest };

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerSounds = GetComponents<AudioSource>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        faceDir = Direction.South;
        rightArm = GameObject.FindGameObjectWithTag("ThrowingArm");
        footsteps = playerSounds[0];
        dodge = playerSounds[1];
    }

    void Update()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        if ((moveDirection.x != 0 || moveDirection.y != 0 || moveDirection.z != 0) && !anim.GetBool("Rolling"))
        {
            footsteps.volume = 0.3f;
            footsteps.pitch = 0.9f;
        } else
        {
            footsteps.volume = 0f;
        }
        velocity = moveDirection.normalized * moveSpeed;
        velocity.y += Physics.gravity.y * gravityScale;

        UpdateFaceDir();

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            dodge.Play();
            if (dashTimeLeft <= 0.001)
            {
                dashTimeLeft = dashTime;
            } 
        }

        if (dashTimeLeft > 0)
        {
            velocity = transform.forward * dashSpeed;
            dashTimeLeft -= Time.deltaTime;
        }

        characterController.Move(velocity * Time.deltaTime);
        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        anim.SetBool("Throwing", Input.GetKeyDown("space"));
        anim.SetBool("Moving", Mathf.Abs(moveDirection.x) > 0.01 || Mathf.Abs(moveDirection.z) > 0.01);
        anim.SetBool("Rolling", dashTimeLeft > 0);
    }

    void UpdateFaceDir()
    {
        Vector3 faceDirVec = new Vector3();
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            faceDirVec += new Vector3(-1, 0, 0);
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            faceDirVec += new Vector3(1, 0, 0);
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            faceDirVec += new Vector3(0, 0, 1);
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            faceDirVec += new Vector3(0, 0, -1);

        Vector3 newDir = Vector3.RotateTowards(transform.forward, faceDirVec, turnSpeed * Time.deltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(newDir);

        if (faceDirVec.x - 1 > -0.01 && faceDirVec.z - 1 > -0.01)
            faceDir = Direction.NorthEast;
        else if (faceDirVec.x + 1 < 0.01 && faceDirVec.z - 1 > -0.01)
            faceDir = Direction.NorthWest;
        else if (faceDirVec.z - 1 > -0.01)
            faceDir = Direction.North;
        else if (faceDirVec.x - 1 > -0.01 && faceDirVec.z + 1 < 0.01)
            faceDir = Direction.SouthEast;
        else if (faceDirVec.x + 1 < 0.01 && faceDirVec.z + 1 < 0.01)
                faceDir = Direction.SouthWest;
        else if (faceDirVec.z + 1 < 0.01)
            faceDir = Direction.South;
        else if (faceDirVec.x - 1 > -0.01)
            faceDir = Direction.East;
        else if (faceDirVec.x + 1 < 0.01)
            faceDir = Direction.West;
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public void UseKey()
    {
        objectHeld = null;
    }
}
