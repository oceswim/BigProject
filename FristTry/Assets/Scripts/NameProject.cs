using UnityEngine;
using TMPro;
public class NameProject : MonoBehaviour
{
    public TMP_InputField projectName;
    private bool input1;
    // Start is called before the first frame update
    private void Start()
    {
        input1 = false;
        GameManager.projectName = "DefaultProjectName";
    }
    public void NameTheProject()
    {
        if (projectName.text.Length > 0)
        {
            if (!input1)
            {
                input1 = true;
                GameManager.changes++;
            }
            Debug.Log("PROJECT NAME: the project name is now" + projectName.text);
            GameManager.projectName = projectName.text;
        }
        else
        {
            if (input1)
            {
                input1 = false;
                GameManager.changes--;
            }
        }
    }
}
