using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BSPLevelGenController : MonoBehaviour {

    // Logic for splitting the map a-la Binary Space Partition
    private enum SplitDirection {HORIZONTAL, VERTICAL};
    private SplitDirection DIR;
    private PseudoRandom rand;
    private bool[,] levelMap;
    public int minSize;
    public string seed;
    public int height;
    public int width;
    public int currentLevel;

    // for spawnning enemies & items
    public int hardMin_enemy;
    public int hardMax_enemy;
    public int hardMin_item;
    public int hardMax_item;
    public int hardMin_stains;
    public int hardMax_stains;

    // for tracking stats of how many enemies and items were killed/collected out of all that exist
    private int mobGhosts;
    private int mobWizards;
    private int mobOgres;
    private int healthPotions;
    private int manaPotions;
    private int floorDecorations;
    private int wallDecorations;


    // for drawing up the map
    public GameObject floorCell;
    public GameObject wallTile_plain;
    public GameObject wallTile_bot;
    public GameObject wallTile_top;
    public GameObject wallTile_side;
    public GameObject wallTile_oneNeighbor;
    public GameObject wallTile_twoNeighbors_opposite;
    public GameObject wallTile_threeNeighbors;
    public GameObject floorDecor_dirt;

    // entities
    public GameObject ghost;
    public GameObject wizard;
    public GameObject ogre;
    public GameObject healthPotion;
    public GameObject manaPotion;

    // for convenience during development + for later optimization through batching
    public GameObject holderTemplate;
    public GameObject tilesHolderInstance;
    public GameObject enemyHolderInstance;
    public GameObject potionsHolderInstance;

    // level flow control
    public GameObject startPoint;
    public GameObject exitLadder;
    public GameObject player;

    // what level should each enemy start showing up at
    public int minLevel_ghost;
    public int minLevel_wizard;
    public int minLevel_ogre;

    // The UI at the end of each level
    public GameObject LevelEndUI;
    public LevelEndUIController UIController;

    // for score calculation
    public int enemyKilledPoints;
    public int potionDrankPoints;



    void Start () {
        PlayerPrefs.SetInt("GameScore", 0);
        LevelEndUI.SetActive(false);
        // get the seed from the playerPrefs. If none, assume the name is Shantanu and store it cuz why not

        seed = PlayerPrefs.GetString("Name");
        if(seed.Length == 0) {
            seed = "ThankGodForJimSterling";
        }

        // get references in-script
        player = GameObject.FindGameObjectWithTag("Player");


        // start-of-game conditions
        currentLevel = 0; 
        DIR = SplitDirection.HORIZONTAL; // starter
        BSP_initLevel();
    }   
	

    public void BSP_initLevel () {
        LevelEndUI.SetActive(false);
        // destroy previous level if there is one, and make a new one.
        if (currentLevel != 0) {
            GameObject.Destroy(tilesHolderInstance);
            GameObject.Destroy(enemyHolderInstance);
            GameObject.Destroy(potionsHolderInstance);
            print(generateStats());
        }
        tilesHolderInstance = Instantiate(holderTemplate);
        tilesHolderInstance.transform.name = "Tiles Holder";

        enemyHolderInstance = Instantiate(holderTemplate);
        enemyHolderInstance.transform.name = "Enemy Holder";

        potionsHolderInstance = Instantiate(holderTemplate);
        potionsHolderInstance.transform.name = "Potions Holder";

        currentLevel++;
        updateSeed();
        
        rand = GetComponent<PseudoRandom>();
        rand.randomInit(rand.hasher, seed, 2);
        levelMap = new bool[width, height];  
        levelMap = mapInit(levelMap);
        levelMap = partition(levelMap, 1, height - 2, 1, width - 2);
        setSpawnCounts();
        // once logic'd out, draw it as a Unity level, put the player in, and let's dance.
        GenerateUnityMap();
        placeStartAndExit();
    }

    void Update() {
        if (Input.GetKeyDown("f")) {
            BSP_initLevel();
        }
    }

    //draw generated map in Unity. This is before we replace some walls with decorative walls and some floors with painted floors
    void GenerateUnityMap() {

        for (int x = 1; x < width - 1; x++) {
            for (int y = 1; y < height - 1; y++) {
                GameObject t;
                if (levelMap[x, y] == true) { // if wall
                    // case 1: this is non-terminating piece of a horizontal wall
                    if (levelMap[x + 1, y] == true && levelMap[x - 1, y] == true && levelMap[x, y + 1] == false && levelMap[x, y - 1] == false) {
                        t = Instantiate(wallTile_twoNeighbors_opposite, new Vector3(x, y, 0), Quaternion.identity);
                    }
                    // case 2: non-terminatin piece of vertical wall
                    else if (levelMap[x + 1, y] == false && levelMap[x - 1, y] == false && levelMap[x, y + 1] == true && levelMap[x, y - 1] == true) {
                        t = Instantiate(wallTile_twoNeighbors_opposite, new Vector3(x, y, 0), Quaternion.Euler(0, 0, 90));
                    }
                    // case 3: end piece, wall on left
                    else if (levelMap[x + 1, y] == true && levelMap[x - 1, y] == false && levelMap[x, y + 1] == false && levelMap[x, y - 1] == false) {
                        t = Instantiate(wallTile_threeNeighbors, new Vector3(x, y, 0), Quaternion.Euler(0, 0, 180));
                    }
                    // case 4: end piece, wall on right
                    else if (levelMap[x + 1, y] == false && levelMap[x - 1, y] == true && levelMap[x, y + 1] == false && levelMap[x, y - 1] == false) {
                        t = Instantiate(wallTile_threeNeighbors, new Vector3(x, y, 0), Quaternion.identity);
                    }
                    // case 5: end piece, wall on top
                    else if (levelMap[x + 1, y] == false && levelMap[x - 1, y] == false && levelMap[x, y + 1] == true && levelMap[x, y - 1] == false) {
                        t = Instantiate(wallTile_threeNeighbors, new Vector3(x, y, 0), Quaternion.Euler(0, 0, 270));
                    }
                    // case 6: end piece, wall on bottom
                    else if (levelMap[x + 1, y] == false && levelMap[x - 1, y] == false && levelMap[x, y + 1] == false && levelMap[x, y - 1] == true) {
                        t = Instantiate(wallTile_threeNeighbors, new Vector3(x, y, 0), Quaternion.Euler(0, 0, 90));
                    }
                    // Only one neighboring floor, which is at top
                    else if (levelMap[x + 1, y] == true && levelMap[x - 1, y] == true && levelMap[x, y + 1] == false && levelMap[x, y - 1] == true) {
                        t = Instantiate(wallTile_oneNeighbor, new Vector3(x, y, 0), Quaternion.Euler(0, 0, 180));
                        //string wallName = "wall " + x + ", " + y;
                        //t.transform.name = wallName;
                    }
                    // Only one neighboring floor, which is at botton
                    else if (levelMap[x + 1, y] == true && levelMap[x - 1, y] == true && levelMap[x, y + 1] == true && levelMap[x, y - 1] == false) {
                        t = Instantiate(wallTile_oneNeighbor, new Vector3(x, y, 0), Quaternion.identity);
                    }
                    // Only one neighboring floor, which is at right
                    else if (levelMap[x + 1, y] == false && levelMap[x - 1, y] == true && levelMap[x, y + 1] == true && levelMap[x, y - 1] == true) {
                        t = Instantiate(wallTile_oneNeighbor, new Vector3(x, y, 0), Quaternion.Euler(0, 0, 90));
                    }
                    // Only one neighboring floor, which is at right
                    else if (levelMap[x + 1, y] == true && levelMap[x - 1, y] == false && levelMap[x, y + 1] == true && levelMap[x, y - 1] == true) {
                        t = Instantiate(wallTile_oneNeighbor, new Vector3(x, y, 0), Quaternion.Euler(0, 0, 270));
                    }
                    else {
                        t = Instantiate(wallTile_plain, new Vector3(x, y, 0), Quaternion.identity);
                    }
                    string wallName = "wall " + x + ", " + y;
                    t.transform.name = wallName;
                    t.transform.parent = tilesHolderInstance.transform;
                }
                else if (levelMap[x, y] == false) { // if floor
                    t = Instantiate(floorCell, new Vector3(x, y, 0), Quaternion.identity);
                    //t.transform.parent = tilesHolderInstance.transform;
                    string floorName = "floor " + x + ", " + y;
                    t.transform.name = floorName;
                    t.transform.parent = tilesHolderInstance.transform;
                }


            }
        }

        // put a few patches of dirt down lmao
        int dirtCount = 0;  
        while (dirtCount < floorDecorations) {
            int dirtX = rand.getRandint(2, width - 4), dirtY = rand.getRandint(2, height - 5);
            if (
                   levelMap[dirtX, dirtY] == false
                && levelMap[dirtX, dirtY + 1] == false
                && levelMap[dirtX + 1, dirtY] == false
                && levelMap[dirtX + 1, dirtY + 1] == false
                && levelMap[dirtX + 2, dirtY] == false
                && levelMap[dirtX + 2, dirtY + 1] == false
                ) {
                dirtCount++;
                GameObject d = Instantiate(floorDecor_dirt, new Vector3(dirtX, dirtY, 0), Quaternion.identity);
                d.transform.parent = tilesHolderInstance.transform;
            }
        }


        // draw top and bottom wall
        for (int x = 0; x < width; x++) {
            GameObject bWall = Instantiate(wallTile_bot, new Vector3(x, 0, 0), Quaternion.identity);
            GameObject tWall = Instantiate(wallTile_top, new Vector3(x, height - 1, 0), Quaternion.identity);
            string bName = "bottom wall " + x;
            string tName = "top wall" + x;
            bWall.transform.name = bName;
            tWall.transform.name = tName;
            bWall.transform.parent = tilesHolderInstance.transform;
            tWall.transform.parent = tilesHolderInstance.transform;
        }

        // draw left and right wall
        // We don't draw over the bottom walls drawn at corners so we start counting from y=1
        for (int y = 1; y < height; y++) {
            GameObject lWall = Instantiate(wallTile_side, new Vector3(0, y, 0), Quaternion.Euler(0, 0, 180));
            GameObject rWall = Instantiate(wallTile_side, new Vector3(width - 1, y, 0), Quaternion.identity);
            string lName = "left wall" + y;
            string rName = "right wall" + y;
            lWall.transform.name = lName;
            rWall.transform.name = rName;
            lWall.transform.parent = tilesHolderInstance.transform;
            rWall.transform.parent = tilesHolderInstance.transform;
        }

        // finally lay down some enemies and some potions
        if (currentLevel >= minLevel_ghost) {
            spawnObject(ghost, enemyHolderInstance, 1, mobGhosts);
        }
        if (currentLevel >= minLevel_wizard) {
            spawnObject(wizard, enemyHolderInstance, 1, mobWizards);
        }
        if (currentLevel >= minLevel_ogre) {
            spawnObject(ogre, enemyHolderInstance, 2, mobOgres);
        }

        spawnObject(healthPotion, potionsHolderInstance, 1, healthPotions);
        spawnObject(manaPotion, potionsHolderInstance, 1, manaPotions);
    }

    
    public bool[,] mapInit(bool[,] map) {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++)
                map[x, y] = false;
        }
        return map;
    }


    // Recursively split the map into two halves until the larger room you create is the minimum allowed size
    bool[,] partition(bool[,] map, int start_x, int end_x, int start_y, int end_y) {
        if(DIR == SplitDirection.HORIZONTAL && (end_y - start_y) <= minSize){
            //Debug.Log("Exit Condition 1");
            return map;
        }
        else if (DIR == SplitDirection.VERTICAL && (end_x - start_x) <= minSize) {
            //Debug.Log("Exit Condition 2");
            return map;
        }
        else {
            //Debug.Log("Recursing");
            if(DIR == SplitDirection.HORIZONTAL) {
                DIR = SplitDirection.VERTICAL;
                int split_y = rand.getRandint(start_y+1, end_y-1);

                // can probably be made cleaner. Will look into
                while(!((start_y + 1) < split_y && split_y < (end_y - 1))) {
                    split_y = rand.getRandint(start_y, end_y);
                }

                for(int i = start_x; i < (end_x + 1); i++) {
                    map[split_y, i] = true;
                }

                // put two doors in at either end.
                // It is possible to do this with trees,
                // But this is a much quicker solution that is 95% less complicated
                //map[split_y,start_x + 1] = false;
                //map[split_y, end_x - 1] = false;

                map[split_y, rand.getRandint(start_x+1, start_x + 2)] = false;
                map[split_y, rand.getRandint(end_x - 2, end_x-1)] = false;

                //if (split_y - start_y >= end_y - split_y) {
                //    return partition(map, start_x, end_x, start_y, (split_y - 1));
                //}
                //else {
                //    return partition(map, start_x, end_x, (split_y + 1), end_y);
                //}
                //return partition(map, start_x, end_x, start_y, (split_y - 1));
                //return partition(map, start_x, end_x, (split_y + 1), end_y);
                return mergedMaps(partition(map, start_x, end_x, start_y, (split_y - 1)), partition(map, start_x, end_x, (split_y + 1), end_y));

            }
            else if (DIR == SplitDirection.VERTICAL) {
                DIR = SplitDirection.HORIZONTAL;
                int split_x = rand.getRandint(start_x, end_x);

                // again, I think there's a cleaner way to do this next loop part but this works
                while(!((start_x + 1) < split_x && split_x < (end_x - 1))) {
                    split_x = rand.getRandint(start_x, end_x);
                }
                for(int i = start_y; i < (end_y + 1); i++) {
                    map[i, split_x] = true;
                }

                // again we put some doorways
                //map[start_y + 1, split_x] = false;
                //map[end_y - 1, split_x] = false;
                map[rand.getRandint(start_y+1, start_y + 2), split_x] = false;
                map[rand.getRandint(end_y-2, end_y - 1), split_x] = false;

                //if(split_x - start_x >= end_x - split_x) {
                //    return partition(map, start_x, (split_x - 1), start_y, end_y);
                //}
                //else {
                //    return partition(map, (split_x + 1), end_x, start_y, end_y);
                //}
                //return partition(map, start_x, (split_x - 1), start_y, end_y);
                //return partition(map, (split_x + 1), end_x, start_y, end_y);
                return mergedMaps(partition(map, start_x, (split_x - 1), start_y, end_y), partition(map, (split_x + 1), end_x, start_y, end_y));
            }
            else {
                Debug.Log("This literally can't happen");
                return map;
            }
        }
    }

    bool[,] mergedMaps(bool[,] map1, bool[,] map2) {
        bool[,] sum = new bool[width, height];
        for(int i = 0; i < width; i++) {
            for(int j = 0; j < height; j++) {
                sum[i, j] = (map1[i, j] || map2[i, j]);
            }
        }

        return sum;
    }

    void setSpawnCounts() {
        mobGhosts = rand.getRandint(hardMin_enemy, hardMax_enemy);
        mobOgres = rand.getRandint(hardMin_enemy, hardMax_enemy);
        mobWizards = rand.getRandint(hardMin_enemy, hardMax_enemy);

        healthPotions = rand.getRandint(hardMin_item, hardMax_item);
        manaPotions = rand.getRandint(hardMin_item, hardMax_item);

        floorDecorations = rand.getRandint(hardMin_stains, hardMax_stains);  
    }

    // spawner for enemies, potions etc. Size is how many cells in both directions it takes (we assume everything is a sqaure
    public void spawnObject(GameObject thing, GameObject thingHolder, int size, int count) {
        int spawned = 0;
        int spawnX, spawnY;
        while (spawned < count) {
            spawnX = rand.getRandint(size + 2, width - size - 2);
            spawnY = rand.getRandint(size + 2, height - size - 2);
            if(size != 1) {
                bool spaceAvailable = true;
                for(int i = 0; i < size; i++) {
                    for(int j = 0; j < size; j++) {
                        if (levelMap[spawnX+i, spawnY+j] == true) {
                            spaceAvailable = false;
                        }
                    }
                }
                if (spaceAvailable) {
                    GameObject spawnedItem = GameObject.Instantiate(thing, new Vector3(spawnX, spawnY, 0), Quaternion.identity);
                    spawnedItem.transform.parent = thingHolder.transform;
                    spawned++;
                }

            }
            else {
                if(levelMap[spawnX, spawnY] == false) {
                    GameObject spawnedItem = GameObject.Instantiate(thing, new Vector3(spawnX, spawnY, 0), Quaternion.identity);
                    spawnedItem.transform.parent = thingHolder.transform;
                    spawned++;
                }
            }

        }
    }

    // take the seed and increment 
    void updateSeed() {
        string newSeed = seed.Substring(0, seed.Length - (currentLevel-1).ToString().Length);
        newSeed += currentLevel;
        seed = newSeed;
        Debug.Log("New seed: " + seed);
    }


    void placeStartAndExit() {
        // deactivate the player, startpoint, endpoint while calculating and moving stuff while moving the start point.
        player.SetActive(false);
        startPoint.SetActive(false);
        exitLadder.SetActive(false);
        // put Start Point at a spot in the lower left of the map that isn't a wall
        int startX = 1, startY = 1;
        while(levelMap[startX, startY]) {
            startX++;
            if (!levelMap[startX, startY]) {
                break;
            }
            startY++;
            if(!levelMap[startX, startY]) {
                break;
            }
        }

        int ladderX = rand.getRandint(width/2, width - 2), ladderY = rand.getRandint(height/2, height - 2);

        while(levelMap[ladderX, ladderY]) {
            ladderX = rand.getRandint(width / 2, width - 2);
            if(!levelMap[ladderX, ladderY]) {
                break;
            }
            ladderY = rand.getRandint(height / 2, height - 2);
            if (!levelMap[ladderX, ladderY]) {
                break;
            }
        }
        // activate the ladder
        exitLadder.transform.position = new Vector3(ladderX, ladderY, 0);
        exitLadder.SetActive(true);
    
        // activate the startpoint and put the player there
        startPoint.transform.position = new Vector3(startX, startY, 0);
        startPoint.SetActive(true);    
        startPoint.GetComponent<PlayerStartPoint>().StartPointInit();
        player.SetActive(true);
    }
    
    public LevelStats generateStats() {
        LevelStats stats = new LevelStats();
        if(currentLevel >= minLevel_ghost) {
            stats.totalMonsters += mobGhosts;
        }
        if(currentLevel >= minLevel_wizard) {
            stats.totalMonsters += mobWizards;
        }
        if(currentLevel >= minLevel_ogre) {
            stats.totalMonsters += mobOgres;
        }
        stats.killedMonsters = stats.totalMonsters - GameObject.FindGameObjectsWithTag("Enemy").Length;

        stats.totalHealthPotions = healthPotions;
        stats.drankHealthPotions = stats.totalHealthPotions - GameObject.FindGameObjectsWithTag("HealthPotion").Length;

        stats.totalManaPotions = manaPotions;
        stats.drankManaPotions = stats.totalManaPotions - GameObject.FindGameObjectsWithTag("ManaPotion").Length;

        stats.levelScore = (stats.killedMonsters * enemyKilledPoints) + ((stats.drankHealthPotions + stats.drankManaPotions) * potionDrankPoints);


        Debug.Log("Killed " + stats.killedMonsters + " out of " + stats.totalMonsters);
        Debug.Log("Score is " + stats.levelScore);

        return stats;
    }

    // pause the game and show the UI for the stats on this level
    public void levelEndShowUI() {
        int tmp_score = PlayerPrefs.GetInt("GameScore");
        tmp_score += generateStats().levelScore;
        PlayerPrefs.SetInt("GameScore", tmp_score);
        LevelStats lvl_s = generateStats();
        int potions_drank = lvl_s.drankHealthPotions + lvl_s.drankManaPotions;
        int potions_total = lvl_s.totalHealthPotions + lvl_s.totalManaPotions;
        UIController.GetComponent<LevelEndUIController>().showStats(lvl_s.killedMonsters, lvl_s.totalMonsters, potions_drank, potions_total, PlayerPrefs.GetInt("GameScore"));
        LevelEndUI.SetActive(true);
        Time.timeScale = 0f;
    }

}
