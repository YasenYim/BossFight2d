using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightShake : MonoBehaviour
{
    Light2D light2d;
    public float maxLight = 1.4f;
    public float minLight = 0.7f;
    public float changeLightTime = 3f;

    private void Start()
    {
        light2d = GetComponent<Light2D>();
        StartCoroutine(CoLight());
    }


    IEnumerator CoLight()
    {
        float changeSpeed = (maxLight - minLight) / changeLightTime;

        while(true)
        {
            while (light2d.intensity < maxLight)
            {
                light2d.intensity += changeSpeed * Time.deltaTime;
                yield return null;
            }
            while (light2d.intensity > minLight)
            {
                light2d.intensity -= changeSpeed * Time.deltaTime;
                yield return null;
            }
        }
      
    }
}
