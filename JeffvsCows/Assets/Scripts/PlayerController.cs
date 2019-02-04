using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller3D))]
public class PlayerController : MonoBehaviour
{
    Controller3D controller;
    BoxCollider collider;

    public float gravity = -1.0f;
    public float moveSpeed = 2;
    Vector3 velocity;

    void Start()
    {
        controller = GetComponent<Controller3D>();
        collider = GetComponent<BoxCollider>();

        velocity = new Vector3();
    }


    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = targetVelocityX;
        //velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}