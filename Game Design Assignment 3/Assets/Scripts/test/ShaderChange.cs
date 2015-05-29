using UnityEngine;
using System.Collections;

public class ShaderChange : MonoBehaviour {
	//[MenuItem ("AssetDatabase/FileOperationsExample")]
	// Use this for initialization
	private Material transparentGround;
	private Material normalGround;
	void Start () {
		transparentGround = GameObject.Find ("s1_hGroundTransparent 1").GetComponent<Renderer>().material;
		normalGround = GameObject.Find ("s1_hGround 1").GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnCollisionEnter(Collision collisionInfo) {
		if(collisionInfo.gameObject.name.Equals("Player")){
			GameObject[] tops = GameObject.FindGameObjectsWithTag("LVL3_Top");
			for(int i = 0; i < tops.Length; i ++) {
				GameObject go = tops[i];
				//go.GetComponent<Renderer>().material.mainTexture = Resources.Load("Materials/Wall") as Texture;
				//go.GetComponent<Renderer>().material = AssetDatabase.LoadAssetAtPath("Assets/Materials/Transparent", typeof(Material))) as Material;
				//go.GetComponent<Renderer>().material = AssetDatabase.LoadAssetAtPath("Assets/Materials/Transparent", typeof(Material)) as Material;
				go.GetComponent<Renderer>().material = transparentGround;
			}
		}
	}
	
	void OnCollisionExit(Collision collisionInfo) {
		if(collisionInfo.gameObject.name.Equals("Player")){
			GameObject[] tops = GameObject.FindGameObjectsWithTag("LVL3_Top");
			for(int i = 0; i < tops.Length; i ++) {
				GameObject go = tops[i];
				//				Debug.Log(go.name);
				//go.GetComponent<Renderer>().material.mainTexture = Resources.Load("Materials/Wall") as Texture;
				//go.GetComponent<Renderer>().material = AssetDatabase.LoadAssetAtPath("Assets/Materials/Transparent", typeof(Material))) as Material;
				//go.GetComponent<Renderer>().material = AssetDatabase.LoadAssetAtPath("Assets/Materials/Transparent", typeof(Material)) as Material;
				go.GetComponent<Renderer>().material = normalGround;
			}
		}
	}
}