using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField]
    Slider sensitivitySlider;

    [SerializeField]
    TMP_Text sensText;

    void Start()
    {
        sensitivitySlider.onValueChanged.AddListener(OnValueChanged);

        sensitivitySlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("Sens", 1));
        sensText.text = PlayerPrefs.GetFloat("Sens", 1).ToString("F2");
    }

    private void OnValueChanged(float arg0)
    {
        PlayerPrefs.SetFloat("Sens", arg0);
        sensText.text = arg0.ToString("F2");
    }
}
