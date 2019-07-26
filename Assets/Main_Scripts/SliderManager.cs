using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
	[SerializeField]
    private Slider backVolume;
	[SerializeField]
    private Slider sfxVolume;
    private float sfxVol = 1f;
    private float backVol = 1f;

    private SoundManager sm;
    private static SliderManager instance;

    void Start()
    {
        backVol = PlayerPrefs.GetFloat("backvol", 1f);
        backVolume.value = backVol;
        sfxVol = PlayerPrefs.GetFloat("sfxvol", 1f);
        sfxVolume.value = sfxVol;
        sm = GetComponent<SoundManager>();
    }


    public static SliderManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(SliderManager)) as SliderManager;
            }
            return instance;
        }
    }

    public void SoundSlider()
    {
        backVol = backVolume.value;
        PlayerPrefs.SetFloat("backvol", backVol);
    }

    public void sfxSoundSlider()
    {
        sfxVol = sfxVolume.value;
        PlayerPrefs.SetFloat("sfxvol", sfxVol);
    }

    private void Update()
    {
        SoundSlider();
        sfxSoundSlider();
    }

}
