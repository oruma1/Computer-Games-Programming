using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LaneRunnerPlayer : MonoBehaviour
{
    [Header("Lane Movement")]
    public float laneWidth = 3f;
    public float laneChangeSpeed = 12f;
    public float leanAngle = 12f;

    [Header("Touch")]
    public float swipeThreshold = 50f;

    int targetLane;
    Vector2 touchStart;
    bool touching;

    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsRunning) return;

        ReadKeyboard();
        ReadTouch();
        MoveToLane();
    }

    void ReadKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            ChangeLane(-1);

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            ChangeLane(1);
    }

    void ReadTouch()
    {
        if (Input.touchCount <= 0) return;

        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            touchStart = touch.position;
            touching = true;
        }
        else if (touching && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
        {
            Vector2 delta = touch.position - touchStart;
            if (Mathf.Abs(delta.x) > swipeThreshold && Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                ChangeLane(delta.x > 0 ? 1 : -1);

            touching = false;
        }
    }

    public void ChangeLane(int direction)
    {
        targetLane = Mathf.Clamp(targetLane + direction, -1, 1);
    }

    void MoveToLane()
    {
        Vector3 target = transform.position;
        target.x = targetLane * laneWidth;
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * laneChangeSpeed);

        float lean = Mathf.Clamp((target.x - transform.position.x) / laneWidth, -1f, 1f) * -leanAngle;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, lean), Time.deltaTime * 10f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Pickup pickup))
        {
            pickup.Collect();
            return;
        }

        if (other.TryGetComponent(out Obstacle obstacle))
        {
            obstacle.Hit();
        }
    }
}
