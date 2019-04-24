using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateColliderController : MonoBehaviour
{
    bool openGate;
    GameObject gateKey;
    Transform child;
    AudioSource gateOpoenAudio;

    void Start()
    {
        gateKey = transform.parent.GetComponent<GateController>().gateKey;
        gateOpoenAudio = GetComponentInParent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.childCount > 1)
            {
                child = other.transform.GetChild(1);
                if (child.gameObject == gateKey)
                {
                    openGate = true;
                    child.transform.parent = null;
                    Destroy(child.gameObject);
                    GetComponent<Collider>().enabled = false;
                    gateOpoenAudio.Play();
                }
            }
        }
    }

    public bool OpenGate()
    {
        return openGate;
    }
}
