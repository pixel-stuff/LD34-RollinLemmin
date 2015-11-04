﻿
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class randomSpawnAssetConfiguration : System.Object
{
	public GameObject prefabAsset;
	public int probabilityOfApparition;
}

public class assetRandomGenerator : parralaxAssetGenerator {
	
	public randomSpawnAssetConfiguration[] AssetConfiguation;

	public List<GameObject>[] GameObjectTabOfTypePrefabs = null;// = new List<GameObject>();

	private int probabilitySomme;

	// Use this for initialization
	public override void clear(){
		for (int i =0; i < GameObjectTabOfTypePrefabs.Length; i++) {
			GameObjectTabOfTypePrefabs[i].Clear ();
		}
	}

	private int getIdOfNextAsset() {
		int random = Random.Range(0,probabilitySomme);
		for (int i =0; i < AssetConfiguation.Length; i++) {
			random -= AssetConfiguation[i].probabilityOfApparition;
			if (random <= 0){
				return i;
			}
		}
		return -1;
	}

	public override GameObject generateGameObjectAtPosition() {
		if (GameObjectTabOfTypePrefabs == null) {
			probabilitySomme = 0;
			GameObjectTabOfTypePrefabs = new List<GameObject>[AssetConfiguation.Length];
			for (int i =0; i < AssetConfiguation.Length; i++) {
				probabilitySomme += AssetConfiguation[i].probabilityOfApparition;
				GameObjectTabOfTypePrefabs[i] = new List<GameObject>();
			}
		}
		int id = getIdOfNextAsset ();
		Debug.Log (id +" " + probabilitySomme);
		if (id > 0) {
			GameObject asset = availableGameobject (GameObjectTabOfTypePrefabs[id]);
			if (asset == null) {
				asset = Instantiate (AssetConfiguation[id].prefabAsset);
				GameObjectTabOfTypePrefabs[id].Add (asset);
			}
		
			return asset; 
			}
		return null;
	}
	
	private GameObject availableGameobject(List<GameObject> list){
		foreach(GameObject gameobject in list){
			if (!gameobject.activeSelf){
				gameobject.SetActive(true);
				return gameobject;
			}
		}
		return null;
	}
}
