using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceByDistance : MonoBehaviour {

    private float initialX;
    public Vector2 force;
    public Vector2 relativeForce;
    public ConstantForce2D constantforce;
    public Vector2 forceAddByDistance;
    public Vector2 relativeForceAddByDistance;

    // Use this for initialization
    void Start()
    {
        initialX = this.transform.position.x;
    }

    // Update is called once per frame
    void Update () {
        float distance = Mathf.Abs(this.transform.position.x - initialX);
        constantforce.relativeForce = relativeForce + distance * relativeForceAddByDistance;
        constantforce.force = force + distance * forceAddByDistance;
    }
}
