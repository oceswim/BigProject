using System.Collections;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private int azimAngle, elevationAngle,distance,increment;
    private Vector3 target = new Vector3(0, 0, 0);
    private int resWidth = 256;
    private int resHeight = 256;
    public Camera snapCam;
    int index;
    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        increment = GameManager.step3;
        azimAngle = GameManager.azimuthMaxAngle;
        elevationAngle = GameManager.elevationMaxAngle;
        distance = GameManager.objMaxDist;
        transform.position = new Vector3(transform.position.x, transform.position.y,distance);
        transform.Rotate(0, elevationAngle, 0);
        StartCoroutine(TakePicture());
    }

    // Update is called once per frame
    private IEnumerator TakePicture()
    {
        transform.RotateAround(target, Vector3.up, increment);

        snapCam.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
    }
    void LateUpdate()
    {
        if (snapCam.gameObject.activeInHierarchy)
        {
            Texture2D snapshot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            snapCam.Render();
            RenderTexture.active = snapCam.targetTexture;
            snapshot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            byte[] bytes = snapshot.EncodeToPNG();
            string fileName = "Images/" + GameManager.projectName + "/img" + index;
            index++;
            System.IO.File.WriteAllBytes(fileName, bytes);
            Debug.Log("snapshot taken");
            snapCam.gameObject.SetActive(false);
        }
        
    }
}
