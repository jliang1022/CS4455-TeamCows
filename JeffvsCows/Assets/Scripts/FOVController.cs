using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVController : MonoBehaviour
{
    bool canSeePlayer;

    private void OnTriggerEnter(Collider other)
    {
        canSeePlayer = other.gameObject.CompareTag("Player");
    }

    private void OnTriggerExit(Collider other)
    {
        if (canSeePlayer)
            canSeePlayer = !other.gameObject.CompareTag("Player");
    }

    public bool CanSeePlayer()
    {
        return canSeePlayer;
    }
}
