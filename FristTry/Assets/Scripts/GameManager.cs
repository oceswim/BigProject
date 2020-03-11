using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public static int SliderValue,
        objMinDist, objMaxDist, step1,
        elevationMinAngle, elevationMaxAngle, step2,
        azimuthMinAngle, azimuthMaxAngle, step3,
        objRotMinAngle, objRotMaxAngle, step4,
        minNoObj, maxNoObj;
    public static string projectName,SceneSelected;
    public static List<string> models;
    public static int changes;
    public bool notAllInput, noModels;
    public bool change;
    private void Awake()
    {
        noModels = false;
        notAllInput = false;
        
        SliderValue = objMinDist = objMaxDist = step1 = elevationMinAngle =
            elevationMaxAngle = step2 = azimuthMinAngle = azimuthMaxAngle = step3 =
            objRotMinAngle = objRotMaxAngle = step4 = minNoObj = maxNoObj = 0;
        projectName = SceneSelected = "";
        models = new List<string>();
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. Enforces singleton pattern: here can only ever be one instance of a GameManager.
            Destroy(gameObject);

         DontDestroyOnLoad(this);
    }
    
    public void StartProcess()
    {
   
        if (models.Count > 0)
        {
            
            if (changes == 14)
            {
                Debug.Log("Switching scenes...");
                LoadScene(SceneSelected);
                if (!Directory.Exists(Application.dataPath + "/Projects/" + projectName))
               {
                    //if it doesn't, create it
                    Directory.CreateDirectory(Application.dataPath + "/Projects/" + projectName); // returns a DirectoryInfo object
                    CreateFolders();

                }
                else
                {
                  string[] dirs =  Directory.GetDirectories(Application.dataPath + "/Projects/");
                    int count = 0;
                   foreach( string s in dirs)
                    {
                        if (s.Contains(Application.dataPath + "/Projects/" + projectName))
                        {
                            count++;
                        }
                    }
                    projectName = projectName + "(" + count.ToString() + ")";
                    Directory.CreateDirectory(Application.dataPath + "/Projects/" + projectName);
                    CreateFolders();
                }
               

            }
            else
            {
                notAllInput = true;
                Debug.Log("Something's not right...");
            }
        }
        else
        {
            noModels = true;
            Debug.Log("choose at least one model...");
        }
    }
    public void ProceedWithDefault()
    {
        Debug.Log("Switching scenes...");
        LoadScene(SceneSelected);
        if (!Directory.Exists(Application.dataPath + "/Projects/" + projectName))
        {
            //if it doesn't, create it
            Directory.CreateDirectory(Application.dataPath + "/Projects/" + projectName); // returns a DirectoryInfo object
            CreateFolders();
        }
        else
        {
            string[] dirs = Directory.GetDirectories(Application.dataPath + "/Projects/");
            int count = 0;
            foreach (string s in dirs)
            {
                if (s.Equals(Application.dataPath + "/Projects/" + projectName))
                {
                    count++;
                }
            }
            projectName = projectName + "(" + count.ToString() + ")";
            Directory.CreateDirectory(Application.dataPath + "/Projects/" + projectName); 
            CreateFolders();
        }


    }
    public void Reset()
    {
        SliderValue = objMinDist = objMaxDist = step1 = elevationMinAngle =
            elevationMaxAngle = step2 = azimuthMinAngle = azimuthMaxAngle = step3 =
            objRotMinAngle = objRotMaxAngle = step4 = minNoObj = maxNoObj = 0;
        projectName = SceneSelected = "";
        models = new List<string>();
        changes = 0;

    }
    private void CreateFolders()
    {
        Directory.CreateDirectory(Application.dataPath + "/Projects/" + projectName + "/NormalImages");
        Directory.CreateDirectory(Application.dataPath + "/Projects/" + projectName + "/GroundTruthImages");
        Directory.CreateDirectory(Application.dataPath + "/Projects/" + projectName + "/DepthImages");
    }
   
    public void LoadScene(string theName)
    {
        SceneManager.LoadScene(theName);

    }
    public void ExitApp()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
         Application.OpenURL(webplayerQuitURL);
#else
         Application.Quit();
#endif

    }

}
