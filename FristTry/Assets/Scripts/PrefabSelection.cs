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
    //private List<string> prefabsAdded = new List<string>();
    private List<int> indexPrefabsAdded = new List<int>();
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
        if (indexPrefabsAdded.Count < 5)
        {
            string theValue = objDropDown.options[objDropDown.value].text;
            int indexOfObject = objDropDown.value;
            foreach (int s in indexPrefabsAdded)
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
                //prefabsAdded.Add(theValue);
                indexPrefabsAdded.Add(indexOfObject);
                GameManager.models.Add(indexOfObject);
                Debug.Log(indexPrefabsAdded.Count);
                GameObject newListElement = Instantiate(listElement);
                newListElement.transform.parent = listContent.transform;
                newListElement.name = indexOfObject.ToString();
                newListElement.GetComponent<TMP_Text>().text = indexPrefabsAdded.Count + ") " + theValue;
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
        if(indexPrefabsAdded.Count>0)
        {
            Debug.Log("destroying "+ indexPrefabsAdded[(indexPrefabsAdded.Count - 1)]);
            string toDel = indexPrefabsAdded[(indexPrefabsAdded.Count - 1)].ToString();
            GameObject toDestroy=GameObject.Find("Canvas/MainPanel/PrefabSelection/Texts/Scroll View/Viewport/Content/" +toDel);
            Destroy(toDestroy);
            indexPrefabsAdded.RemoveAt((indexPrefabsAdded.Count - 1));
            GameManager.models.RemoveAt((indexPrefabsAdded.Count - 1));
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
