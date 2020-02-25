using UnityEngine;
using TMPro;
public class NameProject : MonoBehaviour
{
    public TMP_InputField projectName;
    // Start is called before the first frame update
    
    public void NameTheProject()
    {
        Debug.Log("PROJECT NAME: the project name is now" + projectName.text);
        GameManager.projectName = projectName.text;
    }
}
