using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DistanceToObject : MonoBehaviour
{
    private const int MAXDIST = 1000;
    public TMP_InputField MinDist, MaxDist, Step; 
    // Start is called before the first frame update
    public void MinDistInput()
    {
        Debug.Log("DIST OBJ: The min dist is now" + MinDist.text);
        GameManager.objMinDist = int.Parse(MinDist.text);
    }
    public void MaxDistInput()
    {
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
    public void StepInput()
    {
        Debug.Log("DIST OBJ: The STEP is now" + Step.text);
        GameManager.step1 = int.Parse(Step.text);

    }
}
