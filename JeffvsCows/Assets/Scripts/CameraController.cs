using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float smoothSpeed = 0.125f;
    Vector3 offset;

    void Start()
    {
        transform.eulerAngles = new Vector3(45, 0, 0);
        offset = new Vector3(0, 10, -10);
    }

    void LateUpdate()
    {
        Vector3 desiredPos = player.transform.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
        //transform.LookAt(player.transform);
    }
}
