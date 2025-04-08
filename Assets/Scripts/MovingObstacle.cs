using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] public Vector3 offset = new Vector3(0f, 0f, 5f);
    [SerializeField] public float speed = 2f;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool movingToEnd = true;

    /*
     * Initializes start and end positions based on the current position and offset.
     */    
    void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + offset; 
    }

    /*
     * Moves the obstacle toward the target position and updates the movement direction.
     */
    void Update()
    {
        if (movingToEnd)
        {
            MoveToward(endPosition);
            UpdateMoveDirection(endPosition);
        }
        else
        {
            MoveToward(startPosition);
            UpdateMoveDirection(startPosition);
        }
    }

    /*
     * Moves the obstacle toward the specified target position at the given speed.
     * @param target - The position to move toward.
     */
    private void MoveToward(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );
    }

    /*
     * Updates the movement direction of the obstacle.
     * If the obstacle is close to the target position, it reverses the movement direction.
     * @param targetPosition - The position to check distance against.
     */
    private void UpdateMoveDirection(Vector3 targetPosition)
    {
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            movingToEnd = !movingToEnd;
        }
    }
}