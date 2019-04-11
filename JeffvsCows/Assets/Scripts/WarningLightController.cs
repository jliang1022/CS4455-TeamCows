using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningLightController : MonoBehaviour
{
    Light light;
    float maxIntensity = 5f, minIntensity = 0f, flashSpeed = 5f, intensity;
    bool on, activated;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        intensity = minIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(on);
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
