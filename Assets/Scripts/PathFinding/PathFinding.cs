using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathFinding
{

    public static List<Vector3> CalculatePathBFS(Node startingNode, Node finishNode)
    {
        if (startingNode == null || finishNode == null)
        {
            if (startingNode == null)
                Debug.LogError("CalculatePathBFS: startingNode is null.");
            else
                Debug.LogError("CalculatePathBFS: finishNode is null.");

            return new List<Vector3>();
        }

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

        List<Vector3> path = new List<Vector3>();

        if (comesFrom.ContainsKey(finishNode))
        {
            Node currentNode = finishNode;

            while (comesFrom[currentNode] != null)
            {
                path.Add(currentNode.transform.position);
                currentNode = comesFrom[currentNode];
            }

            path.Add(startingNode.transform.position);
            path.Reverse();
        }

        return path;
    }


    public static List<Vector3> CalculatePathDijkstra(Node startingNode, Node finishNode)
    {
        if (startingNode == null || finishNode == null)
        {
            if (startingNode == null)
                Debug.LogError("CalculatePathDijkstra: startingNode is null.");
            else
                Debug.LogError("CalculatePathDijkstra: finishNode is null.");

            return new List<Vector3>();
        }

        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        startingNode.Weight = 0;
        frontier.Enqueue(startingNode, 0);

        Dictionary<Node, Node> comesFrom = new Dictionary<Node, Node>();
        comesFrom.Add(startingNode, null);

        Dictionary<Node, float> costSoFar = new Dictionary<Node, float>();
        costSoFar.Add(startingNode, 0);

        while (frontier.Count > 0)
        {
            Node currentNode = frontier.Dequeue();

            bool isDone = false;

            foreach (var neighbour in currentNode.Neighbours)
            {
                float distanceBetween = Vector3.Distance(currentNode.transform.position, neighbour.transform.position);

                float newCost = costSoFar[currentNode] + distanceBetween;

                if (!costSoFar.ContainsKey(neighbour) || newCost < costSoFar[neighbour])
                {
                    if (costSoFar.ContainsKey(neighbour))
                    {
                        costSoFar[neighbour] = newCost;
                    }
                    else
                    {
                        costSoFar.Add(neighbour, newCost);
                    }

                    //neighbour.Weight = newCost;
                    frontier.Enqueue(neighbour, newCost);


                    if (costSoFar.ContainsKey(neighbour))
                    {
                        comesFrom[neighbour] = currentNode;
                    }
                    else
                    {
                        comesFrom.Add(neighbour, currentNode);
                    }

                    if (neighbour == finishNode)
                    {
                        isDone = true;
                        break;
                    }
                }
            }

            if (isDone) break;
        }

        return CalculatePath(startingNode, finishNode, comesFrom);
    }


    public static List<Vector3> CalculatePathGreedyBFS(Node startingNode, Node finishNode)
    {
        if (startingNode == null || finishNode == null)
        {
            if (startingNode == null)
                Debug.LogError("CalculatePathGreedyBFS: startingNode is null.");
            else
                Debug.LogError("CalculatePathGreedyBFS: finishNode is null.");

            return new List<Vector3>();
        }

        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        startingNode.Weight = 0;
        frontier.Enqueue(startingNode, 0);

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

                float priority = ManhattanDistance(neighbour.transform.position, finishNode.transform.position);
                frontier.Enqueue(neighbour, priority);
                comesFrom.Add(neighbour, currentNode);

                if (neighbour == finishNode)
                {
                    isDone = true;
                    break;
                }
            }

            if (isDone) break;
        }

        return CalculatePath(startingNode, finishNode, comesFrom);
    }


    // El pedido de agent por parametro era para tener la posicion inicial cuando intente aplicar theeta*.
    // Sin theeta, es innecesario pero por ahora lo dejo.
    public static List<Vector3> CalculatePathAStar(Agent agent, Node startingNode, Node finishNode)
    {
        if (startingNode == null || finishNode == null)
        {
            if (startingNode == null)
                Debug.Log("CalculatePathAStar: startingNode is null.");
            else
                Debug.Log("CalculatePathAStar: finishNode is null.");

            return new List<Vector3>();
        }

        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        startingNode.Weight = 0;
        frontier.Enqueue(startingNode, 0);

        Dictionary<Node, Node> comesFrom = new Dictionary<Node, Node>();
        comesFrom.Add(startingNode, null);

        Dictionary<Node, float> costSoFar = new Dictionary<Node, float>();
        costSoFar.Add(startingNode, 0);

        while (frontier.Count > 0)
        {
            Node currentNode = frontier.Dequeue();

            bool isDone = false;

            foreach (var neighbour in currentNode.Neighbours)
            {
                float distanceBetween = Vector3.Distance(currentNode.transform.position, neighbour.transform.position);
                float manhattanDist = ManhattanDistance(finishNode.transform.position, neighbour.transform.position);
                float newCost = costSoFar[currentNode] + distanceBetween;

                bool containsNeighbour = costSoFar.ContainsKey(neighbour);

                if (!containsNeighbour || newCost < costSoFar[neighbour])
                {
                    if (containsNeighbour)
                    {
                        costSoFar[neighbour] = newCost;
                    }
                    else
                    {
                        costSoFar.Add(neighbour, newCost);
                    }

                    frontier.Enqueue(neighbour, newCost + manhattanDist);


                    if (costSoFar.ContainsKey(neighbour))
                    {
                        comesFrom[neighbour] = currentNode;
                    }
                    else
                    {
                        comesFrom.Add(neighbour, currentNode);
                    }

                    if (neighbour == finishNode)
                    {
                        isDone = true;
                        break;
                    }
                }
            }

            if (isDone) break;
        }

        //return CalculatePath(startingNode, finishNode, comesFrom);
        return CalculatePath_TheetaStar(agent, startingNode, finishNode, comesFrom);
    }


    public static LayerMask theetaStarObstacles = LayerMask.GetMask("Obstacles");
    public static float thetaStar_ObstacleCheckRadius = 0.1f;

    // No Funcional
    private static List<Vector3> CalculatePath_TheetaStar(Agent agent, Node startingNode, Node finishNode, Dictionary<Node, Node> comesFrom)
    {
        // Debuggeo de layer mask
        for (int i = 0; i < 32; i++) 
        {
            if ((theetaStarObstacles.value & (1 << i)) != 0)
            {
                Debug.Log($"Layer {i} ({LayerMask.LayerToName(i)}) is included.");
            }
        }


        List<Vector3> path = new List<Vector3>() { finishNode.transform.position };

        Node currentNode = finishNode;
        Node previousNode = finishNode;
        Node anchor = finishNode;

        Vector3 thetaStar_InitialPosition = agent.transform.position;

        while (currentNode != null && comesFrom.ContainsKey(currentNode))
        {
            if (currentNode == anchor)
            {
                previousNode = currentNode;
                currentNode = comesFrom[currentNode];
                continue;
            }

            Vector3 currentPos = currentNode.transform.position;
            Vector3 anchorPos = anchor.transform.position;
            Vector3 direction = currentPos - anchorPos;

            // Para dibujar las lineas
            Vector3 previousPos = previousNode.transform.position;
            Vector3 directionPrevious = currentPos - previousPos;


            bool isVisible = !Physics.SphereCast(anchorPos, thetaStar_ObstacleCheckRadius, direction.normalized, out var _, direction.magnitude, theetaStarObstacles);

            if (isVisible)
            {
                Debug.DrawLine(anchorPos, anchorPos + direction, Color.green, 1f);

                previousNode = currentNode;
                currentNode = comesFrom[currentNode];
            }
            else
            {
                Debug.DrawLine(previousPos, previousPos + directionPrevious, Color.green, 1f);
                Debug.DrawLine(anchorPos, anchorPos + direction, Color.red, 1f);

                path.Add(previousNode.transform.position);
                anchor = previousNode;
                previousNode = currentNode; // Hay algo raro con esta linea, PREGUNTAR
                currentNode = comesFrom[currentNode];
            }
        }


        {
            Vector3 currentPos = thetaStar_InitialPosition;
            Vector3 anchorPos = anchor.transform.position;
            Vector3 direction = currentPos - anchorPos;

            bool isVisible = !Physics.SphereCast(anchorPos, thetaStar_ObstacleCheckRadius, direction.normalized, out var _, direction.magnitude, theetaStarObstacles);


            if (isVisible)
            {
                path.Add(thetaStar_InitialPosition);
            }
            else
            {
                path.Add(startingNode.transform.position);
                path.Add(thetaStar_InitialPosition);
            }

        }

        path.Reverse();
        return path;
    }



    private static List<Vector3> CalculatePath(Node startingNode, Node finishNode, Dictionary<Node, Node> comesFrom)
    {
        List<Vector3> path = new List<Vector3>();

        if (comesFrom.ContainsKey(finishNode))
        {
            Node currentNode = finishNode;

            while (comesFrom[currentNode] != null)
            {
                path.Add(currentNode.transform.position);
                currentNode = comesFrom[currentNode];
            }

            path.Add(startingNode.transform.position);
            path.Reverse();
        }

        return path;
    }


    public static float ManhattanDistance(Vector3 a, Vector3 b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z);
    }



    public static void MoveAlongPath(Agent agent, List<Vector3> pathList)
    {
        if (pathList != null && agent.currentPathIndex < pathList.Count)
        {
            Vector3 currentNode = pathList[agent.currentPathIndex];
            Vector3 nodePosition = currentNode;

            Vector3 desiredVelocity = SteeringBehaviours.Seek(agent.transform.position, agent.speed, agent._directionalVelocity, nodePosition, agent._steeringForce);
            agent.SetVelocity(desiredVelocity);

            agent.transform.position += agent._directionalVelocity * Time.deltaTime;


            if (Vector3.Distance(agent.transform.position, currentNode) < 0.5f)
            {
                agent.currentPathIndex++;
            }
        }
        else
        {
            //Debug.Log("Hunter path completed or no path available.");
        }
    }
}
