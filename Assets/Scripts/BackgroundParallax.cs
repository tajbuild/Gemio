using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    [SerializeField, Range(0f, 0.1f)] private float parallaxStrength = 0.03f;
    [SerializeField] private Vector2 maximumOffset = new Vector2(0.5f, 0.2f);

    private Camera targetCamera;
    private Vector3 startingCameraPosition;
    private Vector3 startingLocalPosition;

    private void Start()
    {
        targetCamera = GetComponentInParent<Camera>();

        if (targetCamera == null)
        {
            Debug.LogError("BackgroundParallax must be parented to a Camera.", this);
            enabled = false;
            return;
        }

        startingCameraPosition = targetCamera.transform.position;
        startingLocalPosition = transform.localPosition;
    }

    private void LateUpdate()
    {
        Vector3 cameraMovement = targetCamera.transform.position - startingCameraPosition;

        // Move slightly against the camera to create distant depth.
        float offsetX = Mathf.Clamp(
            -cameraMovement.x * parallaxStrength,
            -maximumOffset.x,
            maximumOffset.x
        );

        float offsetY = Mathf.Clamp(
            -cameraMovement.y * parallaxStrength,
            -maximumOffset.y,
            maximumOffset.y
        );

        transform.localPosition = startingLocalPosition +
            new Vector3(offsetX, offsetY, 0f);
    }
}