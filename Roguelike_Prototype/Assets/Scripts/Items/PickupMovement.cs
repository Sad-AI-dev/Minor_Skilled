using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupMovement : MonoBehaviour
{
    public float amplitude = 1f;
    public float moveHeight = 0.1f;
    public float rotateSpeed = 1f;

    private float startHeight;
    private float timer;

    private void Start()
    {
        startHeight = transform.position.y;
    }

    private void Update()
    {
        SinMove();
        Rotate();
    }

    private void SinMove()
    {
        timer += Time.deltaTime;
        float yPos = startHeight + Mathf.Sin(timer * amplitude) * moveHeight;
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }

    private void Rotate()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }
}
