using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    GameObject pickUpText;
    GameObject player;

    void Start()
    {
        pickUpText = transform.GetChild(2).gameObject;
    }

    public void PickUp()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
            player.GetComponent<PlayerController3D>().SetNearbyObject(gameObject);
            pickUpText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pickUpText.SetActive(false);
            player.GetComponent<PlayerController3D>().SetNearbyObject(null);
        }
    }
}
