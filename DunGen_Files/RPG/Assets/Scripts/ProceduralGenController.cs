using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenController : MonoBehaviour {

    /*
     * --------------------------------------
     *          PROCEDURAL DUNGEON 
     *           LEVEL GENERATOR
     * --------------------------------------
     * 
     * A class in charge of generating dungeon maps procedurally.
     * Uses Conway's Game Of Life as the basis for its cellular automata model for making smooth, natural-seeming underground chambers
     * TODO:
     *      Add potions - []
     *      Turn "islands" into stacks of crates - []
     *      Make sure the map does not have closed-off rooms - []
     *      Spawn the player and next-level ladder in accessible areas - []
     */



    private enum RoomType {corridor, foursquare, squareSmall, squareBig, };
    private PseudoRandom rand;
    private bool[,] cellmap;

    public string seed;
    public int height;
    public int width;
    public int birthThresh;
    public int starvationLimit;
    public int populationLimit;
    public int cycles;
    public int aliveChance;
    public int currentLevel;
    public GameObject aliveCell;
    public GameObject deadCell;

    public GameObject skeleton;
    public GameObject potion;

    public GameObject levelHolderTemplate;

    public GameObject levelHolderInstance;
    public GameObject tilesHolderInstance;
    public GameObject skeletonsHolderInstance;
    public GameObject potionsHolderInstance;

    // might be more/less per map, for variety, but these are an upper and lower limit
    public int minSkeletons;
    public int maxSkeletons;
    private int skeletons;
    

	void Start () {
        levelHolderInstance = Instantiate(levelHolderTemplate);
        currentLevel = 0;
        initLevel(seed);
    }
	

	void Update () {
        if (Input.GetKeyDown("r")) {
            GameObject.Destroy(levelHolderInstance);
            levelHolderInstance = Instantiate(levelHolderTemplate);
            currentLevel += 1;
            seed = seed + currentLevel;
            initLevel(seed);
        }
	}


    public bool[,] mapInit(bool[,] map) {
        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                //float r = Random.Range(0f, 100f);
                int r = rand.getRandom();
                //Debug.Log(r);
                map[x, y] = (r < aliveChance);
            }
        }

        return map;
    }


    void GenerateUnityMap() {
        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                if(cellmap[x,y] == true) {
                    GameObject t = Instantiate(aliveCell, new Vector3(x,y,0), Quaternion.identity);
                    t.transform.parent = tilesHolderInstance.transform;
                }
                else if (cellmap[x, y] == false) {
                    GameObject t = Instantiate(deadCell, new Vector3(x, y, 0), Quaternion.identity);
                    t.transform.parent = tilesHolderInstance.transform;
                }
                
            }
        }
    }


    public bool[,] doSimulationStep(bool[,] oldMap) {
        bool[,] newMap = new bool[width, height];
        for (int x = 0; x < oldMap.GetLength(0); x++) {
            for (int y = 0; y < oldMap.GetLength(1); y++) {
                int nbs = countAliveNeighbours(oldMap, x, y);
                if (oldMap[x, y]) {
                    if(nbs < starvationLimit) {
                        newMap[x, y] = false;
                    }
                    else if(nbs > populationLimit) {
                        newMap[x, y] = false;
                    }
                    else {
                        newMap[x, y] = true;
                    }
                }
                else {
                    if(nbs > birthThresh) {
                        newMap[x, y] = true;
                    }
                    else {
                        newMap[x, y] = false;
                    }
                }
            }
        }
        return newMap;
    }


    public int countAliveNeighbours(bool[,] map, int x, int y) {
        int count = 0;
        for(int i = -1; i < 2; i++) {
            for(int j = -1; j < 2; j++) {
                int neighbour_x = x + i;
                int neighbour_y = y + j;
                if (i == 0 && j == 0) {
                    // do nothing at this cell's own position
                }
                else if (neighbour_x < 0 || neighbour_y < 0 || neighbour_x >= map.GetLength(0) || neighbour_y >= map.GetLength(1)) {
                    count += 1;
                }
                else if(map[neighbour_x, neighbour_y]) {
                    count += 1;
                }
            }
        }

        return count;
    }


    bool[,] giveBorder(bool[,] map) {
        bool[,] borderedMap = map;
        // fill the top and bottom row completely
        for(int x = 0; x < width; x++) {
            borderedMap[x, 0] = false;
            borderedMap[x, height - 1] = false;
        }
        // fill the initial and final element of every other row
        for(int y = 1; y < height-1; y++) {
            borderedMap[0, y] = false;
            borderedMap[width - 1, y] = false;
        }

        return borderedMap;
    }


    int numberOfSkeletons() {
        int skels = 0;
        skels = rand.getRandom();
        while(skels < (maxSkeletons - minSkeletons)) {
            skels += rand.getRandom();
        }
        skels = minSkeletons + (skels % (maxSkeletons - minSkeletons));
        Debug.Log(skels + " skeletons in this map");
        return skels;
    }


    // Spawn skeletons in random locations along the world map in appropriate locations
    // A location is appropriate if it has no wall-tiles around it i.e it has eight neighbours
    void spawnSkeletons(bool[,] worldMap) {

        // init empty skelMap
        bool[,] skelMap = new bool[width, height];
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                skelMap[x, y] = false;
            }
        }


        int skelsPlaced = 0;

        while(skelsPlaced < skeletons) {
            int x = rand.getRandomRanged(width);
            int y = rand.getRandomRanged(height);
            if(countAliveNeighbours(worldMap, x, y) == 8) {
                skelMap[x, y] = true;
                skelsPlaced++;
            }
        }

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (skelMap[x, y] == true) {
                    GameObject t = Instantiate(skeleton, new Vector3(x, y, 0), Quaternion.identity);
                    t.transform.parent = skeletonsHolderInstance.transform;
                }
            }
        }

    }

    public void initLevel(string seed) {

        rand = GetComponent<PseudoRandom>();
        rand.randomInit(rand.hasher, seed, 2);
        cellmap = new bool[width, height];
        cellmap = mapInit(cellmap);

        for (int i = 0; i < cycles; i++) {
            cellmap = doSimulationStep(cellmap);
        }
        
        potionsHolderInstance = levelHolderInstance.transform.Find("Potions").gameObject;
        skeletonsHolderInstance = levelHolderInstance.transform.Find("Skeletons").gameObject;
        tilesHolderInstance = levelHolderInstance.transform.Find("MapTiles").gameObject;
       
        cellmap = giveBorder(cellmap);
        skeletons = numberOfSkeletons();
        GenerateUnityMap();
        spawnSkeletons(cellmap);
    }

}
