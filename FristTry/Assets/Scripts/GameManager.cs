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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartProcess()
    {
        if(SliderValue >= 0 && objMinDist > 0 && objMaxDist > 0 && step1 > 0 && elevationMinAngle >= 0 &&
            elevationMaxAngle > 0 && step2 > 0 && azimuthMinAngle >= 0 && azimuthMaxAngle > 0 && step3 > 0 &&
            objRotMinAngle >= 0 && objRotMaxAngle > 0 && step4 > 0 && minNoObj > 0 && maxNoObj > 0
            && projectName.Length>0 && SceneSelected.Length>0 && models.Count>0)
        {
            Debug.Log("Switching scenes...");
            LoadScene(SceneSelected);
            Directory.CreateDirectory("Images/"+projectName); // returns a DirectoryInfo object

        }
        else
        {
            Debug.Log("Something's not right...");
        }
    }
    private void LoadScene(string theName)
    {
        SceneManager.LoadScene(theName);

    }
    
}
