using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CameraBackgroundFitter : MonoBehaviour
{
    [SerializeField, Min(1f)] private float overscan = 1.1f;

    private Camera targetCamera;
    private SpriteRenderer spriteRenderer;

    private float previousOrthographicSize = -1f;
    private float previousAspect = -1f;

    private void Awake()
    {
        targetCamera = GetComponentInParent<Camera>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (targetCamera == null)
        {
            Debug.LogError(
                "CameraBackgroundFitter must be parented to a Camera.",
                this
            );
        }

        if (spriteRenderer.sprite == null)
        {
            Debug.LogError(
                "CameraBackgroundFitter has no background sprite.",
                this
            );
        }
    }

    private void LateUpdate()
    {
        if (targetCamera == null || spriteRenderer.sprite == null) return;

        // Cinemachine can change the real camera's lens after Start().
        // Refit whenever its size or screen aspect ratio changes.
        if (!Mathf.Approximately(
                targetCamera.orthographicSize,
                previousOrthographicSize
            ) ||
            !Mathf.Approximately(
                targetCamera.aspect,
                previousAspect
            ))
        {
            FitToCamera();
        }
    }

    private void FitToCamera()
    {
        float cameraHeight = targetCamera.orthographicSize * 2f;
        float cameraWidth = cameraHeight * targetCamera.aspect;

        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        float requiredScale = Mathf.Max(
            cameraWidth / spriteSize.x,
            cameraHeight / spriteSize.y
        ) * overscan;

        // Set an absolute scale rather than multiplying the current scale.
        transform.localScale = new Vector3(
            requiredScale,
            requiredScale,
            1f
        );

        previousOrthographicSize = targetCamera.orthographicSize;
        previousAspect = targetCamera.aspect;
    }
}