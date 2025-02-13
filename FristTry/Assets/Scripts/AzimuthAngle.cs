﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AzimuthAngle : MonoBehaviour
{
    private const int MAXANGLE = 360;
    public TMP_InputField MinAngleAzim, MaxAngleAzim, Step3;
    private bool input1, input2, input3;
    // Start is called before the first frame update
    private void Start()
    {
        input1 = input2 = input3 = false;
        GameManager.azimuthMinAngle = 0;
        GameManager.azimuthMaxAngle = 360;
        GameManager.step3 = 60;
    }
    public void MinAngleAzimInput()
    {
        if (MinAngleAzim.text.Length > 0)
        {
            if (!input1)
            {
                input1 = true;
                GameManager.changes++;
            }
            Debug.Log("AZIM ANGLE: The min angle is now" + MinAngleAzim.text);
            GameManager.azimuthMinAngle = int.Parse(MinAngleAzim.text);
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
    public void MaxAngleAzimInput()
    {
        if (MaxAngleAzim.text.Length > 0)
        {
            if (!input2)
            {
                input2 = true;
                GameManager.changes++;
            }
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
        else
        {
            if (input2)
            {
                input2 = false;
                GameManager.changes--;
            }
        }

    }
    public void Step3Input()
    {
        if (Step3.text.Length > 0)
        {
            if (!input3)
            {
                input3 = true;
                GameManager.changes++;
            }
            Debug.Log("AZIM ANGLE: The step3 is now" + Step3.text);
            int ste3 = int.Parse(Step3.text);
            if (ste3 <= MAXANGLE & ste3>0)
            {
                GameManager.step3 = int.Parse(Step3.text);
            }
            else
            {
                GameManager.step3 = 5;
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
