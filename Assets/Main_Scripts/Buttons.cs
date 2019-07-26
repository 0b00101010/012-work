using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Buttons : MonoBehaviour
{
    public Button sfxButton;

    public Sprite OnSFX;
    public Sprite OffSFX;

    public Button bgmButton;
    public Sprite OnBGM;
    public Sprite OffBGM;

    public GameObject option_Popup;
    public GameObject option_Button;
	public GameObject start_Button;

	public Animator startAnim;
	public GameObject mainManager;

	private void Start()
	{
		mainManager.SetActive(true);
		start_Button.SetActive(true);

		if (PlayerPrefs.GetInt("MuteSFX") == 1)
			sfxButton.image.sprite = OffSFX;
		else
			sfxButton.image.sprite = OnSFX;

		if (PlayerPrefs.GetInt("MuteBGM") == 1)
			bgmButton.image.sprite = OffBGM;
		else
			bgmButton.image.sprite = OnBGM;

	}

	public void OptionButton()
    {
		option_Popup.SetActive(true);
		option_Button.SetActive(false);
		start_Button.SetActive(false);

		SoundManager.instance.Play("click");
    }

    public void OpCloseButton()
    {
		option_Popup.SetActive(false);
		option_Button.SetActive(true);
		start_Button.SetActive(true);

		SoundManager.instance.Play("click");
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
			SoundManager.instance.SetMuteCancel("bgm");
			PlayerPrefs.SetInt("MuteBGM", 0);

		}

	}

	public void StartButton()
	{
		start_Button.SetActive(false);
		StartCoroutine("StartCorutine");
	}

	IEnumerator StartCorutine()
	{
		SoundManager.instance.Play("click");
		startAnim.SetBool("Play", true);
		yield return new WaitForSeconds(2.2f);
        option_Button.SetActive(false);
        start_Button.SetActive(false);
        //씬넘기기
        SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
		mainManager.SetActive(false);
	}

	public void ResetButton()
	{
		PlayerPrefs.DeleteAll();
	}

    private void Update()
    {

		SoundManager.instance.SetVolumn("bgm", PlayerPrefs.GetFloat("backvol", 1f));
		SoundManager.instance.SetVolumn("click", PlayerPrefs.GetFloat("sfxvol", 1f));

        if (SceneManager.sceneCount == 1 && Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_ANDROID
            Application.Quit();
#endif
        }
    }
}
