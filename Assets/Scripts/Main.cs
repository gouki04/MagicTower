using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour 
{
	public GameObject TileMap;
	public GameObject Player;

	// Use this for initialization
	void Start () 
	{
		TileMapManager.Instance.TileMapObj = TileMap;
		TileMapManager.Instance.PlayerObj = Player;
		TileMapManager.Instance.EnterMap (0, 0, 0);

//        string[] floor_layer = {
//            "00000100000", 
//            "00000100000", 
//            "00000100000", 
//            "00000100000", 
//            "00000100000", 
//            "01111111110", 
//            "01111111110", 
//            "01111111110", 
//            "01110101110", 
//            "00110101100", 
//            "00000000000", 
//        };
//
//        string[] collide_layer = {
//            "33332023333", 
//            "33332023333", 
//            "33332023333", 
//            "23232023232", 
//            "22222022222", 
//            "20000000002", 
//            "20000000002", 
//            "20000000002", 
//            "20002020002", 
//            "22002020022", 
//            "22222222222", 
//        };
//
//        var floor_prefab = Resources.Load("Prefabs/Floor") as GameObject;
//        var wall_prefab = Resources.Load("Prefabs/Wall") as GameObject;
//        var water_prefab = Resources.Load("Prefabs/Water") as GameObject;
//
//		for (int i = 0; i < 11; ++i)
//		{
//			for (int j = 0; j < 11; ++j)
//			{
//                var type = floor_layer[i][j];
//                if (type == '1')
//                {
//                    var floor = (GameObject)GameObject.Instantiate(floor_prefab);
//                    floor.transform.parent = TileMap.transform;
//                    floor.transform.localPosition = new Vector3(j, i, 0);
//                }
//			}
//		}
//
//        for (int i = 0; i < 11; ++i)
//        {
//            for (int j = 0; j < 11; ++j)
//            {
//                var type = collide_layer[i][j];
//                if (type == '2')
//                {
//                    var wall = (GameObject)GameObject.Instantiate(wall_prefab);
//                    wall.transform.parent = TileMap.transform;
//                    wall.transform.localPosition = new Vector3(j, i, 0);
//                }
//                else if (type == '3')
//                {
//                    var water = (GameObject)GameObject.Instantiate(water_prefab);
//                    water.transform.parent = TileMap.transform;
//                    water.transform.localPosition = new Vector3(j, i, 0);
//                }
//            }
//        }
//
//		AddMonster (0, 5, 7);
//
//		var monster_csv = Csv.Instance ["monster"];
//		Debug.Log (monster_csv [1] ["prefab"]);
//		Debug.Log (monster_csv [2] ["name"]);
//		Debug.Log (monster_csv [3] ["atk"]);
	}

	private void AddMonster(int r, int c, uint id)
	{
		var monster_prefab = Resources.Load ("Prefabs/Monster_" + id) as GameObject;
		var monster = (GameObject)GameObject.Instantiate (monster_prefab);
		monster.transform.parent = TileMap.transform;
		monster.transform.localPosition = new Vector3 (c, r, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
