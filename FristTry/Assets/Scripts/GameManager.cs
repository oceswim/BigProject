using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public GameObject notInputEverything, noModels;
    private void Awake()
    {
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
        /* if(SliderValue >= 0 && objMinDist > 0 && objMaxDist > 0 && step1 > 0 && elevationMinAngle >= 0 &&
             elevationMaxAngle > 0 && step2 > 0 && azimuthMinAngle >= 0 && azimuthMaxAngle > 0 && step3 > 0 &&
             objRotMinAngle >= 0 && objRotMaxAngle > 0 && step4 > 0 && minNoObj > 0 && maxNoObj > 0
             && projectName.Length>0 && SceneSelected.Length>0 && models.Count>0)*/
        if (models.Count > 0)
        {
            Debug.Log("changes: " + changes);
            if (changes == 14)
            {
                Debug.Log("Switching scenes...");
                LoadScene(SceneSelected);
                if (!Directory.Exists("Images/" + projectName))
               {
                    //if it doesn't, create it
                    Directory.CreateDirectory("Images/" + projectName); // returns a DirectoryInfo object

                }
                else
                {
                  string[] dirs =  Directory.GetDirectories("Images/");
                    int count = 0;
                   foreach( string s in dirs)
                    {
                        if (s.Contains("Images/" + projectName))
                        {
                            count++;
                        }
                    }
                    Directory.CreateDirectory("Images/" + projectName +"("+ count.ToString()+")");
                }
               

            }
            else
            {
                notInputEverything.SetActive(true);
                Debug.Log("Something's not right...");
            }
        }
        else
        {
            noModels.SetActive(true);
            Debug.Log("choose at least one model...");
        }
    }
    public void ProceedWithDefault()
    {
        Debug.Log("Switching scenes...");
        LoadScene(SceneSelected);
        if (!Directory.Exists("Images/" + projectName))
        {
            //if it doesn't, create it
            Directory.CreateDirectory("Images/" + projectName); // returns a DirectoryInfo object

        }
        else
        {
            string[] dirs = Directory.GetDirectories("Images/");
            int count = 0;
            foreach (string s in dirs)
            {
                if (s.Equals("Images/" + projectName))
                {
                    count++;
                }
            }
            Directory.CreateDirectory("Images/" + projectName+"("+count.ToString()+")");
        }


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
