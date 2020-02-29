using System.Collections;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    private int increment, MinRotationValue, MaxRotationValue, index;
    // Start is called before the first frame update
    void Start()
    {
        //increment = GameManager.step4;
        //MinRotationValue = GameManager.objRotMinAngle;
        //MaxRotationValue = GameManager.objRotMaxAngle;
        increment = 45;
        MinRotationValue = 0;
        MaxRotationValue = 360;
        index = MinRotationValue;
        StartCoroutine(Rotate());
    }

    // Update is called once per frame
    private IEnumerator Rotate()
    {
        Debug.Log("rotating" + index);
        while(index<MaxRotationValue)
        {
            transform.Rotate(0, increment, 0);
            yield return new WaitForSeconds(.5f);
            index += increment;
        }
        
    }
}
