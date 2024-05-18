using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_manager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        cells = this.gameObject.GetComponent<Load_battle>().cell;
        Level = this.gameObject;

        level_canvas = GameObject.Find("level_canvas");
        enemy_damage_number = GameObject.Find("damage_ind_en");
        //characters[0].GetComponent<Samurai>().StartTurn();
        //characters = new GameObject[8];


    }
    public static GameObject[,] cells;
    public static GameObject level_canvas;
    //public static GameObject[] characters;
    public static List<GameObject> characters = new List<GameObject>();        // <---------- 
    public static List<GameObject> activated_cells = new List<GameObject>();
    public static List<GameObject> target_cells = new List<GameObject>();
    public static List<GameObject> path_cells = new List<GameObject>();

    public static int players_count = 1;
    public static int turn_order;
    public static GameObject current_die;
    public static GameObject Level;
    public static GameObject current_skill;
    public static GameObject current_player;
    public static bool using_die;
    public static bool activation;
    public static GameObject move_cell_lock;
    public static GameObject target_lock;

    static int end_turn_timer = 0;

    public static GameObject skill_die;
    public static GameObject move_die;
    public static GameObject slash;
    public static GameObject ranged;
    public static GameObject move;
    public static GameObject dice_manager;
    public static GameObject above_samurai_dice;

    public static GameObject enemy_damage_number;

    public static bool ongoing_animation;

    public static bool stop_skill_activation;
    public static bool stop_cell_activation;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (end_turn_timer > 0) end_turn_timer--;
    }

    public static GameObject AccessCharacter(GameObject cell)
    {
        for (int a = 1; a < characters.Count; a++)
        {
            if (characters[a].GetComponent<Enemy_atributes>().x_coord == cell.GetComponent<Cell>().x && characters[a].GetComponent<Enemy_atributes>().y_coord == cell.GetComponent<Cell>().y) return characters[a];
        }
        return characters[0];
    }

    public static void CheckDice(GameObject dice_used)
    {
        if (dice_used == skill_die)
        {
            StopCellTarget();
            slash.GetComponent<Slash>().die_used_to_slash = null;
            skill_die = null;
        }
        else if (dice_used == move_die)
        {
            StopCellActivation();
            move.GetComponent<Move>().die_used_to_move = null;
            move_die = null;
        }
    }

    public static List<GameObject> GetCellsAround(int x, int y)
    {
        List<GameObject> cells_to_return = new List<GameObject>();

        int object_x = x;
        int object_y = y;

        

        if (object_x + 1 < Battle_manager.cells.GetLength(0)) cells_to_return.Add(cells[object_x + 1, object_y]); // Right cell
        if (object_x - 1 >= 0) cells_to_return.Add(cells[object_x - 1, object_y]); // Left cell
        if (object_y + 1 < Battle_manager.cells.GetLength(1)) cells_to_return.Add(cells[object_x, object_y + 1]); // Top cell
        if (object_y - 1 >= 0) cells_to_return.Add(cells[object_x, object_y - 1]); // Bottom_cell

        if ((object_x + 1 < Battle_manager.cells.GetLength(0)) && (object_y + 1 < Battle_manager.cells.GetLength(1))) cells_to_return.Add(cells[object_x + 1, object_y + 1]); // Top right
        if ((object_x - 1 >= 0) && (object_y + 1 < Battle_manager.cells.GetLength(1))) cells_to_return.Add(cells[object_x - 1, object_y + 1]); // Top left
        if ((object_x - 1 >= 0) && (object_y - 1 >= 0)) cells_to_return.Add(cells[object_x - 1, object_y - 1]); // Bottom left
        if ((object_x + 1 < Battle_manager.cells.GetLength(0)) && (object_y - 1 >= 0)) cells_to_return.Add(cells[object_x + 1, object_y - 1]); // Bottom right

        return cells_to_return;

    }

    public static List<GameObject> GetCellsDiagonal(int object_x, int object_y, string direction, bool ignore_characters, bool ignore_obstacles, int distance)
    {
        List<GameObject> cells_to_return = new List<GameObject>();

        //int object_x = 0;
        //int object_y = 0;

        int dir_x = 0;
        int dir_y = 0;

        switch (direction)
        {
            case "LU":
                dir_y++;
                dir_x--;
                break;
            case "LD":
                dir_x--;
                dir_y++;
                break;
            case "RU":
                dir_x++;
                dir_y++;
                break;
            case "RD":
                dir_y--;
                dir_x++;
                break;
            default:

                break;

        }

        if (direction == "all")
        {
            cells_to_return = CellsInDiagonal(object_x, object_y, 1, 1, ignore_obstacles, ignore_characters, cells_to_return, distance);
            cells_to_return = CellsInDiagonal(object_x, object_y, -1, 1, ignore_obstacles, ignore_characters, cells_to_return, distance);
            cells_to_return = CellsInDiagonal(object_x, object_y, 1, -1, ignore_obstacles, ignore_characters, cells_to_return, distance);
            cells_to_return = CellsInDiagonal(object_x, object_y, -1, -1, ignore_obstacles, ignore_characters, cells_to_return, distance);
        }
        else cells_to_return = CellsInDiagonal(object_x, object_y, dir_x, dir_y, ignore_obstacles, ignore_characters, cells_to_return, distance);

        return cells_to_return;
    }

    static List<GameObject> CellsInDiagonal(int x_coord, int y_coord, int direction_x, int direction_y, bool ignore_o, bool ignore_c, List<GameObject> list_of_cells, int limit)
    {
        int limit_counter = 0;

        for (int a = 1; a <= limit; a++)
        {
            if (x_coord + direction_x * a >= 0 && x_coord + direction_x * a < cells.GetLength(0) && y_coord + direction_y * a >= 0 && y_coord + direction_y * a < cells.GetLength(1))
            {
                
               list_of_cells.Add(cells[x_coord + direction_x * a, y_coord + direction_y * a]);
                if (cells[x_coord + direction_x * a, y_coord + direction_y * a].tag == "cell_occupied" && !ignore_c) break;
                else if (cells[x_coord + direction_x * a, y_coord + direction_y * a].tag == "cell_obstacle" && !ignore_o) break;

            }
            
        }

        return list_of_cells;
    }

        public static List<GameObject> GetCellsLine(int x, int y, string direction, bool ignore_characters, bool ignore_obstacles, int distance)
    {
        List<GameObject> cells_to_return = new List<GameObject>();
        
        int object_x = x;
        int object_y = y;

        int dir_x = 0;
        int dir_y = 0;

        

        switch (direction)
        {
            case "up":
                dir_y++;
                break;
            case "left":
                dir_x--;
                break;
            case "right":
                dir_x++;
                break;
            case "down":
                dir_y--;
                break;
            default:

                break;

        }

        if (direction == "up" || direction == "down")
        {
            cells_to_return = CellsInLine(object_x, object_y, dir_x, dir_y, ignore_obstacles, ignore_characters, cells_to_return, cells.GetLength(1), distance);
        }

        else if (direction == "left" || direction == "right")
        {
            cells_to_return = CellsInLine(object_x, object_y, dir_x, dir_y, ignore_obstacles, ignore_characters, cells_to_return, cells.GetLength(0), distance);
        }

        else if (direction == "all")
        {
            cells_to_return = CellsInLine(object_x, object_y, 0, 1, ignore_obstacles, ignore_characters, cells_to_return, cells.GetLength(1), distance);
            cells_to_return = CellsInLine(object_x, object_y, 0, -1, ignore_obstacles, ignore_characters, cells_to_return, cells.GetLength(1), distance);
            cells_to_return = CellsInLine(object_x, object_y, 1, 0, ignore_obstacles, ignore_characters, cells_to_return, cells.GetLength(0), distance);
            cells_to_return = CellsInLine(object_x, object_y, -1, 0, ignore_obstacles, ignore_characters, cells_to_return, cells.GetLength(0), distance);
        }
        return cells_to_return;
    }

    static List<GameObject> CellsInLine(int x_coord, int y_coord, int direction_x, int direction_y, bool ignore_o, bool ignore_c, List<GameObject> list_of_cells, int limit_field, int limit_other)
    {
        int limit_counter = 0;
        int last_cell_coord_x = 0;
        int last_cell_coord_y = 0;

        for (int a = (direction_x == 0) ? y_coord + direction_y + direction_x : x_coord + direction_y + direction_x; a >= 0 && a < limit_field; a += direction_y + direction_x) //int a = object_y + dir_y; a > 0 && a < cells.GetLength(1); a + dir_y
        {
            if (direction_x == 0)
            {
                list_of_cells.Add(cells[x_coord, a]);
                if (!ignore_c && cells[x_coord, a].tag == "cell_occupied") break;
                else if (!ignore_o && cells[x_coord, a].tag == "cell_obstacle") break;

                last_cell_coord_x = x_coord;
                last_cell_coord_y = a;
            }
            else
            {
                list_of_cells.Add(cells[a, y_coord]);
                if (!ignore_c && cells[a, y_coord].tag == "cell_occupied") break;
                else if (!ignore_o && cells[a, y_coord].tag == "cell_obstacle") break;

                last_cell_coord_x = a;
                last_cell_coord_y = y_coord;
            }
            
            limit_counter++;
            if (limit_counter >= limit_other) break;
        }

        cells[last_cell_coord_x, last_cell_coord_y].GetComponent<Cell>().marked = true;

        return list_of_cells;
    }

    public static void StopCellActivation()
    {
        for (int a = 0; a < activated_cells.Count;)
        {
            activated_cells[a].GetComponent<Cell>().StopActivation();
        }
    }

    public static void DeletePath()
    {
        for (int a = 0; a < path_cells.Count;)
        {
            path_cells[a].GetComponent<Cell>().GetComponent<Cell>().color_lock = false;
            path_cells[a].GetComponent<Cell>().GetComponent<Cell>().Indicator.GetComponent<SpriteRenderer>().color = new Color(0.5f, 1f, 0.5f, 0.5f);
            Battle_manager.path_cells.Remove(path_cells[a]);
        }
    }

    public static void StopCellTarget()
    {
        for (int a = 0; a < target_cells.Count;)
        {
            target_cells[a].GetComponent<Cell>().StopTarget();
        }
    }

    public static int action_points = 2;

    public static void EndTurn(int action_points_cost)
    {
        if (end_turn_timer > 0) return;
        else end_turn_timer = 5;
        target_lock = null;

        if (turn_order < players_count) current_player.GetComponent<Samurai_animation>().Reset();

        if (above_samurai_dice != null)  //                                       <<<<< Could implement this as a separate light_return function in Dice code;
        {
            above_samurai_dice.transform.position = above_samurai_dice.GetComponent<Dice_code>().default_position.transform.position;
            above_samurai_dice.transform.localScale = above_samurai_dice.GetComponent<Dice_code>().normal_size;
        }

        action_points -= action_points_cost;
        Debug.Log("Current action points: " + action_points);
        if (action_points < 1)
        {
            //if (above_samurai_dice != null) Destroy(above_samurai_dice);

            Debug.Log("Global turn ends");
            action_points = 2;

            // Actual turn change

            turn_order++;
            if (turn_order == characters.Count) turn_order = 0;

            current_player = characters[turn_order];

            if (turn_order < players_count) characters[turn_order].GetComponent<Samurai>().StartTurn();
            else if (characters[0].GetComponent<Samurai>().dead == false) characters[turn_order].GetComponent<Enemy_atributes>().StartTurn();
        }

    }
}
