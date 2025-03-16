using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour
{
    public List<Transform> targets;
    public float padding = 2f;
    public float desiredBottomY = 0f;
    public float minSize = 5f; // Minimum camera size
    public float smoothTime = 0.3f; // Smoothing time for camera movement and size adjustment

    private Vector3 velocity = Vector3.zero;
    private float sizeVelocity = 0f;

    private void Update()
    {
        if (targets.Count == 0) return;

        // Calculate bounds of all targets
        Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
        Vector2 max = new Vector2(float.MinValue, float.MinValue);
        foreach (Transform target in targets)
        {
            min = Vector2.Min(min, (Vector2)target.position);
            max = Vector2.Max(max, (Vector2)target.position);
        }

        // Add padding
        min -= new Vector2(padding, padding);
        max += new Vector2(padding, padding);

        // Calculate required size based on height and width
        float height = max.y - min.y;
        float width = max.x - min.x;
        float requiredSize = Mathf.Max(height / 2f, width / (2f * Camera.main.aspect));

        // Clamp size to minimum adjust
        requiredSize = Mathf.Max(requiredSize, minSize);
        Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, requiredSize, ref sizeVelocity, smoothTime);

        // Calculate and adjust camera position
        float targetY = desiredBottomY + requiredSize;
        float targetX = (min.x + max.x) / 2f;
        Vector3 targetPosition = new Vector3(targetX, targetY, Camera.main.transform.position.z);
        Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, targetPosition, ref velocity, smoothTime);
    }

    // Public methods to add/remove targets
    public void AddTarget(Transform target)
    {
        targets.Add(target);
    }
    public void RemoveTarget(Transform target)
    {
        targets.Remove(target);
    }
    public void ClearTargets()
    {
        targets.Clear();
    }
}
