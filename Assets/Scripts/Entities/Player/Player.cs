using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Player : Agent
{
    [SerializeField] protected float _speed;
    [SerializeField] private LayerMask _victoryLayerMask;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _victoryLayerMask)
        {
            Debug.Log("ganaste");
            GameOverManager.Instance.gameWon = true;
            GameOverManager.Instance.TriggerVictory();
        }
    }
}
