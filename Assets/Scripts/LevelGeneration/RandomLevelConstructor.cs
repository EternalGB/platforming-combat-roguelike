using UnityEngine;
using System.Collections.Generic;

public class RandomLevelConstructor : MonoBehaviour
{

	public int width;
	public int height;
	public string tilePath;
	public float tileSize;
	public Vector3 startingPos;


	string path = "RoomGeneration/Rooms/";

	void Start()
	{
		TextAsset[] textRooms = Resources.LoadAll<TextAsset>(path);
		Room[] possibleRooms = new Room[textRooms.Length];
		for(int i = 0; i < textRooms.Length; i++) {
			possibleRooms[i] = new Room(textRooms[i]);
		}

		//all rooms have to be the same width and height
		float roomWidth = possibleRooms[0].Width;
		float roomHeight = possibleRooms[0].Height;
		List<Room> usedRooms = new List<Room>();
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
					usedRooms.Add(nextRoom);
				} else
					Debug.LogError("Room " + nextRoom.name + " does not have the right dimensions");

			}
		}

		List<List<Vector2>> enemyLocs = new List<List<Vector2>>();
		foreach(Room room in usedRooms) {
			List<Vector2> locs = new List<Vector2>();
			for(int i = 0; i < room.roomTiles.Length; i++) {
				for(int j = 0; j < room.roomTiles[i].Length; j++) {
					if(room.roomTiles[i][j] == TileType.ENEMY)
						locs.Add(new Vector2(i,j));
				}
			}
			enemyLocs.Add(locs);
		}
	
	}

}

