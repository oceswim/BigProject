using UnityEngine;

public class SceneSwitch : MonoBehaviour
{
   public void BackToMenu()
    {
        GameManager.instance.Reset();
        GameManager.instance.LoadScene("Menu");
       
    }
}
