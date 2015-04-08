using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using System;

public class Main : MonoBehaviour 
{
	public GameObject TileMap;
	public GameObject Player;

	// Use this for initialization
	void Start () 
	{
        mt.SpriteSheetManager.Instance.Load("3");
        mt.SpriteSheetManager.Instance.Load("monster");
        mt.SpriteSheetManager.Instance.Load("wall");

		mt.TileMapManager.Instance.TileMapObj = TileMap;
        mt.TileMapManager.Instance.PlayerObj = Player;
        mt.TileMapManager.Instance.EnterMap(0, 0, 0);
	}

	private void AddMonster(int r, int c, uint id)
	{
		var monster_prefab = Resources.Load ("Prefabs/Monster_" + id) as GameObject;
		var monster = (GameObject)GameObject.Instantiate (monster_prefab);
		monster.transform.parent = TileMap.transform;
		monster.transform.localPosition = new Vector3 (c, r, 0);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
        }
	}
}
