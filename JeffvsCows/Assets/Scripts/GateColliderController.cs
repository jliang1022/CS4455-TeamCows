using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateColliderController : MonoBehaviour
{
    bool openGate;
    GameObject gateKey;
    Transform child;

    void Start()
    {
        gateKey = transform.parent.GetComponent<GateController>().gateKey;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.childCount > 1)
            {
                child = other.transform.GetChild(1);
                if (child.gameObject.tag == "Key")
                {
                    openGate = true;
                    child.transform.parent = null;
                    Destroy(child.gameObject);
                    GetComponent<Collider>().enabled = false;
                }
            }
        }
    }

    public bool OpenGate()
    {
        return openGate;
    }
}
