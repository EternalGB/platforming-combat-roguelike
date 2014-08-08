using UnityEngine;
using System.Collections;

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
		for(int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++) {
				Room nextRoom = possibleRooms[Random.Range(0,possibleRooms.Length-1)];
				if(nextRoom.Width == roomWidth && nextRoom.Height == roomHeight) {
					if(Random.value < 0.5)
						nextRoom = Room.flipHorizontally(nextRoom);
					nextRoom.construct(tilePath,tileSize,startingPos + 
					                   new Vector3(i*roomWidth*tileSize,-j*roomHeight*tileSize));
				} else
					Debug.LogError("Room " + nextRoom.name + " does not have the right dimensions");

			}
		}
	}

}

