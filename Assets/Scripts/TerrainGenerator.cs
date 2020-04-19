using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGenerator : MonoBehaviour
{

	public List<Sprite> tileSprites = new List<Sprite>();
	public int baseThreshold = 300;
	public int sandThreshold=50;
	public GameObject player;
	public int grassThreshold=50;
	public int mountainThreshold=30;
	public int mountainUpLim = 40;
	public int mountainSporadicity=69;
	public int robotBaseSporadicity = 100;
	public int mountainSpread=7;
	public List<GameObject> entities = new List<GameObject>();
	[SerializeField]
	bool change = false;
	public GameObject reactor;
	public GameObject ore;
	public GameObject car;

	// Start is called before the first frame update
	void Start()
    {

		int counter = 0;
		foreach (Type type in Type.GetValues(typeof(Type)))
        {
			
			Tile tile = ScriptableObject.CreateInstance<Tile>();
			tile.sprite = tileSprites[counter];
			counter++;
			tiles.Add(type, tile);

        }


		regenerate();
		Robot.player = player;
	
    }


	void placePlayer()
    {
		int rNum = (int)Random.Range(0, Reactor.reactors.Count-1);
		
		Reactor.reactors[rNum].GetComponent<Reactor>().isPlayer = true;
		Vector3 start = Reactor.reactors[rNum].transform.position;
		Vector3 place = Random.onUnitSphere;
		Vector2 pos = (Vector2)place;
		pos = pos.normalized;
		player.transform.position = start + new Vector3(pos.x,pos.y,0) * 5;
		

	}


	void regenerate()
    {
		levelx = new double[DATA_SIZE, DATA_SIZE];
		
		generateLevel();
        while (average() < baseThreshold + sandThreshold + mountainThreshold + grassThreshold)
        {
			generateLevel();
        }
		populateTilemap();
		placePlayer();
	}

    // Update is called once per frame
    void Update()
    {
        if (change)
        {
			change = false;
			while (average() < baseThreshold + sandThreshold + mountainThreshold + grassThreshold)
			{

				generateLevel();
			}
			regenerate();
        }
    }


	public void populateMap()
    {

    }




	public Tilemap tilemap;
	public Dictionary<Type, Tile> tiles = new Dictionary<Type, Tile>();
	static int DATA_SIZE = 257;
	public static double[,] levelx = new double[DATA_SIZE,DATA_SIZE];


	public int average()
    {

		int count = 0;
		int average = 0;
		for (int i = 0; i < DATA_SIZE-2; i+=2)
		{
			for (int j = 0; j < DATA_SIZE-2; j+=2)
			{
				average += (int)levelx[i, j];
				count++;
			}
		}
		return average / count;
	}


	public void populateTilemap()
    {
		tilemap.ClearAllTiles();
		for(int i = 0; i < DATA_SIZE; i++)
        {
			for(int j = 0; j < DATA_SIZE; j++)
            {

				double data = levelx[i, j];
                if (tilemap.GetTile(new Vector3Int(i, j, 0)) == null)
                {
					Tile tile = GetTileFromData(data, i, j);
					tilemap.SetTile(new Vector3Int(i, j, 0), tile);
                    if (tile != tiles[Type.Mountain])
                    {
						tilemap.SetColliderType(new Vector3Int(i, j, 0), Tile.ColliderType.None);
                    }
				}
                else
                {
					tilemap.SetColliderType(new Vector3Int(i, j, 0), Tile.ColliderType.None);
				}
            }
        }
    }



	private Tile GetTileFromData(double data, int x, int y)
    {
		if(data>=baseThreshold + sandThreshold && data <= baseThreshold + sandThreshold+mountainThreshold)
        {
			return tiles[Type.Sand];
        }
		else if (data > baseThreshold + sandThreshold+ mountainThreshold && data < baseThreshold + sandThreshold + mountainThreshold+grassThreshold)
		{
           
			return tiles[Type.Mountain];
			
			
		}
		else if(data> baseThreshold + sandThreshold + mountainThreshold + grassThreshold)
        {
			if ((int)data % robotBaseSporadicity == 0 && data - (int)data < 0.02)
			{
				int numOfTilesToReplace = 1;
				for(int i = x - numOfTilesToReplace; i <= x + numOfTilesToReplace; i++)
                {
					for(int j = y- numOfTilesToReplace; j <= y + numOfTilesToReplace; j++)
                    {
                        if (j != i)
                        {
							tilemap.SetTile(new Vector3Int(x - 1, y, 0), tiles[Type.Grass]);
						}
						
					}
                }
			
				
				Reactor.reactors.Add(Instantiate(reactor, new Vector3(tilemap.GetCellCenterWorld(new Vector3Int(x, y, 0)).x, tilemap.GetCellCenterWorld(new Vector3Int(x, y, 0)).y, 0), Quaternion.Euler(0, 0, Random.Range(0, 360))));
				return tiles[Type.Grass];
				
			}
			else if((int)data % mountainSporadicity <= mountainSpread&& (int)data < baseThreshold + sandThreshold + mountainThreshold +mountainUpLim && data- (int)data <30f/mountainSporadicity)
            {
                if (Random.value > 0.95)
                {
					entities.Add(Instantiate(ore, new Vector3(tilemap.GetCellCenterWorld(new Vector3Int(x, y, 0)).x, tilemap.GetCellCenterWorld(new Vector3Int(x, y, 0)).y, 0), Quaternion.Euler(0, 0, Random.Range(0, 360))));
					return tiles[Type.Grass];
				}
				tilemap.SetColliderType(new Vector3Int(x, y, 0), Tile.ColliderType.Sprite);
				return tiles[Type.Mountain];
			}
			if (Random.value > 0.999)
            {
				
				entities.Add(Instantiate(car, new Vector3(tilemap.GetCellCenterWorld(new Vector3Int(x, y, 0)).x, tilemap.GetCellCenterWorld(new Vector3Int(x, y, 0)).y, 0), Quaternion.Euler(0,0,Random.Range(0,360))));
            }
			return tiles[Type.Grass];
        }
		
        else
        {
			return tiles[Type.Water];
        }
    }

	public void EntitiesClear()
    {
        for (int i = 0; i < entities.Count; i++)
        {
			Destroy(entities[i]);
        }
		entities.Clear();
    }


	public void generateLevel()
	{
		Reactor.ClearList();
		EntitiesClear();
		//for (int i = 0; i < Types.length; i++)
		//{
		//	tileSetImgs.put(Types[i], GameEntity.getImage("tileset/" + Types[i] + ".png"));
		//}


		

		//size of grid to generate, note this must be a
		//value 2^n+1
		 
		//an initial seed value for the corners of the levelx
		 double SEED = 1000.0*(Random.value);
		//seed the levelx
		levelx[0,0] = levelx[0,DATA_SIZE - 1] = levelx[DATA_SIZE - 1,0] = levelx[DATA_SIZE - 1,DATA_SIZE - 1] = SEED;

		double h = 500.0;//the range (-h -> +h) for the average offset
		Random r = new Random();//for the new value in range of h
								//side length is distance of a single square side
								//or distance of diagonal in diamond
		for (int sideLength = DATA_SIZE - 1;
			//side length must be >= 2 so we always have
			//a new value (if its 1 we overwrite existing values
			//on the last iteration)
			sideLength >= 2;
			//each iteration we are looking at smaller squares
			//diamonds, and we decrease the variation of the offset
			sideLength /= 2, h /= 2.0)
		{
			//half the length of the side of a square
			//or distance from diamond center to one corner
			//(just to make calcs below a little clearer)
			int halfSide = sideLength / 2;

			//generate the new square values
			for (int x = 0; x < DATA_SIZE - 1; x += sideLength)
			{
				for (int y = 0; y < DATA_SIZE - 1; y += sideLength)
				{
					//x, y is upper left corner of square
					//calculate average of existing corners
					double avg = levelx[x,y] + //top left
					levelx[x + sideLength,y] +//top right
					levelx[x,y + sideLength] + //lower left
					levelx[x + sideLength,y + sideLength];//lower right
					avg /= 4.0;
					double rand = Random.value;
					while (rand < 0.5 && rand > 0.25) { rand = Random.value; }
					//center is average plus random offset
					levelx[x + halfSide,y + halfSide] =
				  //We calculate random value in range of 2h
				  //and then subtract h so the end value is
				  //in the range (-h, +h)


				  avg + (rand * 2 * h) - h;
				}
			}

			//generate the diamond values
			//since the diamonds are staggered we only move x
			//by half side
			//NOTE: if the levelx shouldn't wrap then x < DATA_SIZE
			//to generate the far edge values
			
			for (int x = 0; x < DATA_SIZE - 1; x += halfSide)
			{
				//and y is x offset by half a side, but moved by
				//the full side length
				//NOTE: if the levelx shouldn't wrap then y < DATA_SIZE
				//to generate the far edge values
				for (int y = (x + halfSide) % sideLength; y < DATA_SIZE - 1; y += sideLength)
				{
					//x, y is center of diamond
					//note we must use mod  and add DATA_SIZE for subtraction 
					//so that we can wrap around the array to find the corners
					double avg =
					  levelx[(x - halfSide + DATA_SIZE) % DATA_SIZE,y] + //left of center
					  levelx[(x + halfSide) % DATA_SIZE,y] + //right of center
					  levelx[x,(y + halfSide) % DATA_SIZE] + //below center
					  levelx[x,(y - halfSide + DATA_SIZE) % DATA_SIZE]; //above center
					avg /= 4.0;

					//new value = average plus random offset
					//We calculate random value in range of 2h
					//and then subtract h so the end value is
					//in the range (-h, +h)
					double rand = Random.value;
					while (rand < 0.5 && rand > 0.25) { rand = Random.value; }
					avg = avg + (rand * 2 * h) - h;
					//update value for center of diamond
					levelx[x,y] = avg;
					
					//wrap values on the edges, remove
					//this and adjust loop condition above
					//for non-wrapping values.
					if (x == 0) levelx[DATA_SIZE - 1,y] = avg;
					if (y == 0) levelx[x,DATA_SIZE - 1] = avg;
					
				}
			}
			

		}
		
	
	}
	public enum Type
	{
		Grass, Sand, Water, Mountain, Robot
	}

}
