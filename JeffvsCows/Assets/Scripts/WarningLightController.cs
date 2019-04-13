using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningLightController : MonoBehaviour
{
    Light light;
    float maxIntensity = 5f, minIntensity = 0f, flashSpeed = 5f, intensity;
    bool on, activated;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        intensity = minIntensity;
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            if (!on)
                intensity += flashSpeed * Time.deltaTime;
            else
                intensity -= flashSpeed * Time.deltaTime;

            if ((intensity >= maxIntensity && !on) || (intensity <= minIntensity && on))
                on = !on;
                
            light.intensity = intensity;
        }
        else
        {
            if (intensity > minIntensity)
            {
                intensity -= flashSpeed * Time.deltaTime;
                light.intensity = intensity;
            }
        }
        transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
    }

    public void Activate()
    {
        activated = true;
    }

    public void Deactivate()
    {
        activated = false;
    }
}
