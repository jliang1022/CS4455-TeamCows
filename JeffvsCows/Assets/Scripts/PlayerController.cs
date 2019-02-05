using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller3D))]
public class PlayerController : MonoBehaviour
{
    Controller3D controller;
    BoxCollider collider;

    public float gravity = -1.0f;
    public float moveSpeed = 2f, rollSpeed = 4f;
    Vector3 velocity;

    bool rolling;
    float rollTime = 0.5f, rollTimeRemaining;
    Vector3 rollDirection;

    void Start()
    {
        controller = GetComponent<Controller3D>();
        collider = GetComponent<BoxCollider>();
        rolling = false;
        velocity = new Vector3();
    }
    
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = targetVelocityX;
        //velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        velocity.z = input.y * moveSpeed;

        if (velocity.magnitude > moveSpeed)
        {
            velocity = velocity.normalized * moveSpeed;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            Debug.Log("shift pressed");
            if (!rolling)
            {
                rolling = true;
                rollTimeRemaining = rollTime;
                rollDirection = GetFaceDir();
                Debug.Log(rolling);
            }
        }

        if (rolling)
        {
            velocity = rollDirection * rollSpeed;
            rollTimeRemaining -= Time.deltaTime;
            rolling = rollTimeRemaining > 0f;
        }

        velocity.y += gravity * Time.deltaTime;
        Debug.Log(rollDirection);
        //Debug.Log(rolling);
        controller.Move(velocity * Time.deltaTime);
    }

    Vector3 GetFaceDir()
    {
        Vector3 direction = new Vector3();
        switch(controller.collisions.faceDir)
        {
            case 0:
                direction = new Vector3(0, 0, 1);
                break;
            case 1:
                direction = new Vector3(0.5f, 0, 0.5f);
                break;
            case 2:
                direction = new Vector3(1, 0, 0);
                break;
            case 3:
                direction = new Vector3(0.5f, 0, -0.5f);
                break;
            case 4:
                direction = new Vector3(0, 0, -1);
                break;
            case 5:
                direction = new Vector3(-0.5f, 0, -0.5f);
                break;
            case 6:
                direction = new Vector3(-1, 0, 0);
                break;
            case 7:
                direction = new Vector3(-0.5f, 0, 0.5f);
                break;
        }
        return direction;
    }
}