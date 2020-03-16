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
    private int index,sceneInd;
    public Camera Camera1, Camera2, Camera3;
    private Transform T1, T2, T3;
    public Shader effectShader;
    public Slider slider1, slider2, slider3;
    public TMP_Text text1, text2, text3;
    public GameObject panel2; //panel1
    private Transform[] models;
    public Transform spawners;
    private string path, path2, path3;
    // Start is called before the first frame update
    void Start()
    {

        models = new Transform[GameManager.models.Count];
        for (int i = 0; i < models.Length; i++)
        {
            Transform parent = spawners.GetChild(i);
            models[i] = parent.GetChild(0);
        }
        SetSegmentationEffect();
        Camera3.renderingPath = RenderingPath.Forward;
        SetUpCameraWithReplacementShader(0, Color.gray, Camera3);

        Camera2.renderingPath = RenderingPath.Forward;
        SetUpCameraWithReplacementShader(2, Color.white, Camera2);
       
        T1 = Camera1.transform;
        T2 = Camera2.transform;
        T3 = Camera3.transform;
        Camera2.enabled=false;
        Camera3.enabled =false;
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
        
        sceneInd = SceneManager.GetActiveScene().buildIndex;
        path = Application.dataPath + "/Projects/" + GameManager.projectName + "/NormalImages/";
        path2 = Application.dataPath + "/Projects/" + GameManager.projectName + "/DepthImages/";
        path3 = Application.dataPath + "/Projects/" + GameManager.projectName + "/GroundTruthImages/";

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
                        //Debug.Log("D : " + x + "Elev : " + c + "Azim : " + v + "Obj rot : " + w);
                        foreach (Transform t in models)
                        {
                            Vector3 temp2 = t.rotation.eulerAngles;
                            temp2.y = w;
                            t.rotation = Quaternion.Euler(temp2);

                        }
                      
                        string imageName ="Light" + GameManager.SliderValue + "_D" + x + "_Elev" + c + "_Azim" + v + "_ObjRot" + w + "_Scene" + sceneInd +"_"+index+ ".png";

                        
                        T1.LookAt(target);
                        ScreenCapture.CaptureScreenshot(Path.Combine(path + imageName));
                        Debug.Log(index + "_C1:" + path + "" + imageName);
                        yield return new WaitForSeconds(.05f);
                        Camera1.enabled=false;
                        Camera2.enabled=true;
                        T2.LookAt(target);

                        ScreenCapture.CaptureScreenshot(Path.Combine(path2 + imageName));
                        Debug.Log(index + "_C2:" + path2 + "" + imageName);
                        yield return new WaitForSeconds(.05f);
                        Camera2.enabled=false;
                        Camera3.enabled=true;
                        T3.LookAt(target);

                        ScreenCapture.CaptureScreenshot(Path.Combine(path3 + imageName));
                        Debug.Log(index + "_C3:" + path3 + "" + imageName);
                        yield return new WaitForSeconds(.05f);
                        Camera3.enabled=false;
                        Camera1.enabled=true;
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
            case "Audi":            //1
                theId = 105441;
                break;
            case "Bmwz4":           //2
                theId = 95662;
                break;
            case "Mini":            //3
                theId = 85443;
                break;
            case "Golf":            //4
                theId = 75444;
                break;
            case "Ambulance":       //5
                theId = 65445;
                break;
            case "Kangoo":          //6
                theId = 55446;
                break;
            case "Combi":           //7
                theId = 45445;
                break;
            case "Minivan":         //8 
                theId = 35446;
                break;
            case "dodge":           //9
                theId = 25445;
                break;
            case "Ferrari":         //10
                theId = 15446;
                break;
            case "McLaren":         //11
                theId = 67445;
                break;
            case "Porsche":         //12
                theId = 57446;
                break;
            case "Bus":         //13
                theId = 47445;
                break;
            case "london_bus":         //14
                theId = 37446;
                break;
            case "School_bus":         //15
                theId = 27445;
                break;
            case "Tourist_bus":         //16
                theId = 17446;
                break;
            case "Cessna":         //17
                theId = 98445;
                break;
            case "Hawker":         //18
                theId = 88446;
                break;
            case "Learjet":         //19
                theId = 78445;
                break;
            case "Breguet14":         //20 
                theId = 68446;
                break;
            case "B2_Spirit":         //21 
                theId = 58446;
                break;
            case "F35":           //22
                theId = 48445;
                break;
            case "Mig_29":         //23
                theId = 38446;
                break;
            case "Eurofighter":         //24 
                theId = 28445;
                break;
            case "AH-64D":         //25
                theId = 18446;
                break;
            case "Huey":         //26
                theId = 110445;
                break;
            case "Ka-50":         //27
                theId = 120446;
                break;
            case "Mi_24":         //28
                theId = 130445;
                break;
            case "A380":         //29
                theId = 155446;
                break;
            case "B747":         //30
                theId = 165445;
                break;
            case "Concord":         //31
                theId = 150446;
                break;
            case "A757":         //32 
                theId = 160445;
                break;
            default:        
                theId = 999999;
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
}
