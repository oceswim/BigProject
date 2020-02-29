using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;
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
    public Slider slider1, slider2, slider3;
    public TMP_Text text1, text2, text3;
    public GameObject panel2; //panel1
    private bool once;
    // Start is called before the first frame update
    void Start()
    {
        once = false;


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


        /*slider1.minValue = minDist;
        slider1.maxValue = maxDist;
        
        slider2.minValue = minAzimAngle;
        slider2.maxValue = maxAzimAngle;

        slider3.minValue = minElevAngle;
        slider3.maxValue = maxElevAngle;

        text1.text = text2.text = text3.text = "0%";*/

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
                /*slider1.value = minDist;
                float percentage = (float)Math.Round(((float)minDist / (float)maxDist) * 100);
                text1.text = percentage.ToString() + "%";*/
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

                /*slider3.value = minElevAngle;
                float percentage = (float)Math.Round(((float)minElevAngle / (float)maxElevAngle) * 100);
                text3.text = percentage.ToString() + "%";*/

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

               /*slider2.value = minAzimAngle;
                float percentage = (float)Math.Round(((float)minAzimAngle / (float)maxAzimAngle) * 100);
                text2.text = percentage.ToString() + "%";*/

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
           // panel1.SetActive(false);
            panel2.SetActive(true);
            Debug.Log("YOURE DONE");
        }
    }

    private void SetSegmentationEffect()
    {
        var renderers = FindObjectsOfType<Renderer>();
        var mpb = new MaterialPropertyBlock();
        foreach (var r in renderers)
        {
            //var id = Math.Abs(r.gameObject.GetInstanceID());
            var theTag = r.gameObject.tag;
            var id = getTheId(theTag);
            Debug.Log("object name :" + theTag + "object ID :" + id);
            mpb.SetColor("_ObjectColor", ColorEncoding.EncodeIDAsColor(id,theTag));
            mpb.SetColor("_CategoryColor", ColorEncoding.EncodeTagAsColor(theTag));//give a color by name
            r.SetPropertyBlock(mpb);
            
        }
    }
    private int getTheId(string aTag)
    {
        int theId = 0;
        switch(aTag)
        {
            case "AirbusA310":
                theId = 105441;
                break;
            case "Bus":
                theId = 95442;
                break;
            case "Bicycle":
                theId = 85443;
                break;
            case "Car":
                theId = 75444;
                break;
            case "Jet":
                theId = 65445;
                break;
            case "Motor":
                theId = 55446;
                break;
            default:
                theId = 45440;
                break;
        }

        return theId;
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

