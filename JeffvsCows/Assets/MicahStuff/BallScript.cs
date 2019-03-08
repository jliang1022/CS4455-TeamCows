using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public GameObject arm;
    public Rigidbody ball;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Mathf.Abs(arm.transform.position.x - ball.transform.position.x) + Mathf.Abs(arm.transform.position.y - ball.transform.position.y) + Mathf.Abs(arm.transform.position.z - ball.transform.position.z);
        if (arm.transform.childCount < 3 && distance < 2.1f)
        {
            ball.useGravity = false;
            ball.isKinematic = true;
            ball.transform.localScale = new Vector3(0, 0, 0);
            // ball.transform.position = new Vector3(arm.transform.position.x, arm.transform.position.y - 1, arm.transform.position.z - 0.5f);
            ball.transform.rotation = arm.transform.rotation;
            transform.parent = arm.transform;
        }
    }

    public void ReleaseMe()
    {
        if (arm.transform.childCount >= 3)
        {
            if (ball.tag == "Key")
            {
                ball.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            } else if (ball.tag == "Rock")
            {
                ball.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
            }
            ball.useGravity = true;
            ball.isKinematic = false;
            transform.parent = null;
            transform.rotation = arm.transform.rotation;
            ball.AddForce(transform.forward * 600);
        }
    }
}
