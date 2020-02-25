using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AzimuthAngle : MonoBehaviour
{
    private const int MAXANGLE = 180;
    public TMP_InputField MinAngleAzim, MaxAngleAzim, Step3; 
    // Start is called before the first frame update
    public void MinAngleAzimInput()
    {
        Debug.Log("AZIM ANGLE: The min angle is now" + MinAngleAzim.text);
        GameManager.azimuthMinAngle = int.Parse(MinAngleAzim.text);
    }
    public void MaxAngleAzimInput()
    {
        int newMaxVal = int.Parse(MaxAngleAzim.text);
        if (newMaxVal <= MAXANGLE)
        {
            GameManager.azimuthMaxAngle = newMaxVal;
        }
        else
        {
            GameManager.azimuthMaxAngle = MAXANGLE;
        }
        Debug.Log("AZIM ANGLE: The max angle is now" + GameManager.azimuthMaxAngle);


    }
    public void Step3Input()
    {
        Debug.Log("AZIM ANGLE: The step3 is now" + Step3.text);
        GameManager.step3 = int.Parse(Step3.text);


    }
}
