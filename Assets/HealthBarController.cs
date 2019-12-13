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

    public float aux;
    void Start()
    {
        barFill = GetComponentInChildren<Slider>();
        textValue = GetComponentInChildren<TextMeshProUGUI>();
        barColor = barFill.GetComponentInChildren<Image>();
    }
    void Update()
    {
        barColor.color = Color.Lerp(lowColor, highColor, barFill.value / aux);

    }
    public void ChangeValue(int currentValue, int maxValue)
    {
        barFill.value = (float)currentValue / maxValue;
        textValue.text = currentValue.ToString() + " / " + maxValue.ToString();
        barColor.color = Color.Lerp(lowColor, highColor, barFill.value / aux);

    }
}
