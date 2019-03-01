using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowController : MonoBehaviour
{
    public GameObject player;
    float speed = 3f;
    float timer = 5f;

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            transform.LookAt(player.transform);

            if (Vector3.Distance(transform.position, player.transform.position) >= 1f)
            {

                transform.position += transform.forward * speed * Time.deltaTime;
            }
        }
        else
        {
            transform.Rotate(Vector3.up, speed * Time.deltaTime);
            timer -= Time.deltaTime;
        }
    }

    public void PlayerFound(GameObject p)
    {
        player = p;
    }
}
