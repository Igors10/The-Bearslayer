using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load_battle : MonoBehaviour
{
    // Temporary 

    [SerializeField] GameObject ccamera;
    [SerializeField] GameObject rock;
    public static GameObject defence_mod;
    public static GameObject resist_mod;
    //
    
    [SerializeField] GameObject cell_prefab;
    [SerializeField] GameObject samurai;
    [SerializeField] GameObject grid;
    public int map_width;
    public int map_height;
    public GameObject[,] cell;

    // Start is called before the first frame update
    void Awake()
    {
        

    }
    void Start()
    {
        defence_mod = GameObject.Find("defence_modifier_lol");
        defence_mod.SetActive(false);

        resist_mod = GameObject.Find("resist_modifier_lol");
        resist_mod.SetActive(false);

        cell = new GameObject[map_width, map_height];

        float starting_x = 0f;
        float x_adj;
        float starting_y;
        
        // DRAWING GRID

        for (int a = 0; a < map_width; a++)
        {
            starting_y = -1.5f;
            x_adj = 0f;
            for (int b = 0; b < map_height; b++)
            {
                GameObject current_cell;
                current_cell = Instantiate(cell_prefab, new Vector3(starting_x + x_adj, starting_y, 0f), Quaternion.identity, grid.transform);

                current_cell.GetComponent<Cell>().x = a;
                current_cell.GetComponent<Cell>().y = b;

                cell[a, b] = current_cell;
                
                for (int c = 0; c < Battle_list.obstacle_coordinate.Length; c++)
                {
                    if ((float)b == Battle_list.obstacle_coordinate[c].y && (float)a == Battle_list.obstacle_coordinate[c].x) Placing_obstacle(current_cell, b);
                                    //else Debug.Log("nah " + b + ":" + a + " is not equal to " + Battle_list.obstacle_coordinate[c].y + " : " + Battle_list.obstacle_coordinate[c].x);
                }

                starting_y += 0.5f;
                x_adj += 0.75f;
            }

            starting_x += 1.5f;
        }

        // SPAWNING CHARACTERS

        samurai.transform.position = new Vector3(cell[(int)Battle_list.samurai_spawn.x, (int)Battle_list.samurai_spawn.y].transform.position.x, cell[(int)Battle_list.samurai_spawn.x, (int)Battle_list.samurai_spawn.y].transform.position.y, 0f);
        
        Camera.main.transform.position = new Vector3(cell[(int)Battle_list.samurai_spawn.x, (int)Battle_list.samurai_spawn.y].transform.position.x, cell[(int)Battle_list.samurai_spawn.x, (int)Battle_list.samurai_spawn.y].transform.position.y + 2f, -10.0f);
        cell[(int)Battle_list.samurai_spawn.x, (int)Battle_list.samurai_spawn.y].tag = "cell_occupied";
        Samurai_stats.samurai_cell_x = (int)Battle_list.samurai_spawn.x;
        Samurai_stats.samurai_cell_y = (int)Battle_list.samurai_spawn.y;

        Battle_manager.characters.Add(samurai);
        //Debug.Log("Samurai transported to " + new Vector3(cell[(int)Battle_list.samurai_spawn.x, (int)Battle_list.samurai_spawn.y].transform.position.x, cell[(int)Battle_list.samurai_spawn.x, (int)Battle_list.samurai_spawn.y].transform.position.y, 0f));

        Camera_movement.FocusOnSamurai();

        // SPAWNING ENEMIES

        for (int a = 0; a < Battle_list.enemy_list.Length; a++)
        {
            
            // Dont forget to make cells occupied
            Battle_manager.characters.Add(Instantiate(Battle_list.enemy_types[(int)Battle_list.enemy_list[a].z], cell[(int)Battle_list.enemy_list[a].x, (int)Battle_list.enemy_list[a].y].transform.position, Quaternion.identity));
            cell[(int)Battle_list.enemy_list[a].x, (int)Battle_list.enemy_list[a].y].tag = "cell_occupied";
            Battle_manager.characters[a + Battle_manager.players_count].GetComponent<Enemy_atributes>().Sprite.sortingOrder = 20 - (int)Battle_list.enemy_list[a].y;
            Battle_manager.characters[a + Battle_manager.players_count].GetComponent<Enemy_atributes>().x_coord = (int)Battle_list.enemy_list[a].x;
            Battle_manager.characters[a + Battle_manager.players_count].GetComponent<Enemy_atributes>().y_coord = (int)Battle_list.enemy_list[a].y;
        }

    }

    void Placing_obstacle(GameObject cell, int sorting_order)
    {
        GameObject obstacle;
        obstacle = Instantiate(rock, new Vector3(cell.transform.position.x +0.1f, cell.transform.position.y +0.8f, 0f), Quaternion.identity, grid.transform);
        obstacle.GetComponent<SpriteRenderer>().sortingOrder = 20 - sorting_order;
        cell.tag = "cell_obstacle";
        //Destroy(cell.gameObject);  // <-- Destroying
        //Debug.Log("I deleted a cell");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
