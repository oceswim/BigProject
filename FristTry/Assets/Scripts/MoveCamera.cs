using System.Collections;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private int azimAngle, elevationAngle,distance,increment;
    private Vector3 target = new Vector3(0, 0, 0);
    private int resWidth = 256;
    private int resHeight = 256;
    private Camera snapCam;
    int index;
    // Start is called before the first frame update
    void Start()
    {
        snapCam = GetComponent<Camera>();
        index = 0;
        increment = GameManager.step3;
        azimAngle = GameManager.azimuthMaxAngle;
        elevationAngle = GameManager.elevationMaxAngle;
        distance = GameManager.objMaxDist;
        transform.position = new Vector3(transform.position.x, transform.position.y,distance);
        transform.Rotate(elevationAngle, 0, 0);
        RotateAndCapture();
    }
    private void RotateAndCapture() // for loop based on elevation angle + azim angle + dist value
    {
       
        StartCoroutine(TakePicture());
    }
    // Update is called once per frame
    private IEnumerator TakePicture()
    {
        string path = "Images/" + GameManager.projectName + "/Img" + index.ToString();
        index++;
        ScreenCapture.CaptureScreenshot(path);
        yield return new WaitForEndOfFrame();
    }
    
    
}//for loop from min value to max value avec increment pour i++;
