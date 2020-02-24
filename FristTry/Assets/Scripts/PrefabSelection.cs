using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PrefabSelection : MonoBehaviour
{
    private string toAdd;
    public TMP_Dropdown objDropDown;
    public TMP_InputField minNoObj, maxNoObj;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void addObjToList()
    {

    }
    public void setMinNoObj()
    {
        Debug.Log("PREF SEL: The min no is now" + minNoObj.text);
    }
    public void setMaxNoObj()
    {
        Debug.Log("PREF SEL: The max no is now" + maxNoObj.text);
    }

}
