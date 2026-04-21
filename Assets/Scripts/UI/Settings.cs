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

    [SerializeField]
    Toggle invertY;

    [SerializeField]
    Toggle invertX;

    void Start()
    {
        sensitivitySlider.onValueChanged.AddListener(OnValueChanged);

        invertY.onValueChanged.AddListener(OnInvertYChanged);
        invertX.onValueChanged.AddListener(OnInvertXChanged);

        UpdateUI();
    }

    private void UpdateUI()
    {
        sensitivitySlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("Sens", 1));
        sensText.text = PlayerPrefs.GetFloat("Sens", 1).ToString("F2");
        invertY.isOn = PlayerPrefs.GetInt("InvertY", 0) == 1;
        invertX.isOn = PlayerPrefs.GetInt("InvertX", 0) == 1;
    }

    private void OnValueChanged(float arg0)
    {
        PlayerPrefs.SetFloat("Sens", arg0);
        sensText.text = arg0.ToString("F2");

        PlayerPrefs.Save();

    }

    private void OnInvertYChanged(bool arg0)
    {
        PlayerPrefs.SetInt("InvertY", arg0 ? 1 : 0);
        invertY.isOn = arg0;

        PlayerPrefs.Save();
    }

    private void OnInvertXChanged(bool arg0)
    {
        PlayerPrefs.SetInt("InvertX", arg0 ? 1 : 0);
        invertX.isOn = arg0;

        PlayerPrefs.Save();

    }

    public void ResetSettings()
    {
        // OnInvertYChanged(false);
        // OnInvertXChanged(false);
        // OnValueChanged(1);

        ClearPrefs();
        UpdateUI();
    }

    public void ClearPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
