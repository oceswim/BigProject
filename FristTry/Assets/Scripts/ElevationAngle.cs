using UnityEngine;
using TMPro;
public class ElevationAngle : MonoBehaviour
{
    private const int MAXANGLE = 90;
    public TMP_InputField MinAngle, MaxAngle, Step2;
    private bool input1, input2, input3;
    // Start is called before the first frame update
    private void Start()
    {
        input1 = input2 = input3 = false;
        GameManager.elevationMinAngle = 0;
        GameManager.elevationMaxAngle = MAXANGLE;
        GameManager.step2 = 5;
    }
    public void MinAngleInput()
    {
        if (MinAngle.text.Length > 0)
        {
            if (!input1)
            {
                input1 = true;
                GameManager.changes++;
            }
            Debug.Log("ELEV: The min angle elev is now" + MinAngle.text);
            GameManager.elevationMinAngle = int.Parse(MinAngle.text);
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
    public void MaxAngleInput()
    {
        if (MaxAngle.text.Length > 0)
        {
            if (!input2)
            {
                input2 = true;
                GameManager.changes++;
            }
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
        else
        {
            if (input2)
            {
                input2 = false;
                GameManager.changes--;
            }
        }

    }
    public void Step2Input()
    {
        if (Step2.text.Length > 0)
        {
            if (!input3)
            {
                input3 = true;
                GameManager.changes++;
            }
            Debug.Log("ELEV: The step2 is now" + Step2.text);
            int ste2 = int.Parse(Step2.text);
            if (ste2 <= MAXANGLE)
            {
                GameManager.step2 = int.Parse(Step2.text);
            }
            else
            {
                GameManager.step2 = 5;
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
