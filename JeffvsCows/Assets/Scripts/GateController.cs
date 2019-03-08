using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    public Vector3 closedPos, openPos;
    public GameObject gateKey;
    GateColliderController gateColliderController;
    bool gateOpening;
    float gateSpeed, startTime, openDist;

    void Start()
    {
        closedPos = transform.position;
        gateColliderController = transform.GetChild(0).GetComponent<GateColliderController>();
        openDist = Vector3.Distance(closedPos, openPos);
        gateSpeed = 5f;
   }

    private void Update()
    {
        if (gateOpening || gateColliderController.OpenGate())
        {
            OpenGate();
        }
    }

    void OpenGate()
    {
        if (gateOpening == false)
        {
            gateOpening = true;
            startTime = Time.time;
        }
        else
        {
            float distCovered = (Time.time - startTime) * gateSpeed;
            float fracJourney = distCovered / openDist;
            transform.position = Vector3.Lerp(closedPos, openPos, fracJourney);
        }
    }

    void CloseGate()
    {

    }
}
