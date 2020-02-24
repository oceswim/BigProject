using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AzimuthAngle : MonoBehaviour
{
    public TMP_InputField MinAngleAzim, MaxAngleAzim, Step3; 
    // Start is called before the first frame update
    public void MinAngleAzimInput()
    {
        Debug.Log("AZIM ANGLE: The min angle is now" + MinAngleAzim.text);

    }
    public void MaxAngleAzimInput()
    {
        Debug.Log("AZIM ANGLE: The max angle is now" + MaxAngleAzim.text);

    }
    public void Step3Input()
    {
        Debug.Log("AZIM ANGLE: The step3 is now" + Step3.text);

    }
}
