using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateColliderController : MonoBehaviour
{
    bool openGate;
    GameObject gateKey;
    AudioSource gateOpoenAudio;

    void Start()
    {
        gateKey = transform.parent.GetComponent<GateController>().gateKey;
        gateOpoenAudio = GetComponentInParent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        openGate = other.CompareTag("Player") && other.gameObject.GetComponent<PlayerController3D>().IsHoldingObject() && other.gameObject.GetComponent<PlayerController3D>().ObjectHeld() == gateKey;
        if (openGate)
        {
            other.gameObject.GetComponent<PlayerController3D>().UseKey();
            gateOpoenAudio.Play();
        }
    }

    public bool OpenGate()
    {
        return openGate;
    }
}
