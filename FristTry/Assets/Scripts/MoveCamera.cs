using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MoveCamera : MonoBehaviour
{

    private int minDist, maxDist, increment1,
        minElevAngle, maxElevAngle, increment2,
        minAzimAngle, maxAzimAngle, increment3,
        minObjRot, maxObjRot, increment4;
    private Vector3 target = new Vector3(0, 0, 0);
    int index, goodToGo;
    public Camera Camera1, Camera2, Camera3;
    private Transform T1, T2, T3;
    public Shader effectShader;
    private bool change1, change2, change3;
    public Slider slider1, slider2, slider3;
    public TMP_Text text1, text2, text3;
    public GameObject panel2; //panel1
    private bool once;
    private Transform[] models;
    public Transform spawners;
    // Start is called before the first frame update
    void Start()
    {
      
        once = false;

        models = new Transform[GameManager.models.Count];
        for (int i = 0; i < models.Length; i++)
        {
            Transform parent = spawners.GetChild(i);
            models[i] = parent.GetChild(0);
        }
        change1 = change2 = change3 = false;
        SetSegmentationEffect();
        Camera3.renderingPath = RenderingPath.Forward;
        SetUpCameraWithReplacementShader(0, Color.gray, Camera3);

        Camera2.renderingPath = RenderingPath.Forward;
        SetUpCameraWithReplacementShader(2, Color.white, Camera2);

        Camera2.gameObject.SetActive(false);
        Camera3.gameObject.SetActive(false);
        T1 = Camera1.transform;
        T2 = Camera2.transform;
        T3 = Camera3.transform;

        index = 1;
        minObjRot = GameManager.objRotMinAngle;
        maxObjRot = GameManager.objRotMaxAngle;
        increment4 = GameManager.step4;

        minDist = GameManager.objMinDist;
        maxDist = GameManager.objMaxDist;
        increment1 = GameManager.step1;
        minElevAngle = GameManager.elevationMinAngle;
        maxElevAngle = GameManager.elevationMaxAngle;
        increment2 = GameManager.step2;
        minAzimAngle = GameManager.azimuthMinAngle;
        maxAzimAngle = GameManager.azimuthMaxAngle;
        increment3 = GameManager.step3;




        StartCoroutine(RotateAndCapture());

    }
    private IEnumerator RotateAndCapture() // for loop based on elevation angle + azim angle + dist value
    {
        for (int x = minDist; x <= maxDist; x += increment1)
        {

            transform.Translate(0, 0, -x);

            for (int c = minElevAngle; c <= maxElevAngle; c += increment2)
            {

                Vector3 temp = transform.rotation.eulerAngles;
                temp.x = c;
                transform.rotation = Quaternion.Euler(temp);


                for (int v = minAzimAngle; v <= maxAzimAngle; v += increment3)
                {


                    transform.RotateAround(target, new Vector3(0.0f, 1.0f, 0.0f), increment3);


                    for (int w = minObjRot; w <= maxObjRot; w += increment4)
                    {
                        Debug.Log("D : " + x + "Elev : " + c + "Azim : " + v + "Obj rot : " + w);
                        foreach (Transform t in models)
                        {
                            Vector3 temp2 = t.rotation.eulerAngles;
                            temp2.y = w;
                            t.rotation = Quaternion.Euler(temp2);

                        }
                        int sceneInd = SceneManager.GetActiveScene().buildIndex;
                        string imageName = "Img_" + index + "_Light" + GameManager.SliderValue + "_D" + x + "_Elev" + c + "_Azim" + v + "_ObjRot" + w + "_Scene" + sceneInd + ".png";
                        T1.LookAt(target);
                        string path, path2, path3;
                        path = "Images/" + GameManager.projectName + "/GroundTruthImages/";
                        path2 = "Images/" + GameManager.projectName + "/NormalImages/";
                        path3 = "Images/" + GameManager.projectName + "/DepthImages/";
                        yield return new WaitForEndOfFrame();
                        ScreenCapture.CaptureScreenshot(Path.Combine(path + imageName));
                        yield return new WaitForEndOfFrame();
                        Camera1.gameObject.SetActive(false);
                        Camera2.gameObject.SetActive(true);
                        T2.LookAt(target);

                        ScreenCapture.CaptureScreenshot(Path.Combine(path2 + imageName));
                        yield return new WaitForEndOfFrame();
                        Camera2.gameObject.SetActive(false);
                        Camera3.gameObject.SetActive(true);
                        T3.LookAt(target);

                        ScreenCapture.CaptureScreenshot(Path.Combine(path3 + imageName));
                        yield return new WaitForEndOfFrame();
                        Camera3.gameObject.SetActive(false);
                        Camera1.gameObject.SetActive(true);
                        index++;

                    }
                }


            }
        }

        panel2.SetActive(true);
        Debug.Log("YOURE DONE");

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
            mpb.SetColor("_ObjectColor", ColorEncoding.EncodeIDAsColor(id, theTag));
            mpb.SetColor("_CategoryColor", ColorEncoding.EncodeTagAsColor(theTag));//give a color by name
            r.SetPropertyBlock(mpb);

        }
    }
    private int getTheId(string aTag)
    {
        int theId = 0;
        switch (aTag)
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
