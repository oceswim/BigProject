using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class PrefabSelection : MonoBehaviour
{
    private string toAdd;
    public TMP_Dropdown objDropDown;
    public TMP_Text minNumText, maxNumText;
    public Slider minNoObj, maxNoObj;
    private List<string> prefabsAdded = new List<string>();
    private bool existing;
    public GameObject listElement,listContent,errorMessage1,errorMessage2;

    // Start is called before the first frame update
    void Start()
    {
        existing = false;
        //minNumText.text = "Min no of Objects: 1";
        //maxNumText.text = "Max no of Objects: 2";
    }

    public void AddObjToList()
    {
        if (prefabsAdded.Count < 5)
        {
            string theValue = objDropDown.options[objDropDown.value].text;

            foreach (string s in prefabsAdded)
            {
                if (s.Equals(theValue))
                {
                    Debug.Log(theValue + " already in list");
                    existing = true;
                    break;
                }
            }
            if (!existing)
            {
                prefabsAdded.Add(theValue);
                GameManager.models.Add(theValue);
                Debug.Log(prefabsAdded.Count);
                GameObject newListElement = Instantiate(listElement);
                newListElement.transform.parent = listContent.transform;
                newListElement.name = theValue;
                newListElement.GetComponent<TMP_Text>().text = prefabsAdded.Count + ") " + theValue;
            }
            else
            {
                errorMessage2.SetActive(true);
                existing = false;//resets the value for next add
            }
        }
        else
        {
            errorMessage1.SetActive(true);
            Debug.Log("max atteint");
        }
    }
    public void RemoveObjFromList()
    {
        if(prefabsAdded.Count>0)
        {
            Debug.Log("destroying "+ prefabsAdded[(prefabsAdded.Count - 1)]);
            string toDel = prefabsAdded[(prefabsAdded.Count - 1)];
            GameObject toDestroy=GameObject.Find("Canvas/MainPanel/PrefabSelection/Texts/Scroll View/Viewport/Content/" +toDel);
            Destroy(toDestroy);
            prefabsAdded.RemoveAt((prefabsAdded.Count - 1));
            GameManager.models.RemoveAt((prefabsAdded.Count - 1));
        }
    }
    public void SetMinNoObj()
    {
        int newMin = (int)minNoObj.value;
        minNumText.text = "Min no of Objects: "+ newMin;
        Debug.Log("PREF SEL: The min no is now" + newMin);
        GameManager.minNoObj = newMin;

    }
    public void SetMaxNoObj()
    {
        int newMax = (int)maxNoObj.value;
        maxNumText.text = "Max no of Objects: " + newMax;
        Debug.Log("PREF SEL: The max no is now" + newMax);
        GameManager.maxNoObj = newMax;

    }
    public void CheckValue()
    {
        Debug.Log(objDropDown.options[objDropDown.value].text + "is selected");
    }

}
