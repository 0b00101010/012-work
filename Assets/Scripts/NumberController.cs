using System.Linq;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NumberController : MonoBehaviour
{
    [SerializeField]
    private float distance;
    [SerializeField]
    private float width;


    private Dictionary<int, NumberPoint> point = new Dictionary<int, NumberPoint>();
    private RaycastHit2D[] results;
    private List<NumberPoint> points = new List<NumberPoint>();
    private Stopwatch stopwatch = new Stopwatch();

    public float Distance => distance;
    public System.Action<GameObject, System.Action> Clear { get; set; }

    private void Awake()
    {
        InitializePoints();
        points.ForEach(i => StartCoroutine(AlphaFade(i.Renderer, Color.clear, Color.black, 0.5F)));
        stopwatch.Start();
    }
    private IEnumerator AlphaFade(LineRenderer renderer, Color startColor, Color endColor, float time)
    {
        for (float i = 0; i <= time; i += Time.deltaTime)
        {
            renderer?.SetColor(Color.Lerp(startColor, endColor, i / time));
            yield return null;
        }

        renderer?.SetColor(endColor);
    }
    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            OnTouchBegan(0, Input.mousePosition);
        }
        if (Input.GetMouseButton(0))
        {
            OnTouchProgress(0, Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnTouchEnded(0);
        }
#elif UNITY_ANDROID
        for (var i = 0; i < Input.touchCount && !EventSystem.current.IsPointerOverGameObject(i); i += 1)
        {
            var touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Began)
            {
                OnTouchBegan(touch.fingerId, touch.position);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                OnTouchProgress(touch.fingerId, touch.position);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                OnTouchEnded(touch.fingerId);
            }
        }
#endif
    }
    private void OnTouchBegan(int index, Vector3 touchPosition)
    {
        var position = (Vector2)Camera.main.ScreenToWorldPoint(touchPosition);
        var hit = Physics2D.OverlapPoint(position, LayerMask.GetMask("Point"));

        if (hit != null)
        {
            point[index] = hit.GetComponent<NumberPoint>();
        }
    }
    private void OnTouchProgress(int index, Vector3 touchPosition)
    {
        if (point.ContainsKey(index) && point[index] != null && !point[index].IsFixed)
        {
            var position = (Vector2)Camera.main.ScreenToWorldPoint(touchPosition);
            point[index].transform.position = position;
            point[index].LeftPoint?.UpdateLeftPoint(point[index]);
            point[index].RightPoint?.UpdateRightPoint(point[index]);
            points.ForEach(i => i.UpdateRenderer());
        }
    }
    private void OnTouchEnded(int index)
    {
        if (point != null)
        {
            var origin = points.First().transform.position;
            var direction = points.Last().transform.position - origin;
            var count = Physics2D.RaycastNonAlloc(origin, direction.normalized, results,
                direction.sqrMagnitude, LayerMask.GetMask("Hitbox"));

            if (count == points.Count && stopwatch.ElapsedMilliseconds >= 500)
            {
                Clear?.Invoke(gameObject, () => points.ForEach(i => StartCoroutine(
                    AlphaFade(i.Renderer, Color.black, Color.clear, 0.5F))));

                stopwatch.Restart();
            }

            point[index] = null;
        }
    }
    private void InitializePoints()
    {
        NumberPoint previousPoint = null;

        for (int i = 0; i < transform.childCount; i += 1)
        {
            var point = transform.GetChild(i).GetComponent<NumberPoint>();
            point.LeftPoint = previousPoint;

            if (previousPoint != null) previousPoint.RightPoint = point;
            point.Initialize(width, this);
            points.Add(point);
            previousPoint = point;
        }

        results = new RaycastHit2D[points.Count];
        points.ForEach(i => i.UpdateRenderer());
    }
}
