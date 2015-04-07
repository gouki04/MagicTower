using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
	public enum EType
	{
		None = 0,
		Floor = 1,
		Wall = 2,
		Water = 3,
		Sky = 4,
		Monster = 5,
		Npc = 6,
		Item = 7,
		Trigger = 8,
		Door = 9,
		Portal = 10,
		Player = 11,
	}

	public EType Type;
}

