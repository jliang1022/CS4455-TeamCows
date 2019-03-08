using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    Vector3 offset;
    void Start()
    {
        transform.eulerAngles = new Vector3(45, 0, 0);
        offset = new Vector3(0, 10, -10);
    }

    void Update()
    {
        transform.position = player.transform.position + offset;
    }
}
