using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public static NodeManager Instance { get; private set; }

    [SerializeField] private List<Node> _nodeList;
    public List<Node> NodeList
    {
        get => _nodeList;
        set => _nodeList = value;
    }

    public WeightArea weightArea;

    public Node targetNode;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _nodeList = new List<Node>(FindObjectsOfType<Node>());
    }

    private void CalculateInitialWeight()
    {
        foreach (var node in _nodeList)
        {
            node.OriginalWeight = weightArea.GetAreaWeight(node.transform.position);
            node.Weight = node.OriginalWeight;
        }
    }

    public Node GetClosestNode(Vector3 position)
    {
        float distanceSqr = float.MaxValue;
        Node closestNode = null;

        foreach (Node node in _nodeList)
        {
            float newDistanceSqr = (node.transform.position - position).sqrMagnitude;

            if (newDistanceSqr < distanceSqr)
            {
                distanceSqr = newDistanceSqr;
                closestNode = node;
            }
        }

        return closestNode;
    }
}
