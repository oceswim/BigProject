using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public GameObject inputError, modelsError;
    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.notAllInput == true)
        {
            GameManager.instance.notAllInput = false;
            inputError.SetActive(true);
        }
        if(GameManager.instance.noModels == true)
        {
            GameManager.instance.noModels = false;
            modelsError.SetActive(true);
        }
    }

    public void GoWithDefault()
    {
        GameManager.instance.ProceedWithDefault();
    }

    public void StartProcess()
    {
        GameManager.instance.StartProcess();
    }

    public void Exit()
    {
        GameManager.instance.ExitApp();
    }
}
