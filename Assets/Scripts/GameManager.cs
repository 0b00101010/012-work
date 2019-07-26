using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [System.Serializable]
    private struct Number
    {
        public NumberController[] controllers;
    }

    [SerializeField]
    private float sliderSpeed;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Image highScore;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Material lineMaterial;
    [SerializeField]
    private Sprite circle;
    [SerializeField]
    private Transform playArea;
    [SerializeField]
    private Transform[] workingNumbers;
    [SerializeField]
    private Transform[] workArea;
    [SerializeField]
    private Number[] numbers;

	private GameButtons gO;
    private IEnumerator updateCrt;

    private int score;
	public int scoreRe;

    public Material LineMaterial => lineMaterial;
    public Sprite Circle => circle;
    public System.Func<(int number, Transform transform)> GetWaitingNumber { get; set; }

    private void Start()
    {
        this.Invoke(() => StartCoroutine(StartNextNumber()), 3F);
		gO = FindObjectOfType<GameButtons>();
    }
    private void Update()
    {
        if (sliderSpeed > 1F)
        {
            sliderSpeed = Mathf.Clamp(sliderSpeed - Time.deltaTime / 60F, 1F, 5F);
        }
    }
    private IEnumerator UpdateCrt()
    {
        while (true)
        {
            slider.value -= Time.deltaTime;

            if (slider.value <= 0)
            {
                GameOver();
            }

            yield return null;
        }
    }
    private void GameOver()
    {
        Time.timeScale = 0F;
		gO.GameOverUI();
		scoreRe = score;
    }
    private IEnumerator Transform(SpriteRenderer renderer, Vector3 startPos, Vector3 endPos,
        Vector3 startScale, Vector3 endScale, Color startColor, Color endColor, float time)
    {
        for (float i = 0; i <= time; i += Time.deltaTime)
        {
            renderer.transform.position = Vector3.Lerp(startPos, endPos, i / time);
            renderer.transform.localScale = Vector3.Lerp(startScale, endScale, i / time);
            renderer.color = Color.Lerp(startColor, endColor, i / time);
            yield return null;
        }

        renderer.transform.position = endPos;
        renderer.transform.localScale = endScale;
        renderer.color = endColor;
    }
    private IEnumerator StartNextNumber()
    {
        var workingNumber = GetWaitingNumber();
        var sprite = workingNumber.transform.GetComponent<SpriteRenderer>();

        yield return StartCoroutine(Transform(sprite,
            workingNumber.transform.position, Vector3.zero, workingNumber.transform.localScale, Vector3.one,
            Color.white, Color.white * new Color(1F, 1F, 1F, 0F), 0.5F));

        workingNumber.transform.gameObject.SetActive(false);
        sprite.color = Color.white;
        workingNumber.transform.localPosition = Vector3.zero;
        workingNumber.transform.localScale = Vector3.one * 0.3F;

        updateCrt = UpdateCrt();
        StartCoroutine(updateCrt);
        slider.maxValue = sliderSpeed;
        slider.value = slider.maxValue;

        var numberControllers = numbers[workingNumber.number].controllers;
        var numberController = numberControllers[Random.Range(0, numberControllers.Length)];
        Instantiate(numberController, playArea).Clear = Clear;
    }
    private void Clear(GameObject sender, System.Action action)
    {
        StopCoroutine(updateCrt);
        score += 1;

        if (score > Prefs.HighScore)
        {
            HighScore();
        }

        scoreText.text = $": {score}점";
        action();
        StartCoroutine(SendToWorkArea(sender));
        StartCoroutine(StartNextNumber());
    }
    private IEnumerator SendToWorkArea(GameObject sender)
    {
        float x = Random.Range(workArea[0].position.x, workArea[1].position.x);
        float y = Random.Range(workArea[0].position.y, workArea[1].position.y);

        var number = System.Array.Find(workingNumbers, i => !i.gameObject.activeInHierarchy);

        if (number != null)
        {
            number.gameObject.SetActive(true);

            yield return StartCoroutine(Transform(number.GetComponent<SpriteRenderer>(),
                Vector3.zero, new Vector3(x, y), Vector3.one, Vector3.one * 0.3F,
                Color.white * new Color(1F, 1F, 1F, 0F), Color.white, 0.5F));
        }

        Destroy(sender);
    }

    private void HighScore()
    {
        Prefs.HighScore = score;
        highScore.gameObject.SetActive(true);
		gO.newScoreAnim();
        StartCoroutine(HighScoreScale());
    }
    private IEnumerator HighScoreScale()
    {
        float minScale = 0.75F;
        float maxScale = 1F;

        for (int j = 0; j < 3; j += 1)
        {
            for (float i = 0F; i <= 0.5F; i += Time.deltaTime)
            {
                float scale = Mathf.Lerp(minScale, maxScale, i / 0.5F);
                highScore.rectTransform.localScale = new Vector3(scale, scale, 1F);
                yield return null;
            }
            for (float i = 0F; i <= 0.5F; i += Time.deltaTime)
            {
                float scale = Mathf.Lerp(maxScale, minScale, i / 0.5F);
                highScore.rectTransform.localScale = new Vector3(scale, scale, 1F);
                yield return null;
            }
        }
    }
}