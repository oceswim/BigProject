using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ElevationAngle : MonoBehaviour
{
    public TMP_InputField MinAngle, MaxAngle, Step2; 
    // Start is called before the first frame update
    public void MinAngleInput()
    {
        Debug.Log("ELEV: The min angle elev is now" + MinAngle.text);

    }
    public void MaxAngleInput()
    {
        Debug.Log("ELEV: The max angle is now" + MaxAngle.text);

    }
    public void Step2Input()
    {
        Debug.Log("ELEV: The step2 is now" + Step2.text);

    }
}
