using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathFinding
{

    public static List<Node> CalculatePathBFS(Node startingNode, Node finishNode)
    {
        Queue<Node> frontier = new Queue<Node>();
        frontier.Enqueue(startingNode);

        Dictionary<Node, Node> comesFrom = new Dictionary<Node, Node>();
        comesFrom.Add(startingNode, null);

        while (frontier.Count > 0)
        {
            Node currentNode = frontier.Dequeue();

            bool isDone = false;

            foreach (var neighbour in currentNode.Neighbours)
            {
                if (comesFrom.ContainsKey(neighbour))
                    continue;

                frontier.Enqueue(neighbour);
                comesFrom.Add(neighbour, currentNode);

                if (neighbour == finishNode)
                {
                    isDone = true;
                    break;
                }
            }

            if (isDone) break;
        }

        List<Node> path = new List<Node>();

        if (comesFrom.ContainsKey(finishNode))
        {
            Node currentNode = finishNode;

            while (comesFrom[currentNode] != null)
            {
                path.Add(currentNode);
                currentNode = comesFrom[currentNode];
            }

            path.Add(startingNode);
            path.Reverse();
        }

        return path;
    }


    public static void MoveAlongPath(Agent agent, List<Node> pathList, int currentPathIndex)
    {
        if (pathList != null && currentPathIndex < pathList.Count)
        {   
            Node currentNode = pathList[currentPathIndex];
            Vector3 nodePosition = currentNode.transform.position;

            Vector3 desiredVelocity = SteeringBehaviours.Seek(agent.transform.position, agent.speed, agent._directionalVelocity, nodePosition, agent._steeringForce);
            agent.SetVelocity(desiredVelocity);

            agent.transform.position += agent._directionalVelocity * Time.deltaTime;


            if (Vector3.Distance(agent.transform.position, currentNode.transform.position) < 0.5f)
            {
                currentPathIndex++;
            }
        }
        else
        {
            Debug.Log("Hunter: Path completed or no path available.");
        }
    }
}
