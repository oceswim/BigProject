using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustLight : MonoBehaviour
{
    private Light myLight;
    private int lightValue;
    // Start is called before the first frame update
    void Start()
    {
        myLight = GetComponent<Light>();
        lightValue = GameManager.SliderValue;

        if(lightValue>=0 && lightValue<=10)
        {
            myLight.intensity = .1f;
        }
        else if(lightValue>10 && lightValue <=20)
        {
            myLight.intensity = .2f;
        }
        else if (lightValue > 20 && lightValue <= 30)
        {
            myLight.intensity = .3f;
        }
        else if (lightValue > 30 && lightValue <= 40)
        {
            myLight.intensity = .4f;
        }
        else if (lightValue > 40 && lightValue <= 50)
        {
            myLight.intensity = .5f;
        }
        else if (lightValue > 50 && lightValue <= 60)
        {
            myLight.intensity = .6f;
        }
        else if (lightValue > 60 && lightValue <= 70)
        {
            myLight.intensity = .7f;
        }
        else if (lightValue > 70 && lightValue <= 80)
        {
            myLight.intensity = .8f;
        }
        else if (lightValue > 80 && lightValue <= 90)
        {
            myLight.intensity = .9f;
        }
        else if (lightValue > 90 && lightValue <= 100)
        {
            myLight.intensity = 1;
        }
    }

}
