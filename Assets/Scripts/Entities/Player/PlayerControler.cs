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

        //transform.Translate(moveDirection * _speed * Time.deltaTime, Space.World);

        Vector3 playerPosition = transform.position + moveDirection * _speed * Time.deltaTime;

        if (!DetectCollision(playerPosition, _collisionBoxSize, obstacleMask))
        {
            transform.Translate(moveDirection * _speed * Time.deltaTime, Space.World);
        }


        if (DetectCollision(transform.position, _collisionBoxSize, _victoryLayerMask)) // Victory condition
        {
            GameOverManager.Instance.gameWon = true;
            GameOverManager.Instance.TriggerVictory();
        }
    }
}
