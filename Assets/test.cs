using UnityEngine;
using System.Collections;

public class test : MonoBehaviour  {
	// Please assign a material that is using position and color.
	public Material material;

	public Rect position = new Rect (16, 16, 128, 24);
	public Color color = Color.red;

	void OnGUI ()
	{        
		DrawRectangle (position, color);        
	}

	void DrawRectangle (Rect position, Color color)
	{   
		Debug.Log ("Draw");
		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0,0,color);
		texture.Apply();
		GUI.skin.box.normal.background = texture;
		GUI.Box(position, GUIContent.none);
	}

}
