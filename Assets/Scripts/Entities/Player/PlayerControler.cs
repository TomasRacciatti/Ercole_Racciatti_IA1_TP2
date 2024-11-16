using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : Player
{
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ).normalized;

        Vector3 futurePosition = transform.position + moveDirection * _speed * Time.deltaTime;

        
        // Collide against walls
        if (!DetectCollision(futurePosition, _collisionBoxSize, obstacleMask))
        {
            transform.Translate(moveDirection * _speed * Time.deltaTime, Space.World);
        }

        // Victory condition
        if (DetectCollision(transform.position, _collisionBoxSize, _victoryLayerMask)) 
        {
            GameOverManager.Instance.gameWon = true;
            GameOverManager.Instance.TriggerVictory();
        }
    }
}
