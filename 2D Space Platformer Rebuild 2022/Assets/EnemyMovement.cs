using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] private Vector3 velocity;
    [SerializeField] private float rotationSpeed = 10f, zConstant = 1f;

    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 0, zConstant) * rotationSpeed * Time.deltaTime);
    }
}
