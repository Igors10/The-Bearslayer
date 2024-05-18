using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    // Start is called before the first frame update
    
    public GameObject die_used_to_move;

    public GameObject glowing;
    [SerializeField] GameObject capsule;

    GameObject[,] cell;
    int movement_distance;

    void Start()
    {
        cell = Battle_manager.cells;
        Battle_manager.move = this.gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void PrimarMove()
    {
        
        //glowing.SetActive(false);
        movement_distance = die_used_to_move.GetComponent<Dice_code>().value;
        if (die_used_to_move.GetComponent<Dice_code>().element == "magic") movement_distance -= 2;

        die_used_to_move.transform.position = transform.position;

        capsule.GetComponent<Capsule_code>().Recolor(1); // <----
        
        if (movement_distance < 1)
        {
            List<GameObject> cells_around = Battle_manager.GetCellsAround(Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y);
            for (int a = 0; a < cells_around.Count; a++)
            {
                cells_around[a].GetComponent<Cell>().Ready_to_move(die_used_to_move);
            }

        }

        List<GameObject> last_cells = new List<GameObject>();

        bool ignoring_obstacles = (die_used_to_move.GetComponent<Dice_code>().element == "dark") ? true : false;
        bool ignoring_characters = (die_used_to_move.GetComponent<Dice_code>().element == "wind") ? true : false;
 
        List<GameObject> cells_move_to = Battle_manager.GetCellsLine(Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y, "all", ignoring_characters, ignoring_obstacles, movement_distance); //(GameObject start_object, int start_object_type, string direction, bool ignore_characters, bool ignore_obstacles, int distance)

        for (int a = 0; a < cells_move_to.Count; a++)
        {
            cells_move_to[a].GetComponent<Cell>().Ready_to_move(die_used_to_move);
            if (cells_move_to[a].GetComponent<Cell>().marked)
            {
                last_cells.Add(cells_move_to[a]);
                cells_move_to[a].GetComponent<Cell>().marked = false;
            }
        }

        

        if (die_used_to_move.GetComponent<Dice_code>().element == "water" || die_used_to_move.GetComponent<Dice_code>().element == "magic")
        {
            //Debug.Log(last_cells.Count);
            for (int a = 0; a < last_cells.Count; a++)
            {
                if (die_used_to_move.GetComponent<Dice_code>().element == "magic" && movement_distance < 1) break;
                if (last_cells[a] != null)
                {
                    for (int x = -1; x < 2; x++)
                    {
                        for (int y = -1; y < 2; y++)
                        {
                            int used_x = x;
                            int used_y = y;

                            if ((a == 0 || a == 1) && die_used_to_move.GetComponent<Dice_code>().element == "water") used_y = 0;
                            else if ((a == 2 || a == 3) && die_used_to_move.GetComponent<Dice_code>().element == "water") used_x = 0;

                            if (last_cells[a].GetComponent<Cell>().x + used_x < Battle_manager.cells.GetLength(0) && last_cells[a].GetComponent<Cell>().x + used_x >= 0 && last_cells[a].GetComponent<Cell>().y + used_y < Battle_manager.cells.GetLength(1) && last_cells[a].GetComponent<Cell>().y + used_y >= 0)
                            {
                                Battle_manager.cells[last_cells[a].GetComponent<Cell>().x + used_x, last_cells[a].GetComponent<Cell>().y + used_y].GetComponent<Cell>().Ready_to_move(die_used_to_move);
                            }
                        }
                    }
                }
            }
            
        }
        

        //die_used_to_move = null;
        Debug.Log(Battle_manager.activated_cells.Count + " cells activated");
    }

    void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "dice")
        {
            glowing.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "dice" && die_used_to_move == null)
        {
            glowing.SetActive(false);
        }
    }

    void OnTriggerStay2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "dice" && Battle_manager.activation)
        {
            glowing.SetActive(true);
            //if (obj.gameObject.GetComponent<Dice_code>().set) return; // it is not set that's why it is still working
            if (obj.gameObject == die_used_to_move) return;

            if (Battle_manager.skill_die != null) Battle_manager.skill_die.GetComponent<Dice_code>().ReturnBack();

            if (die_used_to_move != null) //Battle_manager.activated_cells.Count > 0
            {
                die_used_to_move.GetComponent<Dice_code>().ReturnBack();
                die_used_to_move = null;

                Battle_manager.StopCellActivation();
                //Battle_manager.stop_cell_activation = true; // sdelaj etu funkciju normalno !!!
                /*
                Battle_manager.activation = false;
                return;*/
                Debug.Log("dolzhen il vernutsa");
                //die_used_to_move.GetComponent<Dice_code>().ReturnBack();
                //die_used_to_move.transform.position = die_used_to_move.GetComponent<Dice_code>().default_position.transform.position;
            }

            Battle_manager.CheckDice(obj.gameObject);
            //capsule.GetComponent<Capsule_code>().Stop_skill_activation(die_used_to_move);
            die_used_to_move = obj.gameObject;
            Battle_manager.move_die = obj.gameObject;
            Debug.Log("Added to move");

            // ---------------------------------

            PrimarMove();

            Battle_manager.activation = false;

            // ---------------------------------


        }
    }
}



