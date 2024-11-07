using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AIUtility
{

    public static bool IsInFOV(Agent agent, Vector3 targetPosition, LayerMask obstacleLayer)
    {
        Vector3 targetDirection = targetPosition - agent.Position;

        bool isInRange = targetDirection.sqrMagnitude < agent.VisionRadius * agent.VisionRadius;

        if (isInRange)
        {
            bool isInAngle = Vector3.Angle(agent.Forward, targetDirection) < agent.VisionAngle;
            if (isInAngle)
            {
                return IsInLineOfSight(agent.Position, targetDirection, obstacleLayer);
            }
        }
        return false;
    }

    private static bool IsInLineOfSight(Vector3 agentPosition, Vector3 targetDirection, LayerMask obstacleLayer)
    {
        float castRadius = 0.5f;

        return !Physics.SphereCast(agentPosition, castRadius, targetDirection, out _, targetDirection.magnitude, obstacleLayer);
    }

}
