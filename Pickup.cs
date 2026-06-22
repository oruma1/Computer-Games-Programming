using UnityEngine;

public enum PickupType
{
    Coin,
    NairobiFact
}

[RequireComponent(typeof(Collider))]
public class Pickup : MonoBehaviour
{
    public PickupType type = PickupType.Coin;
    public GameObject collectEffect;

    bool collected;

    void Reset()
    {
        Collider pickupCollider = GetComponent<Collider>();
        pickupCollider.isTrigger = true;
    }

    public void Collect()
    {
        if (collected) return;
        collected = true;

        if (type == PickupType.Coin)
            GameManager.Instance.CollectCoin(transform.position);
        else
            GameManager.Instance.CollectFactPowerUp();

        if (collectEffect)
            Instantiate(collectEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
