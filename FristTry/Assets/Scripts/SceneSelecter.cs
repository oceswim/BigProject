using UnityEngine;
using TMPro;
public class SceneSelecter : MonoBehaviour
{
    public TMP_Dropdown SceneDropDown;
    public TMP_Text selectedScene;
    private string currentSelectedScene;
    // Start is called before the first frame update
    void Start()
    {
        currentSelectedScene = SceneDropDown.options[SceneDropDown.value].text;
        Debug.Log(currentSelectedScene);
        selectedScene.text = "Scene selected: " + currentSelectedScene;
        GameManager.SceneSelected = currentSelectedScene;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  
    public void SelectScene()
    {
        if(!SceneDropDown.options[SceneDropDown.value].text.Equals(currentSelectedScene))
        {
            currentSelectedScene = SceneDropDown.options[SceneDropDown.value].text;
            selectedScene.text = "Scene selected: " + currentSelectedScene;
            GameManager.SceneSelected = currentSelectedScene;
        }
    }
}
