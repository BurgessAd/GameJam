using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGenerator : MonoBehaviour
{

	public List<Sprite> tileSprites = new List<Sprite>();
	private List<Tile> tiles = new List<Tile>();
	public int baseThreshold = 300;
	public int sandThreshold=50;
	public static GameObject player;
	public int grassThreshold=50;
	public int mountainThreshold=30;
	public int mountainUpLim = 40;
	public int mountainSporadicity=69;
	public int robotBaseSporadicity = 100;
	public int mountainSpread=7;
	public static List<GameObject> entities = new List<GameObject>();
	[SerializeField]
	bool change = false;
	public GameObject reactor;
	public GameObject ore;
	public GameObject car;
	public GameObject zombie;
	public GameObject playerReactor;
	public List<Sprite> mountainTileConfigurations = new List<Sprite>();



	// Start is called before the first frame update
	void Start()
    {
		for (int i = 0; i < tileSprites.Count; i++)
        {
			Tile tile = ScriptableObject.CreateInstance<Tile>();
			tile.sprite = tileSprites[i];
			tiles.Add(tile);
        }
		regenerate();	
		//Robot.player = player;
    }


	void placePlayer()
    {
		int rNum = (int)Random.Range(0, Reactor.reactors.Count-1);
		GameObject _playerReactor = Reactor.reactors[rNum];
		_playerReactor.AddComponent<PlayerInputComponent>();
		_playerReactor.GetComponent<ReactorComponent>().SetIsPlayer();
		player = _playerReactor.GetComponent<ReactorComponent>().SpawnPlayerBot();
		playerReactor = _playerReactor;
	}


	void regenerate()
    {
		levelx = new double[DATA_SIZE, DATA_SIZE];
		
		generateLevel();
		int avg = average();
        while (avg < baseThreshold )
        {
			generateLevel();
			avg = average();
        }
		populateTilemap();
		populateMap();
		placePlayer();
	}

    // Update is called once per frame
    void Update()
    {
        if (change)
        {
			change = false;
			while (average() < baseThreshold + sandThreshold + mountainThreshold + grassThreshold&& Reactor.reactors.Count<5&& average()> baseThreshold + sandThreshold + mountainThreshold + 4*grassThreshold)
			{
				generateLevel();
			}
			regenerate();
        }
    }





	public void populateMap()
	{
		for (int i = 0; i < DATA_SIZE-1; i++)
		{
			for (int j = 0; j < DATA_SIZE-1; j++)
			{
				if(i==0 ||j==0 || j == DATA_SIZE - 2 || i == DATA_SIZE - 2)
                {
					tilemap.SetTile(new Vector3Int(i, j, 0), tiles[15]);
					tilemap.SetColliderType(new Vector3Int(i, j, 0), Tile.ColliderType.Sprite);
				}


				if(tilemap.GetTile(new Vector3Int(i, j, 0)) == tiles[0])
                {
                    if (Random.value > 0.999)
                    {
						entities.Add(Instantiate(zombie, new Vector3(tilemap.GetCellCenterWorld(new Vector3Int(i, j, 0)).x, tilemap.GetCellCenterWorld(new Vector3Int(i, j, 0)).y, 0), Quaternion.identity));
					}
					else if (Random.value > 0.999)
                    {
						Instantiate(car, new Vector3(tilemap.GetCellCenterWorld(new Vector3Int(i, j, 0)).x, tilemap.GetCellCenterWorld(new Vector3Int(i, j, 0)).y, 0), Quaternion.Euler(0, 0, Random.Range(0, 360)));
					}
					else if (Random.value > 0.9999)
                    {
						int numOfTilesToReplace = 3;
						for (int k = i - numOfTilesToReplace; k <= i + numOfTilesToReplace && k < DATA_SIZE-1; k++)
						{
							for (int l = j - numOfTilesToReplace; l <= j + numOfTilesToReplace && l < DATA_SIZE-1; l++)
							{
								if (l != j || k != i)
								{
                                    if (l >= 1 && k >= 1)
                                    {
										tilemap.SetTile(new Vector3Int(k, l, 0), tiles[0]);
										tilemap.SetColliderType(new Vector3Int(k, l, 0), Tile.ColliderType.None);
									}

								}

							}
						}
						Reactor.reactors.Add(Instantiate(reactor, new Vector3(tilemap.GetCellCenterWorld(new Vector3Int(i, j, 0)).x, tilemap.GetCellCenterWorld(new Vector3Int(i, j, 0)).y, 0), Quaternion.Euler(0, 0, Random.Range(0, 360))));
					}
					else if (Random.value > 0.995)
					{
						Instantiate(ore, new Vector3(tilemap.GetCellCenterWorld(new Vector3Int(i, j, 0)).x, tilemap.GetCellCenterWorld(new Vector3Int(i, j, 0)).y, 0), Quaternion.Euler(0, 0, Random.Range(0, 360)));
					}
				}
			}
		}
        while (Reactor.reactors.Count < 4)
        {
			for (int i = 0; i < DATA_SIZE-1; i++)
			{
				for (int j = 0; j < DATA_SIZE-1; j++)
				{
					if (tilemap.GetTile(new Vector3Int(i, j, 0)) == tiles[0])
					{
						if (Random.value > 0.9999)
						{
							int numOfTilesToReplace = 3;
							for (int k = i - numOfTilesToReplace; k <= i + numOfTilesToReplace && k < DATA_SIZE-1; k++)
							{
								for (int l = j - numOfTilesToReplace; l <= j + numOfTilesToReplace && l < DATA_SIZE-1; l++)
								{
									if (l != j || k != i)
									{
										if (l >= 1 && k >= 1)
										{
											tilemap.SetTile(new Vector3Int(k, l, 0), tiles[0]);
											tilemap.SetColliderType(new Vector3Int(k, l, 0), Tile.ColliderType.None);
										}
									}

								}
							}
							Reactor.reactors.Add(Instantiate(reactor, new Vector3(tilemap.GetCellCenterWorld(new Vector3Int(i, j, 0)).x, tilemap.GetCellCenterWorld(new Vector3Int(i, j, 0)).y, 0), Quaternion.Euler(0, 0, Random.Range(0, 360))));
						}
					}
				}
			}
		}

	}



	public Tilemap tilemap;
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
		for(int i = 0; i < DATA_SIZE-1; i++)
        {
			for(int j = 0; j < DATA_SIZE-1; j++)
            {

				PointType data = GetPointTypeFromData(levelx[i, j]);
				PointType datax = GetPointTypeFromData(levelx[i+1, j]);
				PointType datay = GetPointTypeFromData(levelx[i, j+1]);
				PointType dataxy = GetPointTypeFromData(levelx[i+1, j+1]);

				Tile tile = GetTileFromData(data, datay, datax, dataxy);
				tilemap.SetTile(new Vector3Int(i, j, 0), tile);
                if (data.Equals(PointType.mountains) || datax.Equals(PointType.mountains) || datay.Equals(PointType.mountains) || dataxy.Equals(PointType.mountains))
                {
					tilemap.SetColliderType(new Vector3Int(i, j, 0), Tile.ColliderType.Sprite);
                }
				else
				{
					tilemap.SetColliderType(new Vector3Int(i, j, 0), Tile.ColliderType.None);
				}
            }
        }
    }

	private Tile GetTileFromData(PointType x0y0, PointType x0y1, PointType x1y0, PointType x1y1)
	{
		int bitFlag = 0;
		if (x0y0.Equals(PointType.mountains))
		{
			bitFlag |= (1 << 0);
		}
		if (x0y1.Equals(PointType.mountains))
		{
			bitFlag |= (1 << 1);
		}
		if (x1y0.Equals(PointType.mountains))
		{
			bitFlag |= (1 << 2);
		}
		if (x1y1.Equals(PointType.mountains))
		{
			bitFlag |= (1 << 3);
		}
		return tiles[bitFlag];
	}


	private PointType GetPointTypeFromData(double data)
    {

		if (data < baseThreshold)
		{
			//if()
   //         {
                
				//tilemap.SetColliderType(new Vector3Int(x, y, 0), Tile.ColliderType.Sprite);
				return PointType.mountains;
			//}	
        }
		return PointType.sand;
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
	public enum PointType
	{
		mountains, sand
	}

}
