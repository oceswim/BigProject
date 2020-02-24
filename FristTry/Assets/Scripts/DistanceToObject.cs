using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DistanceToObject : MonoBehaviour
{
    public TMP_InputField MinDist, MaxDist, Step; 
    // Start is called before the first frame update
    public void MinDistInput()
    {
        Debug.Log("DIST OBJ: The min dist is now" + MinDist.text);

    }
    public void MaxDistInput()
    {
        Debug.Log("DIST OBJ: The max dist is now" + MaxDist.text);

    }
    public void StepInput()
    {
        Debug.Log("DIST OBJ: The STEP is now" + Step.text);

    }
}
