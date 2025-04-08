using UnityEngine;

public class Agent : MonoBehaviour
{
    [Header("Agent Properties")]
    public float moveSpeed = 2.5f;  
    public float rotationSpeed = 180f;  
    public float obstacleRayDistance = 3f;
    public float turnAngle = 90f;

    private bool isTurningFromWall = false;
    private Quaternion targetWallRotation;

    protected Vector3 currentDirection; 

    /*
     * Initializes the agent's direction with a random angle.
     */
    protected virtual void Start()
    {
        float randomAngle = Random.Range(0f, 360f);
        currentDirection = Quaternion.Euler(0f, randomAngle, 0f) * Vector3.forward;
        transform.rotation = Quaternion.Euler(0f, randomAngle, 0f);
    }

    /*
     * Moves the predator forward with a specified speed multiplier.
     * @param multiplier - The speed multiplier for movement.
     */
    protected void MoveForward(float multiplier)
    {
        transform.position += multiplier * moveSpeed * Time.deltaTime * currentDirection.normalized;
    }
    
    /*
     * Causes the agent to wander by slightly rotating it randomly.
     */
    protected void Wander()
    {
        if (!isTurningFromWall)
        {
            float angle = Random.Range(-1f, 1f);
            transform.Rotate(Vector3.up, angle);
            currentDirection = transform.forward;
        }
    }

    /*
     * Sets a target rotation for the agent to turn away from a wall.
     */
    private void SetTargetWallRotation(Vector3 wallNormal)
    {
        Vector3 turnDirection = Vector3.Cross(wallNormal, Vector3.up).normalized;
        targetWallRotation = Quaternion.LookRotation(turnDirection);
        isTurningFromWall = true;
    }

    /*
     * Checks for walls and sets the agent to turn away if necessary.
     */
    protected void AvoidWalls()
    {
        if (isTurningFromWall)
        {
            RotateAwayFromWall();
            return;
        }

        int layerMask = LayerMask.GetMask("Walls");
        Vector3 start = transform.position + Vector3.up * 0.5f;
        Vector3 leftDirection = Quaternion.Euler(0f, -30f, 0f) * transform.forward;
        Vector3 rightDirection = Quaternion.Euler(0f, 30f, 0f) * transform.forward;
        RaycastHit hit;

        if (Physics.Raycast(start, transform.forward, out hit, obstacleRayDistance, layerMask)
        || Physics.Raycast(start, leftDirection, out hit, obstacleRayDistance, layerMask)
        || Physics.Raycast(start, rightDirection, out hit, obstacleRayDistance, layerMask))
        {
            SetTargetWallRotation(hit.normal);
        }
    }

    /*
     * Rotates the agent towards the target wall rotation.
     * This method is used to smoothly rotate the agent to avoid obstacles.
     */
    private void RotateAwayFromWall()
    {
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetWallRotation,
            rotationSpeed * Time.deltaTime
        );
        currentDirection = transform.forward;

        float angleRemaining = Quaternion.Angle(transform.rotation, targetWallRotation);
        if (angleRemaining < 1f) 
        {
            isTurningFromWall = false;
        }
    }

    /*
     * Turns the agent to face a specific target direction.
     * @param toTarget - The direction to turn towards.
     */
    protected void TurnTo(Vector3 toTarget)
    {   
        if (!isTurningFromWall)
        {
            Quaternion targetRotation = Quaternion.LookRotation(toTarget, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, 
                targetRotation, 
                rotationSpeed * Time.deltaTime
            );
            currentDirection = transform.forward;
        }
    }
}