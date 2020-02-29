using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MoveCamera : MonoBehaviour
{
    private int minDist, maxDist, increment1,
        minElevAngle, maxElevAngle, increment2,
        minAzimAngle, maxAzimAngle, increment3;
    private Vector3 target = new Vector3(0, 0, 0);
    int index,goodToGo;
    public Camera Camera1, Camera2, Camera3;
    private Transform T1, T2, T3;
    public Shader effectShader;
    private bool change1, change2, change3;
    // Start is called before the first frame update
    void Start()
    {
        change1 = change2 = change3 = false;
        SetSegmentationEffect();
        Camera3.renderingPath = RenderingPath.Forward;
        SetUpCameraWithReplacementShader(0, Color.gray,Camera3);

        Camera2.renderingPath = RenderingPath.Forward;
        SetUpCameraWithReplacementShader(2, Color.white,Camera2);


        T1 = Camera1.transform;
        T2 = Camera2.transform;
        T3 = Camera3.transform;
        T2.gameObject.SetActive(false);
        T3.gameObject.SetActive(false);
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
        while(goodToGo<3)
        {
            if (minDist < maxDist)
            {
                Debug.Log("Min dist : " + minDist);
                T1.Translate(0, 0, -increment1);
                T2.Translate(0, 0, -increment1);
                T3.Translate(0, 0, -increment1);
                minDist += increment1;
             
            }
            else
            {
                if (!change1)
                {
                    change1= true;
                    goodToGo++;
                    Debug.Log("goodToGo: " + goodToGo);
                }
            }
            if (minElevAngle < maxElevAngle)
            {
                T1.Rotate(increment2, 0, 0);
                T2.Rotate(increment2, 0, 0);
                T3.Rotate(increment2, 0, 0);
                minElevAngle += increment2;
              
            }
            else
            {
                if (!change2)
                {
                    change2= true;
                    goodToGo++;
                    Debug.Log("goodToGo: " + goodToGo);
                }
            }
            if (minAzimAngle < maxAzimAngle)
            {
                Debug.Log("Azim angle :" + minAzimAngle);
                T1.RotateAround(target, new Vector3(0.0f, 1.0f, 0.0f), increment3);
                T2.RotateAround(target, new Vector3(0.0f, 1.0f, 0.0f), increment3);
                T3.RotateAround(target, new Vector3(0.0f, 1.0f, 0.0f), increment3);
                minAzimAngle += increment3;
              
            }
            else
            {
                if (!change3)
                {
                    change3 = true;
                    goodToGo++;
                    Debug.Log("goodToGo: " + goodToGo);
                }
            }
            T1.LookAt(target);
            string path = "Images/" + GameManager.projectName + "/GroundTruthImages/Img" + index.ToString()+".png";
            ScreenCapture.CaptureScreenshot(path);
            yield return new WaitForEndOfFrame();
            T1.gameObject.SetActive(false);
            T2.gameObject.SetActive(true);
            T2.LookAt(target);
            string path2 = "Images/" + GameManager.projectName + "/NormalImages/Img" + index.ToString()+".png";
            ScreenCapture.CaptureScreenshot(path2);
            yield return new WaitForEndOfFrame();
            T2.gameObject.SetActive(false);
            T3.gameObject.SetActive(true);
            T3.LookAt(target);
            string path3 = "Images/" + GameManager.projectName + "/DepthImages/Img" + index.ToString()+".png";
            ScreenCapture.CaptureScreenshot(path3);
            yield return new WaitForEndOfFrame();
            T3.gameObject.SetActive(false);
            T1.gameObject.SetActive(true);
            index++;
  
        }

        if(goodToGo==3)
        {
            Debug.Log("YOURE DONE");
        }
    }

    private void SetSegmentationEffect()
    {
        var renderers = FindObjectsOfType<Renderer>();
        var mpb = new MaterialPropertyBlock();
        foreach (var r in renderers)
        {
            var id = Math.Abs(r.gameObject.GetInstanceID());
            var layer = r.gameObject.layer;
            Debug.Log("object name :" + r.gameObject.name+ "object ID :"+ id);
            mpb.SetColor("_ObjectColor", ColorEncoding.EncodeIDAsColor(id));
            mpb.SetColor("_CategoryColor", ColorEncoding.EncodeLayerAsColor(layer));//give a color by name
            r.SetPropertyBlock(mpb);
        }
    }

    private void SetUpCameraWithReplacementShader(int mode, Color clearColor, Camera cam)
    {
        var cb = new CommandBuffer();
        cb.SetGlobalFloat("_OutputMode", (int)mode);
        cam.AddCommandBuffer(CameraEvent.BeforeForwardOpaque, cb);
        cam.AddCommandBuffer(CameraEvent.BeforeFinalPass, cb);
        cam.SetReplacementShader(effectShader, "");
        cam.backgroundColor = clearColor;
        cam.clearFlags = CameraClearFlags.SolidColor;
    }
}//for loop from min value to max value avec increment pour i++;

