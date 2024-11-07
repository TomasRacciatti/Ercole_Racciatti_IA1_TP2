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
        sightTimerCounter = lostSightTimer;
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
}
