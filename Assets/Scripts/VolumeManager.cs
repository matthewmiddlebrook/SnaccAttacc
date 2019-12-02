using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeManager : MonoBehaviour
{
    public AudioMixer masterMixer;

    public Slider musicSlider;
    public string musicParamater;

    public Slider UISlider;
    public string UIParamater;

    void OnEnable()
    {
        float value;
        if (masterMixer.GetFloat(musicParamater, out value))
		{
			musicSlider.value = Mathf.Pow(10, value/20);
		}
        if (masterMixer.GetFloat(UIParamater, out value))
		{
			UISlider.value = Mathf.Pow(10, value/20);
		}
    }

    public void SetMusicLevel(float level)
	{
		masterMixer.SetFloat(musicParamater, Mathf.Log10(level) * 20);
	}

    public void SetUILevel(float level)
	{
		masterMixer.SetFloat(UIParamater, Mathf.Log10(level) * 20);
	}
}
