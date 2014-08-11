using UnityEngine;
using System.Collections.Generic;

public class RandomLevelConstructor : MonoBehaviour
{

	public int width;
	public int height;
	public string tilePath;
	public float tileSize;
	public Vector3 startingPos;
	public string enemiesPath;
	public int numEnemies;

	string path = "RoomGeneration/Rooms/";

	void Start()
	{
		TextAsset[] textRooms = Resources.LoadAll<TextAsset>(path);
		Room[] possibleRooms = new Room[textRooms.Length];
		for(int i = 0; i < textRooms.Length; i++) {
			possibleRooms[i] = new Room(textRooms[i]);
		}

		//Generate room geometries
		//all rooms have to be the same width and height
		float roomWidth = possibleRooms[0].Width;
		float roomHeight = possibleRooms[0].Height;
		List<Room> usedRooms = new List<Room>();
		List<Vector3> roomLocations = new List<Vector3>();
		for(int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++) {
				Room nextRoom = possibleRooms[Random.Range(0,possibleRooms.Length-1)];
				if(nextRoom.Width == roomWidth && nextRoom.Height == roomHeight) {
					if(Random.value < 0.5)
						nextRoom = Room.flipHorizontally(nextRoom);
					//if(Random.value < 0.5)
					//	nextRoom = Room.flipVertically(nextRoom);
					nextRoom.construct(tilePath,tileSize,startingPos + 
					                   new Vector3(i*roomWidth*tileSize,-j*roomHeight*tileSize));
					roomLocations.Add(startingPos + 
					                  new Vector3(i*roomWidth*tileSize,-j*roomHeight*tileSize));
					usedRooms.Add(nextRoom);
				} else
					Debug.LogError("Room " + nextRoom.name + " does not have the right dimensions");

			}
		}

		//Get enemy spawn locations
		List<Vector3> enemyLocs = new List<Vector3>();
		for(int r = 0; r < usedRooms.Count; r++) {
			Room room = usedRooms[r];
			for(int i = 0; i < room.roomTiles.Length; i++) {
				for(int j = 0; j < room.roomTiles[i].Length; j++) {
					if(room.roomTiles[i][j] == TileType.ENEMY) {
						enemyLocs.Add(new Vector3(r,j,i));
					}
				}
			}
		}
		numEnemies = Mathf.Min(numEnemies,enemyLocs.Count);
		List<Vector3> spawnLocs = selectN <Vector3>(enemyLocs,numEnemies);

		//Get possible enemies
		GameObject[] possibleEnemies = Resources.LoadAll<GameObject>(enemiesPath);

		//Spawn enemies
		foreach(Vector3 loc in spawnLocs) {
			Vector3 nextPos = roomLocations[(int)loc.x] + new Vector3(loc.y*tileSize,-loc.z*tileSize);
			GameObject enemy = (GameObject)GameObject.Instantiate(possibleEnemies[Random.Range(0,possibleEnemies.Length-1)]
			                                                      ,nextPos,Quaternion.identity);
			//TODO maybe push the enemy up until it's definitely not inside a block
			//this is mostly not a problem though
		}
	}

	public static List<T> selectN<T>(List<T> list, int n)
	{

		n = Mathf.Min (n,list.Count);
		Debug.Log ("Selecting " + n + " from " + list.Count);
		int l = list.Count;
		float prob;
		List<T> result = new List<T>(n);
		for(int i = 0; i < list.Count; i++) {
			prob = ((float)n)/(l-i);
			Debug.Log (prob);
			if(Random.value < prob) {
				result.Add(list[i]);
				n--;
			}
		}
		return result;
	}

}

