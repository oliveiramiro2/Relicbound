using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GrappleRope : MonoBehaviour
{
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }

    public void Show(Vector2 start, Vector2 end)
    {
        lineRenderer.enabled = true;

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    public void Hide()
    {
        lineRenderer.enabled = false;
    }

    private void LateUpdate()
    {
        if (!lineRenderer.enabled)
            return;
    }
}