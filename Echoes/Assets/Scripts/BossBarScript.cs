using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBarScript : MonoBehaviour
{
    public Slider slider;

    public void SetSliderValue(float value)
    {
        slider.value = value;
    }

    public void SetSliderMaxValue(float maxValue)
    {
        slider.maxValue = maxValue;
        slider.value = maxValue;
    }

}
