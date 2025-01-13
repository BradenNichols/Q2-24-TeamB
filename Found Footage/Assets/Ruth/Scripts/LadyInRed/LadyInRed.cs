using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadyInRed : BaseEnemy
{
    public float movementSpeed = 20f;
    public float rotationSpeed = 100f;

    private bool isWandering = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;

    //for the AI detection and enemy to chase 
    public bool detecting = false;
    public bool chase = false;
    public float detectionRange;

    Rigidbody rb;

    public System.Action<GameObject> onDeath { get; internal set; }

    new void Start()
    {
        base.Start();

        //animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    new void Update()
    {
        if (isWandering == false)
        {
            StartCoroutine(Wander());
        }
        if (isRotatingRight == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * rotationSpeed);
        }
        if (isRotatingLeft == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * -rotationSpeed);
        }
        if (isWalking == true)
        {
            // Move the enemy directly instead of applying force
            rb.MovePosition(rb.position + transform.forward * movementSpeed * Time.deltaTime);
            //animator.SetBool("isRunning", true);
        }
        //if (isWalking == false)
        //{
        //    animator.SetBool("isRunning", false);
        //}
    }

    IEnumerator Wander()
    {
        int rotationTime = Random.Range(1, 1);
        int rotatateWait = Random.Range(1, 1);
        int rotateDirection = Random.Range(1, 1);
        int walkWait = Random.Range(1, 1);
        int walkTime = Random.Range(1, 1);

        isWandering = true;

        yield return new WaitForSeconds(walkWait);

        isWalking = true;
        yield return new WaitForSeconds(walkTime);

        isWalking = false;

        yield return new WaitForSeconds(rotatateWait);

        if (rotateDirection == 1)
        {
            isRotatingLeft = true;
            yield return new WaitForSeconds(rotationTime);
            isRotatingLeft = false;
        }

        if (rotateDirection == 2)
        {
            isRotatingRight = true;
            yield return new WaitForSeconds(rotationTime);
            isRotatingRight = false;
        }

        isWandering = false;
    }
}

