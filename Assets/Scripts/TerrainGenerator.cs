using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGenerator : MonoBehaviour
{

	public Sprite grassTileSprite;
	public Sprite sandTileSprite;
	public Sprite seaTileSprite;
	public Sprite mountainTileSprite;

    // Start is called before the first frame update
    void Start()
    {
		Tile grassTile = new Tile();
		grassTile.sprite = grassTileSprite;
		tiles.Add(Type.Grass, grassTile);

		Tile sandTile = new Tile();
		sandTile.sprite = sandTileSprite;
		tiles.Add(Type.Sand, sandTile);

		Tile seaTile = new Tile();
		seaTile.sprite = seaTileSprite;
		tiles.Add(Type.Water, seaTile);


		Tile mountainTile = new Tile();
		mountainTile.sprite = mountainTileSprite;
		tiles.Add(Type.Mountain, mountainTile);


		generateLevel();
		populateTilemap();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public Tilemap tilemap;
	public Dictionary<Type, Tile> tiles = new Dictionary<Type, Tile>();
	static int DATA_SIZE = 257;
	public static double[,] levelx = new double[DATA_SIZE,DATA_SIZE];


	public void populateTilemap()
    {
		for(int i = 0; i < DATA_SIZE; i++)
        {
			for(int j = 0; j < DATA_SIZE; j++)
            {
				double data = levelx[i, j];

				tilemap.SetTile(new Vector3Int(i, j, 0),GetTileFromData(data));
            }
        }
    }



	private Tile GetTileFromData(double data)
    {
		if(data>=600 && data <= 735)
        {
			return tiles[Type.Sand];
        }
		else if (data > 735 && data < 750)
		{
           
			return tiles[Type.Mountain];
			
            
			
		}
		else if(data>735)
        {
            if ((int)data % 69 <= 7)
            {
				return tiles[Type.Mountain];
			}
			return tiles[Type.Grass];
        }
		
        else
        {
			return tiles[Type.Water];
        }
    }


	public static void generateLevel()
	{
		//for (int i = 0; i < Types.length; i++)
		//{
		//	tileSetImgs.put(Types[i], GameEntity.getImage("tileset/" + Types[i] + ".png"));
		//}




		//size of grid to generate, note this must be a
		//value 2^n+1
		 
		//an initial seed value for the corners of the levelx
		 double SEED = 1000.0;
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
		Debug.Log("Complete");
	
	}
	public enum Type
	{
		Grass, Dirt, Sand, Water, Lava, Stairs, Mountain
	}

}
