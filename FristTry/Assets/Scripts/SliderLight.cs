using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderLight : MonoBehaviour
{
    public TMP_Text sliderValue;
    public Slider mySlider;
    // Start is called before the first frame update
    private void Start()
    {
        sliderValue.text = mySlider.value.ToString();
    }

    public void SliderValueUpdated()
    {
       sliderValue.text = mySlider.value.ToString();
        GameManager.SliderValue = (int)mySlider.value;
    }
}


