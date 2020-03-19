using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DistanceToObject : MonoBehaviour
{
    private const int MAXDIST = 1000;
    public TMP_InputField MinDist, MaxDist, Step;
    private bool input1, input2, input3;
    // Start is called before the first frame update
    private void Start()
    {
        input1 = input2 = input3 = false;
        GameManager.objMinDist = 0;
        GameManager.objMaxDist = 200;
        GameManager.step1 = 50;
    }
    public void MinDistInput()
    {
        if (MinDist.text.Length > 0)
        {
            if (!input1)
            {
                input1 = true;
                GameManager.changes++;
            }
            Debug.Log("DIST OBJ: The min dist is now" + MinDist.text);
            GameManager.objMinDist = int.Parse(MinDist.text);
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
    public void MaxDistInput()
    {
        if (MaxDist.text.Length > 0)
        {
            if (!input2)
            {
                input2 = true;
                GameManager.changes++;
            }
            int newMaxVal = int.Parse(MaxDist.text);
            if (newMaxVal <= MAXDIST)
            {
                GameManager.objMaxDist = newMaxVal;

            }
            else
            {
                GameManager.objMaxDist = MAXDIST;
            }
            Debug.Log("DIST OBJ: The max dist is now" + GameManager.objMaxDist);
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
    public void StepInput()
    {
        if (Step.text.Length > 0)
        {
            if (!input3)
            {
                input3 = true;
                GameManager.changes++;
            }
            Debug.Log("DIST OBJ: The STEP is now" + Step.text);
            int ste1 = int.Parse(Step.text);
            if (ste1 <= MAXDIST && ste1 >0)
            {
                GameManager.step1 = int.Parse(Step.text);
            }
            else
            {
                GameManager.step1 =5;
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
