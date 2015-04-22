using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour {

	private uint mId;
	public uint Id
	{
		get { return mId; }
		set { mId = value; }
	}

	public string SpriteName
	{
		get { return "monster_0"; }
	}

	// Use this for initialization
	void Start () {
		var spriteRender = gameObject.GetComponent<SpriteRenderer> ();
		//spriteRender.sprite = Sprite
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
