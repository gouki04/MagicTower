using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using System;
using MagicTower;

public class Main : MonoBehaviour 
{
	public GameObject TileMap;
	public GameObject Player;

	// Use this for initialization
	void Start () 
	{
        SpriteSheetManager.Instance.Load("3");
        SpriteSheetManager.Instance.Load("monster");
        SpriteSheetManager.Instance.Load("wall");

		Debug.Log(mt.CSVManager.Instance["monster"][2]["name"]);

        //mt.TileMapManager.Instance.TileMapObj = TileMap;
        //mt.TileMapManager.Instance.PlayerObj = Player;
        //mt.TileMapManager.Instance.EnterMap(0, 0, 0);

        //mt.Game.Instance.Init(TileMap, Player);
        //mt.Game.Instance.EnterMap(0, 0, 0);
        EventQueue.Instance.AddEvent(EEventType.ENTER_GAME);
    }

    //private void AddMonster(int r, int c, uint id)
    //{
    //    var monster_prefab = Resources.Load ("Prefabs/Monster_" + id) as GameObject;
    //    var monster = (GameObject)GameObject.Instantiate (monster_prefab);
    //    monster.transform.parent = TileMap.transform;
    //    monster.transform.localPosition = new Vector3 (c, r, 0);
    //}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
        }

        EventQueue.Instance.Update(Time.deltaTime);
	}
}
