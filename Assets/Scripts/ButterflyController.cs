using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyController : MonoBehaviour
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    public float minUpdateDelay;
    public float maxUpdateDelay;

    public float minMoveDistance;
    public float maxMoveDistance;

    public float speed;

    private float lastDirectionUpdateTime;
    private float nextUpdateDelay;

    private Vector2 movementDirection;
    private Vector2 movementDestination;
    private Vector2 previousPosition;
    private float moveDistance;
    private float moveTimer;

    // Start is called before the first frame update
    void Start()
    {
        lastDirectionUpdateTime = Time.time;
        nextUpdateDelay = Random.Range(minUpdateDelay, maxUpdateDelay);
        movementDestination = transform.position;
        previousPosition = transform.position;
        moveDistance = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastDirectionUpdateTime > nextUpdateDelay)
        {
            GetNextMovement();
        }

        if ((Vector2)this.transform.position != movementDestination)
        {
            MoveToDestination();
        }
    }

    private void GetNextMovement()
    {
        previousPosition = movementDestination;
        // Probably better to disable axis that goes out of bounds, or just not move if it'd go outside to avoid the tiny chance that this loop takes forever
        do
        {
            float XFlipModifier = Random.value > 0.5f ? -1f : 1f;
            float YFlipModifier = Random.value > 0.5f ? -1f : 1f;
            float angle = Random.value;

            movementDirection = new Vector2(XFlipModifier * angle, YFlipModifier * (1 - angle));
            moveDistance = Random.Range(minMoveDistance, maxMoveDistance);
            movementDestination = previousPosition + (movementDirection * moveDistance);
        } while (!CheckInBounds(movementDestination));
        moveTimer = 0f;
        lastDirectionUpdateTime = Time.time;
        nextUpdateDelay = Random.Range(minUpdateDelay, maxUpdateDelay);
        if (movementDirection.x < 0f && this.transform.localScale.x > 0)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x * -1f, this.transform.localScale.y, this.transform.localScale.z);
        }
        else if (movementDirection.x > 0f && this.transform.localScale.x < 0f)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x * -1f, this.transform.localScale.y, this.transform.localScale.z);
        }
    }

    private void MoveToDestination()
    {
        transform.position = Vector2.Lerp(previousPosition, movementDestination, (moveTimer += Time.deltaTime * speed * (1/moveDistance)));
    }

    private bool CheckInBounds(Vector2 destination)
    {
        bool result = true;
        result &= destination.x > minX;
        result &= destination.x < maxX;
        result &= destination.y > minY;
        result &= destination.y < maxY;
        return result;
    }
}
