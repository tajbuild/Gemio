using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float speed = 3f;
    private Rigidbody2D rb;
    private int currentWaypointIndex;

    public Vector2 CurrentVelocity { get; private set; }
    
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("MovingPlatform requires at least one waypoint.", this);

            enabled = false;
            return;
        }

        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null)
            {
                Debug.LogError($"MovingPlatform waypoint {i} is not assigned.", this);

                enabled = false;
                return;
            }
        }
    }


    private void FixedUpdate()
    {
        Vector2 targetPosition = waypoints[currentWaypointIndex].position;

        Vector2 nextPosition = Vector2.MoveTowards(rb.position, targetPosition, speed * Time.fixedDeltaTime);

        CurrentVelocity = (nextPosition - rb.position) / Time.fixedDeltaTime;

        rb.MovePosition(nextPosition);

        if ((nextPosition - targetPosition).sqrMagnitude < 0.0001f)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
    }

    // Parent the player to the platform so they don't slide off
    private void OnDisable()
    {
        CurrentVelocity = Vector2.zero;
    }
}