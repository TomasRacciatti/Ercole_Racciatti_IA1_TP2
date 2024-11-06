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

        transform.Translate(moveDirection * _speed * Time.deltaTime, Space.World);
    }
}
