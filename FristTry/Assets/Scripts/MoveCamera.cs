using System.Collections;
using System.IO;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private int minDist, maxDist, increment1,
        minElevAngle, maxElevAngle, increment2,
        minAzimAngle, maxAzimAngle, increment3;
    private Vector3 target = new Vector3(0, 0, 0);
    int index,goodToGo;

    // Start is called before the first frame update
    void Start()
    {
        index =goodToGo= 0;
        minDist = GameManager.objMinDist;
        maxDist = GameManager.objMaxDist;
        increment1 = GameManager.step1;
        minElevAngle = GameManager.elevationMinAngle;
        maxElevAngle = GameManager.elevationMaxAngle;
        increment2 = GameManager.step2;
        minAzimAngle = GameManager.azimuthMinAngle;
        maxAzimAngle = GameManager.azimuthMaxAngle;
        increment3 = GameManager.step3;
        Directory.CreateDirectory("Images/" + GameManager.projectName+"/NormalImages");
        Directory.CreateDirectory("Images/" + GameManager.projectName + "/GroundTruthImages");
        Directory.CreateDirectory("Images/" + GameManager.projectName + "/DepthImages");
        StartCoroutine(RotateAndCapture());
    }
    private IEnumerator RotateAndCapture() // for loop based on elevation angle + azim angle + dist value
    {
        while(goodToGo<2)
        {
            if (minDist < maxDist)
            {
                Debug.Log("Min dist : " + minDist);
                transform.Translate(0, 0, -increment1);
                Debug.Log("Position: " + transform.position);
                minDist += increment1;
             
            }
            if (minElevAngle < maxElevAngle)
            {
                transform.Rotate(increment2, 0, 0);
                minElevAngle += increment2;
              
            }
            if (minAzimAngle < maxAzimAngle)
            {
                Debug.Log("Azim angle :" + minAzimAngle);
                transform.RotateAround(target, new Vector3(0.0f, 1.0f, 0.0f), increment3);
                minAzimAngle += increment3;
              
            }
            transform.LookAt(target);
           
            string path = "Images/" + GameManager.projectName + "/NormalImages/Img" + index.ToString();
            index++;
            ScreenCapture.CaptureScreenshot(path);
            yield return new WaitForEndOfFrame();

             if (minDist > maxDist)
             {
                 goodToGo++;
             }
            if (minElevAngle > maxElevAngle)
             {
                 goodToGo++;
             }
            if (minAzimAngle > maxAzimAngle)
            {
                goodToGo++;
            }

        }

        if(goodToGo==3)
        {
            Debug.Log("YOURE DONE");
        }
    }

    
}//for loop from min value to max value avec increment pour i++;
