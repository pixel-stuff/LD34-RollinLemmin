using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectiblePicker : MonoBehaviour {
    [System.Serializable]
    public class CoinCollectedEvent : UnityEvent<int> { }
    [SerializeField] CoinCollectedEvent coinCollected;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Collectible"))
        {

            collectible hitCollectible = other.GetComponent<collectible>();
            coinCollected.Invoke(hitCollectible.value);
            hitCollectible.ShineAndDestroy();
        }
    }
}
