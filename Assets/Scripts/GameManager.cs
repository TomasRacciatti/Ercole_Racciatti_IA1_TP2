using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    [SerializeField] private List<Hunter> _hunters;
    public bool playerFound;
    [SerializeField] private float lostSightTimer = 10f;
    [SerializeField] private float sightTimerCounter;


    [Header("Path Finding")] // Deberia ir en el game manager o un "Node Manager"?
    [SerializeField] private List<Node> _nodeList;
    public List<Node> NodeList
    {
        get => _nodeList;
        set => _nodeList = value;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _hunters.AddRange(FindObjectsOfType<Hunter>());
    }

    private void Update()
    {
        CheckPlayerVisibility();
    }


    private void CheckPlayerVisibility()
    {
        bool anyHunterSeesPlayer = false;

        foreach (var hunter in _hunters)
        {
            if (hunter.target != null)
            {
                if (AIUtility.IsInFOV(hunter, hunter.target.Position, hunter.obstacleMask))
                {
                    anyHunterSeesPlayer = true;
                    break;
                }
            }
        }

        if (anyHunterSeesPlayer)
        {
            playerFound = true;
            sightTimerCounter = lostSightTimer;
        }
        else
        {
            if (sightTimerCounter > 0)
            {
                sightTimerCounter -= Time.deltaTime;
            }
            else
            {
                playerFound = false;
            }
        }
    }

    // Esta funcion me va a permitir sacar el nodo mas cercano al jugador cuando esta siendo observado
    // Deberia estar en el Game Manager?
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
