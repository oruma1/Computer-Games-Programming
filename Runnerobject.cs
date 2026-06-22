using UnityEngine;

public class RunnerObject : MonoBehaviour
{
    public float destroyZ = -12f;
    public bool spin;
    public Vector3 spinSpeed = new(0f, 180f, 0f);

    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsRunning) return;

        transform.Translate(Vector3.back * GameManager.Instance.CurrentSpeed * Time.deltaTime, Space.World);

        if (spin)
            transform.Rotate(spinSpeed * Time.deltaTime, Space.Self);

        if (transform.position.z < destroyZ)
            Destroy(gameObject);
    }
}
