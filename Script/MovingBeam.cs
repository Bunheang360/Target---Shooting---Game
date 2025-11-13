using UnityEngine;

public class MovingBeam : MonoBehaviour
{
    public float moveDistance = 3f;
    public float moveSpeed = 1f;

    private Vector3 startPosition;
    private float moveTimer = 0f;
    private float phaseOffset; // unique offset per object

    void Start()
    {
        startPosition = transform.position;
        phaseOffset = Random.Range(0f, Mathf.PI * 2f); // random start point
    }

    void Update()
    {
        moveTimer += Time.deltaTime * moveSpeed;
        float offset = Mathf.Sin(moveTimer + phaseOffset) * moveDistance;
        transform.position = startPosition + new Vector3(offset, 0, 0);
    }
}
