using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HealthBarController : MonoBehaviour
{
    private Slider barFill;
    private TextMeshProUGUI textValue;
    private Image barColor;

    public Color lowColor;
    public Color highColor;

    private float colorAux = 1.5f;

    private float sliderSpeed = 4.5f;

    private float currentValue;

    private float targetValue;
    void Awake()
    {
        barFill = GetComponentInChildren<Slider>();
        textValue = GetComponentInChildren<TextMeshProUGUI>();
        barColor = barFill.GetComponentInChildren<Image>();
        currentValue = barFill.value;
        targetValue = currentValue;
    }


    private IEnumerator UpdateBar()
    {
        while(Math.Abs(currentValue - targetValue) > 0.005f)
        {
            barColor.color = Color.Lerp(lowColor, highColor, barFill.value / colorAux);
            currentValue = Mathf.Lerp(currentValue, targetValue, sliderSpeed * Time.deltaTime);
            barFill.value = currentValue;

            yield return null;
        }

    }
    public void ChangeValue(int currValue, int maxValue)
    {
        targetValue = (float)currValue / maxValue;
        textValue.text = currValue.ToString() + " / " + maxValue.ToString();
        barColor.color = Color.Lerp(lowColor, highColor, barFill.value / colorAux);

        StartCoroutine(nameof(UpdateBar));
    }
}
