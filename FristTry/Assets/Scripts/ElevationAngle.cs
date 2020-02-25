using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ElevationAngle : MonoBehaviour
{
    private const int MAXANGLE = 90;
    public TMP_InputField MinAngle, MaxAngle, Step2; 
    // Start is called before the first frame update
    public void MinAngleInput()
    {
        Debug.Log("ELEV: The min angle elev is now" + MinAngle.text);
        GameManager.elevationMinAngle = int.Parse(MinAngle.text);

    }
    public void MaxAngleInput()
    {
        int newMaxVal = int.Parse(MaxAngle.text);
       
        if (newMaxVal <= MAXANGLE)
        {
            GameManager.elevationMaxAngle = newMaxVal;

        }
        else
        {
            GameManager.elevationMaxAngle = MAXANGLE;
        }
        Debug.Log("ELEV: The max angle is now" + GameManager.elevationMaxAngle);
    }
    public void Step2Input()
    {
        Debug.Log("ELEV: The step2 is now" + Step2.text);
        GameManager.step2 = int.Parse(Step2.text);


    }
}
