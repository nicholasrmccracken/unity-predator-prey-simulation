using UnityEngine;

public class Prey : Agent
{
    [Header("Vision Settings")]
    public float viewRadius = 10f;

    [Header("Predator Properties")]
    public float fleeMultiplier = 1.5f;

    private GameObject targetPredator;

    /*
     * Chases the target prey by turning towards it and moving forward.
     * If the predator is close enough to the prey, it consumes the prey.
     */
    void Update()
    {
        DetectPredator();
        if (targetPredator != null)
        {
            Flee();
        }
        else
        {
            Wander();
        }
        AvoidWalls();
        MoveForward(targetPredator != null ? fleeMultiplier : 1f);
    }

    /*
     * Detects predators within the prey's view radius.
     * Sets the closest predator as the target.
     */
    private void DetectPredator()
    {
        targetPredator = null;
        float minDistance = float.MaxValue;
        GameObject[] predatorAgents = GameObject.FindGameObjectsWithTag("Predator");

        foreach(GameObject predator in predatorAgents)
        {
            float distance = Vector3.Distance(transform.position, predator.transform.position);

            if (distance < viewRadius && distance < minDistance)
            {
                minDistance = distance;
                targetPredator = predator;
            }
        }
    } 

    /*
     * Flees from the target predator by turning away from it and moving forward.
     */
    private void Flee()
    {
        Vector3 toTarget = transform.position - targetPredator.transform.position;
        TurnTo(toTarget);
    }
}