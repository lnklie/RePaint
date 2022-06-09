using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * 최종수정일 : 2022-06-05
 * 작성자 : KJY
 * 파일명 : DayandNight.cs
==============================
*/
public class DayandNight : MonoBehaviour
{
    private float nightFogDensity = 0.0025f;
    private bool isNight = false;

    [SerializeField] 
    private float worldTime = 0;
    [SerializeField]
    private Light worldLight = null;
    [SerializeField] 
    private Material skyNight = null;
    [SerializeField] 
    private Material skyDay = null;

    private void Start()
    {
        Light[] lights = GameObject.FindObjectsOfType<Light>();

        foreach(Light light in lights)
        {
            if(light.gameObject.name.StartsWith("Directional Light"))
            {
                worldLight = light;
                break;
            }
        }
    }


    void FixedUpdate()
    {
        SunAndMoon();
    }

    private void SunAndMoon()
    {
        worldLight.transform.Rotate(Vector3.right, 0.1f * worldTime * Time.deltaTime);

        if (worldLight.transform.eulerAngles.x >= 200f)
        {
            isNight = true;
            RenderSettings.skybox = skyNight;
            RenderSettings.fogDensity = nightFogDensity;
        }
        else if (worldLight.transform.eulerAngles.x >= 0f)
        {
            isNight = false;
            RenderSettings.skybox = skyDay;
            RenderSettings.fogDensity = 0.001f;
        }
    }
}
