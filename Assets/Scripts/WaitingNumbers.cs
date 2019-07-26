using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingNumbers : MonoBehaviour
{
    [SerializeField]
    private int randomMin;
    [SerializeField]
    private int randomMax;
    [SerializeField]
    private float speed;
    [SerializeField]
    private Animator[] numbers;
    [SerializeField]
    private RuntimeAnimatorController[] animators;

    private List<(int number, Transform transform)> waitingNumbers = new List<(int, Transform)>();
    private System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

    private void Awake()
    {
        GameManager.Instance.GetWaitingNumber = () =>
        {
            if (waitingNumbers.Count > 0)
            {
                var waitingNumber = waitingNumbers[Random.Range(0, waitingNumbers.Count)];
                waitingNumbers.Remove(waitingNumber);
                return waitingNumber;
            }
            else
            {
                SpawnNumber();
                return GameManager.Instance.GetWaitingNumber();
            }
        };
    }
    private void Start()
    {
        StartCoroutine(SpawnRandomly());
    }
    private void Update()
    {
        for (int i = 0; i < waitingNumbers.Count; i += 1)
        {
            waitingNumbers[i].transform.Translate(-speed * Time.deltaTime, 0F, 0F);

            if (waitingNumbers[i].transform.position.x < -3.5F)
            {
                waitingNumbers[i].transform.gameObject.SetActive(false);
                waitingNumbers.Remove(waitingNumbers[i]);
            }
        }
    }
    private IEnumerator SpawnRandomly()
    {
        while (true)
        {
            SpawnNumber();
            yield return StartCoroutine(WaitRandomly());
        }
    }
    private IEnumerator WaitRandomly()
    {
        stopwatch.Restart();
        int random = Random.Range(randomMin, randomMax);

        while (true)
        {
            if (stopwatch.ElapsedMilliseconds >= random && !(waitingNumbers.Count == 5))
            {
                break;
            }
            yield return null;
        }

        stopwatch.Stop();
    }
    private void SpawnNumber()
    {
        int number = Random.Range(0, 10);
        var workingNumber = System.Array.Find(numbers, i => !i.gameObject.activeInHierarchy);
        workingNumber.transform.position = new Vector2(3.5F, workingNumber.transform.position.y);
        workingNumber.gameObject.SetActive(true);
        workingNumber.runtimeAnimatorController = animators[number];
        waitingNumbers.Add((number, workingNumber.transform));
    }
}
