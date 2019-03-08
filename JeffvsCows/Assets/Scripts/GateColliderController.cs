using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateColliderController : MonoBehaviour
{
    bool openGate;
    GameObject gateKey;

    void Start()
    {
        gateKey = transform.parent.GetComponent<GateController>().gateKey;
    }

    private void OnTriggerEnter(Collider other)
    {
        openGate = other.CompareTag("Player") && other.gameObject.GetComponent<PlayerController3D>().IsHoldingObject() && other.gameObject.GetComponent<PlayerController3D>().ObjectHeld() == gateKey;
        if (openGate)
            other.gameObject.GetComponent<PlayerController3D>().UseKey();
    }

    public bool OpenGate()
    {
        return openGate;
    }
}
