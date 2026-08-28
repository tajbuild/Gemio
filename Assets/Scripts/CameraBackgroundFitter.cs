using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CameraBackgroundFitter : MonoBehaviour
{
    private Camera targetCamera;
    private SpriteRenderer spriteRenderer;

    [SerializeField, Min(1f)] private float overscan = 1.1f;

    private void Awake()
    {
        targetCamera = GetComponentInParent<Camera>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (targetCamera == null)
        {
            Debug.LogError("CameraBackgroundFitter must be parented to a Camera.", this);
        }

        if (spriteRenderer.sprite == null)
        {
            Debug.LogError("CameraBackgroundFitter has no background sprite.", this);
        }
    }

    private void Start()
    {
        FitToCamera();
    }

    private void FitToCamera()
    {
        if (targetCamera == null || spriteRenderer.sprite == null) return;

        // Calculate the visible world-space dimensions of the camera.
        float cameraHeight = targetCamera.orthographicSize * 2f;
        float cameraWidth = cameraHeight * targetCamera.aspect;

        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        // Scale until the image covers the entire screen without stretching.
        float requiredScale = Mathf.Max(
            cameraWidth / spriteSize.x,
            cameraHeight / spriteSize.y
        ) * overscan;

        transform.localScale = new Vector3(requiredScale, requiredScale, 1f);
    }
}