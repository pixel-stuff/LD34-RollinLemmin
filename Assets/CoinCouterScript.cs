using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoinCouterScript : MonoBehaviour
{
    [System.Serializable]
    public class CoinAddedEvent : UnityEvent<string> { }

    int numberOfCoin = 0;
    [SerializeField] CoinAddedEvent coinAdded;
    public void addCoin(int number)
    {
        numberOfCoin+= number;
        Debug.Log("NUMBER OF COIN " + numberOfCoin);
        coinAdded.Invoke(numberOfCoin.ToString());
    }
}
