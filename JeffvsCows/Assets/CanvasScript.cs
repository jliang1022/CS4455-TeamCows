using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{
    public GameObject rightArm;
    public RawImage displayKey;
    public RawImage displayRock;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (rightArm.transform.childCount > 2)
        {
            foreach (Transform obj in rightArm.transform)
            {
                if (obj.gameObject.tag == "Key")
                {
                    displayKey.enabled = true;
                    displayRock.enabled = false;
                }
                if (obj.gameObject.tag == "Rock")
                {
                    displayRock.enabled = true;
                    displayKey.enabled = false;
                }
            }
        } else
        {
            displayKey.enabled = false;
            displayRock.enabled = false;
        }
    }
}
