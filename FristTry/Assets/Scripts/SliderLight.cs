using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderLight : MonoBehaviour
{
    public TMP_Text sliderValue;
    public Slider mySlider;
    private bool input1;
    // Start is called before the first frame update
    private void Start()
    {
        input1 = false;
        GameManager.SliderValue = 90;
        sliderValue.text = mySlider.value.ToString();
    }

    public void SliderValueUpdated()
    {
        if (!input1)
        {
            input1= true;
            GameManager.changes++;
        }
        sliderValue.text = mySlider.value.ToString();
        GameManager.SliderValue = (int)mySlider.value;
    }
}


