﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ParralaxPlanConfiguration : System.Object
{
	public GameObject prefabParralaxPlan;
	public float distance;
	public assetGenerator generatorScript;
	public float lowSpaceBetweenAsset;
	public float hightSpaceBetweenAsset;
}

public class parralaxManager : MonoBehaviour {
	public ParralaxPlanConfiguration[] configurationParralax;
	public GameObject rightBorder;
	public GameObject leftBorder;
	public List<GameObject> parralaxPlans;

	public float speed;
	// Use this for initialization
	void Start () {
		parralaxPlans.Clear ();
		foreach (ParralaxPlanConfiguration config in configurationParralax) {
			GameObject tempParralaxPlan = Instantiate(config.prefabParralaxPlan);
			tempParralaxPlan.transform.parent = this.transform;

			parallaxPlan tempScript = tempParralaxPlan.GetComponent<parallaxPlan>();
			tempScript.popLimitation = rightBorder;
			tempScript.depopLimitation = leftBorder;
			tempScript.generator = config.generatorScript;
			tempScript.distance = config.distance;
			tempScript.lowSpaceBetweenAsset = config.lowSpaceBetweenAsset;
			tempScript.hightSpaceBetweenAsset = config.hightSpaceBetweenAsset;

			parralaxPlans.Add(tempParralaxPlan);
		}
	}
	
	// Update is called once per frame
	void Update () {
		foreach (GameObject plan in parralaxPlans) {
			plan.GetComponent<parallaxPlan> ().setSpeedOfPlan (speed);
		}
	}
}
