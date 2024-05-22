using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private MainController mainController;
    private Rigidbody2D rb;
    public float speed = 5f;

    private Vector2 moveDirection = Vector2.zero;

    private void Awake()
    {
        mainController = GetComponent<MainController>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        mainController.OnMoveEvent += Move;
    }

    private void Move(Vector2 direction)
    {
        moveDirection = direction;
    }

    private void FixedUpdate()
    {
        ApplyMove();
    }

    private void ApplyMove()
    {
        rb.velocity = moveDirection * speed;
    }




}