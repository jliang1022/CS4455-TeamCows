using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    Vector3 closedPos;
    public GateType type;
    public Vector3 openPos;
    public GameObject gateKey;
    GateColliderController gateColliderController;
    bool gateOpening;
    float gateSpeed, startTime, openDist;

    public enum GateType
    {
        xGate, yGate, zGate
    }

    void Start()
    {
        closedPos = transform.localPosition;
        gateColliderController = transform.GetChild(0).GetComponent<GateColliderController>();
        if (type == GateType.xGate)
            openPos = new Vector3(openPos.x, closedPos.y, closedPos.z);
        else if (type == GateType.yGate)
            openPos = new Vector3(closedPos.x, openPos.y, closedPos.z);
        else
            openPos = new Vector3(closedPos.x, closedPos.y, openPos.z);
        openDist = Vector3.Distance(closedPos, openPos);
        gateSpeed = 2f;
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
            transform.localPosition = Vector3.Lerp(closedPos, openPos, fracJourney);
        }
    }

    void CloseGate()
    {

    }
}
