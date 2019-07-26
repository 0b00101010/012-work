using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberPoint : MonoBehaviour
{
    [SerializeField]
    private bool isFixed;
    private NumberController controller;

    public bool IsFixed => isFixed;
    public LineRenderer Renderer { get; private set; }
    public NumberPoint LeftPoint { get; set; }
    public NumberPoint RightPoint { get; set; }

    public void UpdateLeftPoint(NumberPoint previousPoint)
    {
        if (isFixed)
        {
            RightPoint.UpdateRightPoint(this);
        }
        else
        {
            UpdatePoint(previousPoint);
            LeftPoint?.UpdateLeftPoint(this);
        }
    }
    public void UpdateRightPoint(NumberPoint previousPoint)
    {
        if (isFixed)
        {
            LeftPoint.UpdateLeftPoint(this);
        }
        else
        {
            UpdatePoint(previousPoint);
            RightPoint?.UpdateRightPoint(this);
        }
    }
    public void Initialize(float width, NumberController controller)
    {
        this.controller = controller;

        var collider = gameObject.AddComponent<CircleCollider2D>();
        collider.radius = width * 0.66F;
        collider.isTrigger = true;

        var hitbox = new GameObject("Hitbox").AddComponent<CircleCollider2D>();
        hitbox.gameObject.layer = LayerMask.NameToLayer("Hitbox");
        hitbox.transform.SetParent(transform);
        hitbox.transform.localPosition = Vector3.zero;
        hitbox.transform.localScale = new Vector3(0.5F, 0.5F);
        hitbox.isTrigger = true;

        if (LeftPoint != null)
        {
            Renderer = gameObject.AddComponent<LineRenderer>();
            Renderer.material = GameManager.Instance.LineMaterial;
            Renderer.widthMultiplier = width;
            Renderer.numCapVertices = 8;
            Renderer.numCornerVertices = 8;
            Renderer.SetColor(Color.black);
        }
    }
    public void UpdateRenderer()
    {
        if (LeftPoint != null)
        {
            Renderer.SetPosition(0, LeftPoint.transform.position);
            Renderer.SetPosition(1, transform.position);
        }
    }

    private void Start()
    {
        if (isFixed && !((LeftPoint?.IsFixed ?? false) && (RightPoint?.isFixed ?? false)))
        {
            var spriteRenderer = transform.GetChild(0).gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = GameManager.Instance.Circle;
            spriteRenderer.color = Color.white;
            spriteRenderer.sortingOrder = 1;
        }
    }
    private void UpdatePoint(NumberPoint previousPoint)
    {
        var direction = (transform.position - previousPoint.transform.position).normalized;
        transform.position = previousPoint.transform.position + direction * controller.Distance;
    }
}
