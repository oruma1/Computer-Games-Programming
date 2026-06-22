using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Obstacle : MonoBehaviour
{
    public GameObject hitEffect;

    void Reset()
    {
        Collider obstacleCollider = GetComponent<Collider>();
        obstacleCollider.isTrigger = true;
    }

    public void Hit()
    {
        if (hitEffect)
            Instantiate(hitEffect, transform.position, Quaternion.identity);

        GameManager.Instance.HitObstacle();
    }
}
