using UnityEngine;

public class Predator : Agent
{
    [Header("Vision Settings")]
    public float viewRadius = 12f;
    public float viewAngle = 60f;

    [Header("Predator Properties")]
    public float consumptionRadius = 4f;
    public float chaseMultiplier = 1.75f;

    private GameObject targetPrey;

    /*
     * Updates the predator's behavior each frame.
     * Detects prey and either chases it or wanders if no prey is detected.
     */
    void Update()
    {
        DetectPrey();
        if (targetPrey != null)
        {
            Chase();
        }
        else
        {
            Wander();
        }
        AvoidWalls();
        MoveForward(targetPrey != null ? chaseMultiplier : 1f);
    }


    /*
     * Detects prey within the predator's view radius and angle.
     * Sets the closest prey as the target.
     */
    private void DetectPrey()
    {
        targetPrey = null;
        float minDistance = float.MaxValue;
        GameObject[] preyAgents = GameObject.FindGameObjectsWithTag("Prey");

        foreach(GameObject prey in preyAgents)
        {
            Vector3 directionToPrey = prey.transform.position - transform.position;
            float distance = directionToPrey.magnitude;
            float angle = Vector3.Angle(transform.forward, directionToPrey.normalized);

            if (distance < viewRadius && distance < minDistance && angle < viewAngle)
            {
                minDistance = distance;
                targetPrey = prey;
            }

            if (Physics.Raycast(transform.position, directionToPrey.normalized, out RaycastHit hit, distance))
            {
                if (!hit.transform.CompareTag("Prey")) continue;
            }
        }
    } 

    /*
     * Chases the target prey by turning towards it and moving forward.
     * If the predator is close enough to the prey, it consumes the prey.
     */
    private void Chase()
    {
        Vector3 toTarget = targetPrey.transform.position - transform.position;
        TurnTo(toTarget);

        float distance = Vector3.Distance(transform.position, targetPrey.transform.position);
        if (distance < consumptionRadius)
        {
            GameManager.Instance.RespawnPrey();
            Destroy(targetPrey); 
            targetPrey = null;
        }
    }
}