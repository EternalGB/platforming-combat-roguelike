using UnityEngine;
using System.Collections.Generic;

public class Room
{

	public TileType[][] roomTiles;
	char[] rowSeparators = {'\n'};
	public int Height
	{
		get
		{
			if(roomTiles != null && roomTiles.Length > 0)
				return roomTiles.Length;
			else
				return 0;
		}
	}

	public int Width
	{
		get
		{
			if(roomTiles != null && roomTiles.Length > 0 && roomTiles[0] != null && roomTiles[0].Length > 0)
				return roomTiles[0].Length;
			else
				return 0;
		}
	}
	public string name;

	public Room(TileType[][] tiles, string name)
	{
		this.roomTiles = (TileType[][])tiles.Clone();
		this.name = name;
	}

	public Room(TextAsset roomText) : this(roomText.text,roomText.name)
	{}

	//takes a newline separated list of the rows of tiles in the room
	public Room (string roomString, string name)
	{
		this.name = name;
		string[] rows = roomString.Split (rowSeparators);
		roomTiles = new TileType[rows.Length][];
		//UnityEngine.MonoBehaviour.print(rows[0][30]);
		for (int i = 0; i < rows.Length; i++) {
			roomTiles[i] = rowParser(rows[i].Trim());
		}
	}

	public TileType[] rowParser(string row)
	{
		TileType[] tiles = new TileType[row.Length];
		for(int i = 0; i < row.Length; i++) {
			char c = row[i];
			switch(c) {
			case '0':
				tiles[i] = TileType.EMPTY;
				break;
			case '#':
				tiles[i] = TileType.GROUND;
				break;
			case 'H':
				tiles[i] = TileType.LADDER;
				break;
			case 'D':
				tiles[i] = TileType.DAMAGE;
				break;
			case '@':
				tiles[i] = TileType.ENEMY;
				break;
			default:
				tiles[i] = TileType.EMPTY;
				break;
			}
		}
		return tiles;
	}

	public void flipHorizontally()
	{
		foreach(TileType[] row in roomTiles) {
			reverseArray(row);
		}
	}

	public void flipVertically()
	{
		TileType tmp;
		for(int i = 0; i < roomTiles.Length/2; i++) {
			Debug.Log ("Swapping row " + i + " and " + (roomTiles.Length-1-i));
			for(int j = 0; j < roomTiles[i].Length; j++) {
				tmp = roomTiles[i][j];
				roomTiles[i][j] = roomTiles[roomTiles.Length-1-i][j];
				roomTiles[roomTiles.Length-1-i][j] = tmp;
			}
		}
	}

	public static Room flipHorizontally(Room room)
	{
		Room newRoom = new Room(room.roomTiles,room.name);
		newRoom.flipHorizontally();
		return newRoom;
	}

	public static Room flipVertically(Room room)
	{
		Room newRoom = new Room(room.roomTiles,room.name);
		newRoom.flipVertically();
		return newRoom;
	}


	

	public static void reverseArray<T>(T[] array)
	{
		T tmp;
		for(int i = 0; i < array.Length/2; i++) {
			tmp = array[i];
			array[i] = array[array.Length-1-i];
			array[array.Length-1-i] = tmp;
		}
	}

	public void construct(string tileSetPath, float tileSize, Vector3 origin)
	{
		constructRoom (this,this.name,tileSetPath,tileSize,origin);
	}

	/*
	public void construct(string tileSetPath, float tileSize, Vector3 origin, out List<Vector2> enemyLocs)
	{
		constructRoom (this,this.name,tileSetPath,tileSize,origin, out enemyLocs);
	}

	public static void constructRoom(Room room, string name, string tileSetPath, float tileSize, Vector3 origin,
	                                 out List<Vector2> enemyLocs)
	{
		enemyLocs = new List<Vector2>();
		constructRoom(room,name,tileSetPath,tileSize,origin);
		for(int i = 0; i < room.roomTiles.Length; i++) {
			for(int j = 0; j < room.roomTiles[i].Length; j++) {
				if(room.roomTiles[i][j] == TileType.ENEMY)
					enemyLocs.Add(new Vector2(i,j));
			}
		}
	}
	*/

	public static void constructRoom(Room room, string name, string tileSetPath, float tileSize, Vector3 origin)
	{
		GameObject parent = new GameObject(name);
		parent.transform.position = origin;
		bool[] needsTop = new bool[room.roomTiles[0].Length];
		for(int i = 0; i < needsTop.Length; i++)
			needsTop[i] = true;
		for(int i = 0; i < room.roomTiles.Length; i++) {
			for(int j = 0; j < room.roomTiles[i].Length; j++) {
				TileType tt = room.roomTiles[i][j];
				if(tt != TileType.EMPTY) {
					//Debug.Log ("Adding " + tileSetPath + tt.ToString() + " at " + new Vector2(i*tileSize,j*tileSize));
					GameObject tile;
					if(tt != TileType.LADDER && needsTop[j]) {
						tile = (GameObject)GameObject.Instantiate
							(Resources.Load<GameObject>(tileSetPath + tt.ToString() + "_TOP"));
						needsTop[j] = false;
					} else
						tile = (GameObject)GameObject.Instantiate
							(Resources.Load<GameObject>(tileSetPath + tt.ToString()));
					tile.transform.position = origin;
					tile.transform.parent = parent.transform;
					tile.transform.localPosition = new Vector3(j*tileSize,-i*tileSize);
				} else {
					needsTop[j] = true;
				}
			}
		}
	}

}

