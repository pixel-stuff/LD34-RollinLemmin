using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParralaxYOffset : MonoBehaviour {

    public GameObject upObject;
    public GameObject downObject;

    public float parralaxYOffset
    {
        get
        {
            return upObject.transform.position.y - downObject.transform.position.y;
        }
    }
}
