using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public static int SliderValue,
        objMinDist, objMaxDist, step1,
        elevationMinAngle, elevationMaxAngle, step2,
        azimuthMinAngle, azimuthMaxAngle, step3,
        objRotMinAngle, objRotMaxAngle, step4,
        minNoObj, maxNoObj;
    public static string projectName;
    private void Awake()
    {
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. Enforces singleton pattern: here can only ever be one instance of a GameManager.
            Destroy(gameObject);

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