/*
for (int a = 0; a < 4; a++)
{
    last_cells[a] = null;
}

for (int a = 1; a < movement_distance + 1; a++)
{
    if (Samurai_stats.samurai_cell_x + a < Battle_manager.cells.GetLength(0))
    {
        if (Battle_manager.cells[Samurai_stats.samurai_cell_x + a, Samurai_stats.samurai_cell_y].tag == "cell_empty" && right_obstacle == false)
        {
            Battle_manager.cells[Samurai_stats.samurai_cell_x + a, Samurai_stats.samurai_cell_y].GetComponent<Cell>().Ready_to_move(die_used_to_move);  //          <---- Implement this function!
            last_cells[0] = Battle_manager.cells[Samurai_stats.samurai_cell_x + a, Samurai_stats.samurai_cell_y];
            //if (Samurai_stats.samurai_cell_x + (a + 1) > Battle_manager.cells.GetLength(0)) right_obstacle = true;
        }
        else
        {
            if ((die_used_to_move.GetComponent<Dice_code>().element != "dark" && Battle_manager.cells[Samurai_stats.samurai_cell_x + a, Samurai_stats.samurai_cell_y].tag == "cell_obstacle") || (die_used_to_move.GetComponent<Dice_code>().element != "wind" && Battle_manager.cells[Samurai_stats.samurai_cell_x + a, Samurai_stats.samurai_cell_y].tag == "cell_occupied")) right_obstacle = true;

        }
    }

    if (Samurai_stats.samurai_cell_x - a >= 0)
    {
        if (Battle_manager.cells[Samurai_stats.samurai_cell_x - a, Samurai_stats.samurai_cell_y].tag == "cell_empty" && left_obstacle == false)
        {
            Battle_manager.cells[Samurai_stats.samurai_cell_x - a, Samurai_stats.samurai_cell_y].GetComponent<Cell>().Ready_to_move(die_used_to_move);  //          <---- Implement this function!
            last_cells[1] = Battle_manager.cells[Samurai_stats.samurai_cell_x - a, Samurai_stats.samurai_cell_y];
            //if (Samurai_stats.samurai_cell_x - (a + 1) < Battle_manager.cells.GetLength(0)) left_obstacle = true;
        }
        else
        {
            if ((die_used_to_move.GetComponent<Dice_code>().element != "dark" && Battle_manager.cells[Samurai_stats.samurai_cell_x - a, Samurai_stats.samurai_cell_y].tag == "cell_obstacle") || (die_used_to_move.GetComponent<Dice_code>().element != "wind" && Battle_manager.cells[Samurai_stats.samurai_cell_x - a, Samurai_stats.samurai_cell_y].tag == "cell_occupied")) left_obstacle = true;
        }
    }

    if (Samurai_stats.samurai_cell_y + a < Battle_manager.cells.GetLength(1))
    {
        if (Battle_manager.cells[Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y + a].tag == "cell_empty" && top_obstacle == false)
        {
            Battle_manager.cells[Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y + a].GetComponent<Cell>().Ready_to_move(die_used_to_move);  //          <---- Implement this function!
            last_cells[2] = Battle_manager.cells[Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y + a];
            //if (Samurai_stats.samurai_cell_y + (a + 1) > Battle_manager.cells.GetLength(1)) top_obstacle = true;
        }
        else
        {
            if ((die_used_to_move.GetComponent<Dice_code>().element != "dark" && Battle_manager.cells[Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y + a].tag == "cell_obstacle") || (die_used_to_move.GetComponent<Dice_code>().element != "wind" && Battle_manager.cells[Samurai_stats.samurai_cell_x - a, Samurai_stats.samurai_cell_y + a].tag == "cell_occupied")) top_obstacle = true;
        }
    }

    if (Samurai_stats.samurai_cell_y - a >= 0)
    {
        if (Battle_manager.cells[Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y - a].tag == "cell_empty" && bottom_obstacle == false)
        {
            Battle_manager.cells[Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y - a].GetComponent<Cell>().Ready_to_move(die_used_to_move);  //          <---- Implement this function!
            last_cells[3] = Battle_manager.cells[Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y - a];
            //if (Samurai_stats.samurai_cell_y - (a + 1) > Battle_manager.cells.GetLength(1)) bottom_obstacle = true;
        }
        else
        {
            if ((die_used_to_move.GetComponent<Dice_code>().element != "dark" && Battle_manager.cells[Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y - a].tag == "cell_obstacle") || (die_used_to_move.GetComponent<Dice_code>().element != "wind" && Battle_manager.cells[Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y - a].tag == "cell_occupied")) bottom_obstacle = true;
        }
    }

}
*/