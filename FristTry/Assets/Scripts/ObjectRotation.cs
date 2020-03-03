using UnityEngine;
using TMPro;
public class ObjectRotation : MonoBehaviour
{
    private const int MAXANGLE = 360;
    public TMP_InputField MinAngleObjRot, MaxAngleObjRot, Step4;
    private bool input1, input2, input3;
    // Start is called before the first frame update
    private void Start()
    {
        input1 = input2 = input3 = false;
        GameManager.objRotMinAngle = 0;
        GameManager.objRotMaxAngle = 360;
        GameManager.step4 = 45;

    }
    public void MinAngleObjRotInput()
    {
        if (MinAngleObjRot.text.Length > 0)
        {
            if (!input1)
            {
                input1 = true;
                GameManager.changes++;
            }
            Debug.Log("OBJ ROT: The min angle is now" + MinAngleObjRot.text);
            GameManager.objRotMinAngle = int.Parse(MinAngleObjRot.text);
        }
        else
        {
            if (input1)
            {
                input1 = false;
                GameManager.changes--;
            }
        }
    }
    public void MaxAngleObjRotInput()
    {
        if (MaxAngleObjRot.text.Length > 0)
        {
            if (!input2)
            {
                input2 = true;
                GameManager.changes++;
            }
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
        else
        {
            if (input2)
            {
                input2 = false;
                GameManager.changes--;
            }
        }

    }
    public void Step4Input()
    {
        if (Step4.text.Length > 0)
        {

            if (!input3)
            {
                input3 = true;
                GameManager.changes++;
            }
            Debug.Log("OBJ ROT: The step4 is now" + Step4.text);
            int ste4 = int.Parse(Step4.text);
            if (ste4 <= MAXANGLE)
            {
                GameManager.step4 = int.Parse(Step4.text);
            }
            else
            {
                GameManager.step4 = 45;
            }
        }
        else
        {
            if (input3)
            {
                input3 = false;
                GameManager.changes--;
            }
        }
    }
}
