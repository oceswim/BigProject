using System.Collections;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    private int increment, MinRotationValue, MaxRotationValue, index;
    // Start is called before the first frame update
    void Start()
    {
        increment = GameManager.step4;
    }
    private void Update()
    {
        
    }

    // Update is called once per frame
    public void Rotate()
    {
      
       
        transform.Rotate(0, increment, 0);
        

        
        
    }
}
