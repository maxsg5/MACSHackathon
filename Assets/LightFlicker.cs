using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    
    public Light2D light2D;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //flicker the light intensity with a custom graph
        light2D.intensity = Mathf.PerlinNoise(Time.time, 0.0f);
        
    }
}
