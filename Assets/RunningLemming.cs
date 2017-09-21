using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningLemming : MonoBehaviour
{

    private bool _ballIsSpawn = false;
    public bool ballIsSpawn
    {
        get
        {
            return _ballIsSpawn;
        }
        set
        {
            _ballIsSpawn = value;
        }
    }
    public void changePosition(float radius)
    {
        if (ballIsSpawn)
        {
            gameObject.transform.localPosition = new Vector3(0, radius + this.GetComponent<SpriteRenderer>().size.y / 2, 0);
        }
        else
        {
            gameObject.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    public void BallDestructed()
    {
        //move on lemming position
        this.gameObject.GetComponentInParent<lemmings>().transform.position = this.transform.position;
    }
}