using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BGMmanager : MonoBehaviour
{

	public Button sfxButton;
	public Sprite OnSFX;
	public Sprite OffSFX;

	public Button bgmButton;
	public Sprite OnBGM;
	public Sprite OffBGM;

	private void Start()
	{

		if (PlayerPrefs.GetInt("MuteSFX") == 1)
			sfxButton.image.sprite = OffSFX;
		else
			sfxButton.image.sprite = OnSFX;

		if (PlayerPrefs.GetInt("MuteBGM") == 1)
			bgmButton.image.sprite = OffBGM;
		else
		{
			bgmButton.image.sprite = OnBGM;
			SoundManager.instance.Play("bgm");
		}

	}

	public void muteBGM()
	{
		if (bgmButton.image.sprite == OnBGM)
		{
			bgmButton.image.sprite = OffBGM;
			SoundManager.instance.Play("click");
			SoundManager.instance.SetMute("bgm");
			PlayerPrefs.SetInt("MuteBGM", 1);
		}

		else
		{
			bgmButton.image.sprite = OnBGM;
			SoundManager.instance.Play("click");
			PlayerPrefs.SetInt("MuteBGM", 0);
			SoundManager.instance.SetMuteCancel("bgm");
			SoundManager.instance.Play("bgm");
		}

	}

	public void muteSFX()
	{
		if (sfxButton.image.sprite == OnSFX)
		{
			sfxButton.image.sprite = OffSFX;
			SoundManager.instance.SetMute("click");
			PlayerPrefs.SetInt("MuteSFX", 1);
		}

		else
		{
			sfxButton.image.sprite = OnSFX;
			SoundManager.instance.SetMuteCancel("click");
			PlayerPrefs.SetInt("MuteSFX", 0);
		}

	}

	private void Update()
	{
		SoundManager.instance.SetVolumn("bgm", PlayerPrefs.GetFloat("backvol"));
	}
}
