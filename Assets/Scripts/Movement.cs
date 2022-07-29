using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    Animator animator;
    bool isMoving = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");                
        if (xAxis == 0)
        {
            if (isMoving)
            {
                isMoving = false;
                UpdateAnimatorState(isMoving);
            }
            return;
        }

        if (!isMoving) isMoving = true;

        transform.position += new Vector3(xAxis * Time.deltaTime * moveSpeed, 0, 0);
        UpdateAnimatorState(isMoving);

    }

    private void UpdateAnimatorState(bool isMoving)
    {
        animator.SetBool("isMoving", isMoving);
    }
}
