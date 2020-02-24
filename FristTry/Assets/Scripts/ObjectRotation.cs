using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ObjectRotation : MonoBehaviour
{
    public TMP_InputField MinAngleObjRot, MaxAngleObjRot, Step4; 
    // Start is called before the first frame update
    public void MinAngleObjRotInput()
    {
        Debug.Log("OBJ ROT: The min angle is now" + MinAngleObjRot.text);
    }
    public void MaxAngleObjRotInput()
    {
        Debug.Log("OBJ ROT: The max angle is now" + MaxAngleObjRot.text);

    }
    public void Step4Input()
    {
        Debug.Log("OBJ ROT: The step4 is now" + Step4.text);

    }
}
