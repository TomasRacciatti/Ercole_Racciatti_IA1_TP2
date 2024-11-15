using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Player : Agent
{
    [SerializeField] protected float _speed;
    [SerializeField] private LayerMask _victoryLayerMask;
    [SerializeField] private Vector3 _collisionBoxSize = new Vector3(1, 2, 1);


    private void Update()
    {
        Vector3 overlapCenter = transform.position;
        Collider[] hits = Physics.OverlapBox(overlapCenter, _collisionBoxSize / 2, Quaternion.identity, _victoryLayerMask);

        foreach (var hit in hits)
        {
            Debug.Log($"Player detected trigger zone: {hit.gameObject.name}");

            if (((1 << hit.gameObject.layer) & _victoryLayerMask) != 0)
            {
                Debug.Log("Victory");
                GameOverManager.Instance.gameWon = true;
                GameOverManager.Instance.TriggerVictory();
                return;
            }
        }

        /*
        if (hits.Length > 0)
        {

            GameOverManager.Instance.gameWon = true;
            GameOverManager.Instance.TriggerVictory();
        }
        */
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 boxCenter = transform.position;
        Gizmos.DrawWireCube(boxCenter, _collisionBoxSize);
    }
}
