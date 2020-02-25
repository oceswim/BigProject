using UnityEngine;
using TMPro;
public class ObjectRotation : MonoBehaviour
{
    private const int MAXANGLE = 360;
    public TMP_InputField MinAngleObjRot, MaxAngleObjRot, Step4; 
    // Start is called before the first frame update
    public void MinAngleObjRotInput()
    {
        Debug.Log("OBJ ROT: The min angle is now" + MinAngleObjRot.text);
        GameManager.objRotMinAngle = int.Parse(MinAngleObjRot.text);
    }
    public void MaxAngleObjRotInput()
    {
        int newMaxVal = int.Parse(MaxAngleObjRot.text);
       
        if (newMaxVal <= MAXANGLE)
        {
            GameManager.objRotMaxAngle = newMaxVal;
        }
        else
        {
            GameManager.objRotMaxAngle = MAXANGLE;
        }
        Debug.Log("OBJ ROT: The max angle is now" + GameManager.objRotMaxAngle);

    }
    public void Step4Input()
    {
        Debug.Log("OBJ ROT: The step4 is now" + Step4.text);
        GameManager.step4 = int.Parse(Step4.text);


    }
}
