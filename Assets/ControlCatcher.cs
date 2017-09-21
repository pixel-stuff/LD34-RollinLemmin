using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControlCatcher : MonoBehaviour {

    [SerializeField] UnityEvent OneTouchEvent;
    [SerializeField] UnityEvent ShakeEvent;

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.touchCount == 1)
        {
            OneTouchEvent.Invoke();
           
        }

        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.DownArrow) || Mathf.Abs(Input.acceleration.y) > 1.5f)
        {
            ShakeEvent.Invoke(); 
        }
    }
}
