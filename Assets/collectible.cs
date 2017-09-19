using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class collectible : MonoBehaviour {

    public int value = 1;
    [SerializeField] UnityEvent collectibleOnTopEvent;
    [SerializeField] UnityEvent shineEvent;
    private bool isAlive = true;

    public void ShineAndDestroy()
    {
        shineEvent.Invoke();
    }
    public void CollectibleOnTop()
    {
        if(isAlive)
            collectibleOnTopEvent.Invoke();
    }
}
