using UnityEngine;

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
			case '|':
				tiles[i] = TileType.WALL;
				break;
			default:
				tiles[i] = TileType.EMPTY;
				break;
			}
		}
		return tiles;
	}

	public void construct(string tileSetPath, float tileSize, Vector3 origin)
	{
		constructRoom (tileSetPath,tileSize,this,this.name,origin);
	}

	public static void constructRoom(string tileSetPath, float tileSize, Room room, string name, Vector3 origin)
	{
		GameObject parent = new GameObject(name);
		parent.transform.position = origin;
		for(int i = 0; i < room.roomTiles.Length; i++) {
			for(int j = 0; j < room.roomTiles[i].Length; j++) {
				TileType tt = room.roomTiles[i][j];
				if(tt != TileType.EMPTY) {
					//Debug.Log ("Adding " + tileSetPath + tt.ToString() + " at " + new Vector2(i*tileSize,j*tileSize));
					GameObject tile = (GameObject)GameObject.Instantiate
						(Resources.Load<GameObject>(tileSetPath + tt.ToString()));
					tile.transform.position = origin;
					tile.transform.parent = parent.transform;
					tile.transform.localPosition = new Vector3(j*tileSize,-i*tileSize);
				}
			}
		}
	}

}

