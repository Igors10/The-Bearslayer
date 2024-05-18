using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_foe : MonoBehaviour
{
    int x;
    int y;

    [SerializeField] bool tutorial = false;
    [SerializeField] Sprite tutorial_sprites;
    // Start is called before the first frame update
    void Start()
    {
        x = GetComponent<Enemy_atributes>().x_coord;
        y = GetComponent<Enemy_atributes>().y_coord;

        //if (tutorial) GetComponent<Enemy_animation>().Sprite[] =  change for tutorial sprites !!!
    }

    
    GameObject CheckForSamuraiAround()
    {
        GameObject cell_with_samurai = null;
        List<GameObject> cells_around = Battle_manager.GetCellsAround(GetComponent<Enemy_atributes>().x_coord, GetComponent<Enemy_atributes>().y_coord);
        for (int a = 0; a < cells_around.Count; a++)
        {
            if (cells_around[a].GetComponent<Cell>().x == Samurai_stats.samurai_cell_x && cells_around[a].GetComponent<Cell>().y == Samurai_stats.samurai_cell_y) cell_with_samurai = cells_around[a];
        }

        return cell_with_samurai;
        
    }

    GameObject CheckForSamuraiStep()
    {
        GameObject cell_with_samurai = null;
        List<GameObject> cells = Battle_manager.GetCellsDiagonal(GetComponent<Enemy_atributes>().x_coord, GetComponent<Enemy_atributes>().y_coord, "all", false, false, 2);
        for (int a = 0; a < cells.Count; a++)
        {
            if (cells[a].GetComponent<Cell>().x == Samurai_stats.samurai_cell_x && cells[a].GetComponent<Cell>().y == Samurai_stats.samurai_cell_y) cell_with_samurai = cells[a];
        }

        cells = Battle_manager.GetCellsLine(x, y, "all", false, false, 2);
        for (int a = 0; a < cells.Count; a++)
        {
            if (cells[a].GetComponent<Cell>().x == Samurai_stats.samurai_cell_x && cells[a].GetComponent<Cell>().y == Samurai_stats.samurai_cell_y) cell_with_samurai = cells[a];
        }

        return cell_with_samurai;
    }

    void Attack(GameObject chosen_dice)
    {
        // Animation
        GetComponent<Enemy_animation>().NewAnimation("attack");
        // ___________

        Debug.Log("Log attacks the samurai");

        //GameObject chosen_dice = GetComponent<Enemy_atributes>().dice[Random.Range(0, GetComponent<Enemy_atributes>().dice.Count)];

        GetComponent<Enemy_atributes>().HighlightDice(chosen_dice, "attack");

        Battle_manager.characters[0].GetComponent<Attack_manager>().AttackOnSamurai(chosen_dice, this.gameObject);

        GetComponent<Enemy_atributes>().EndTurn(2);
    }

    void StepAttack(GameObject chosen_dice)
    {
        // Animation
        GetComponent<Enemy_animation>().NewAnimation("attack");
        // ___________

        Debug.Log("Log attacks the samurai with step");

        //GameObject chosen_dice = GetComponent<Enemy_atributes>().dice[Random.Range(0, GetComponent<Enemy_atributes>().dice.Count)];

        

        int move_to_x = (Samurai_stats.samurai_cell_x - GetComponent<Enemy_atributes>().x_coord) / 2;
        int move_to_y = (Samurai_stats.samurai_cell_y - GetComponent<Enemy_atributes>().y_coord) / 2;

        
        Battle_manager.cells[GetComponent<Enemy_atributes>().x_coord, GetComponent<Enemy_atributes>().y_coord].tag = "cell_empty";
        GetComponent<Enemy_atributes>().x_coord += move_to_x;
        GetComponent<Enemy_atributes>().y_coord += move_to_y;
        Battle_manager.cells[GetComponent<Enemy_atributes>().x_coord, GetComponent<Enemy_atributes>().y_coord].tag = "cell_occupied";
        GetComponent<Enemy_atributes>().Sprite.sortingOrder -= move_to_y;

        Battle_manager.Level.GetComponent<Animation_manager>().Animation_movement(this.gameObject, Battle_manager.cells[GetComponent<Enemy_atributes>().x_coord, GetComponent<Enemy_atributes>().y_coord].transform.position, 0.1f, "none");
        Battle_manager.characters[0].GetComponent<Attack_manager>().AttackOnSamurai(chosen_dice, this.gameObject);

        GetComponent<Enemy_atributes>().EndTurn(2);
    }

    public void Action()
    {
        if (GetComponent<Enemy_atributes>().dice.Count < 1) GetComponent<Enemy_atributes>().Roll();
        else
        {
            GameObject chosen_dice = GetComponent<Enemy_atributes>().dice[Random.Range(0, GetComponent<Enemy_atributes>().dice.Count)];
            chosen_die = chosen_dice;
            action = 80; // <-- time between the dice is highlighted and the action is performed

            if (CheckForSamuraiAround() != null && tutorial == false) AttackPrep(chosen_dice);

            else if (CheckForSamuraiStep() != null && tutorial == false) StepAttackPrep(chosen_dice);

            else MovePrep(chosen_dice);
        }
        
              //Learn couratines

    }

    

    int action = 0;
    string action_type;
    GameObject chosen_die;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (action>0)
        {
            action--;
        }
        if (action == 1)
        {
            switch (action_type)
            {
                case "attack":
                    Attack(chosen_die);
                    break;
                case "step_attack":
                    StepAttack(chosen_die);
                    break;
                case "move":
                    MoveRandomly(1, chosen_die);
                    break;
            }
        }
    }

    void PickDieForAttack()
    {
        GameObject picked_die = Battle_manager.characters[0].GetComponent<Attack_manager>().PickRandomSamuraiDie();
        Battle_manager.above_samurai_dice = picked_die;

        Battle_manager.characters[0].GetComponent<Attack_manager>().PutDieAboveSamurai(picked_die);
    }

    void AttackPrep(GameObject Die_used)
    {
        PickDieForAttack();
        GetComponent<Enemy_atributes>().HighlightDice(Die_used, "attack");
        action_type = "attack";
        GetComponent<Enemy_animation>().NewAnimation("prepareAttack");
        
    }

    void StepAttackPrep(GameObject Die_used)
    {
        GetComponent<Enemy_atributes>().HighlightDice(Die_used, "attack");
        action_type = "step_attack";
        GetComponent<Enemy_animation>().NewAnimation("prepareAttack");
        PickDieForAttack();
    }

    void MovePrep(GameObject Die_used)
    {
        GetComponent<Enemy_atributes>().HighlightDice(Die_used, "move");
        action_type = "move";
    }

    [SerializeField] bool experiment;
    int bug_save = 0;

    int MoveRandomlyPrep(int val, GameObject chosen_dice)
    {
        if (bug_save < 1)
        {
            //GameObject chosen_dice = GetComponent<Enemy_atributes>().dice[Random.Range(0, GetComponent<Enemy_atributes>().dice.Count)];
            if (chosen_dice == null) GetComponent<Enemy_atributes>().EndTurn(2);
            
            //chosen_dice.GetComponent<MiniDice_code>().Deactivate();
            val = chosen_dice.GetComponent<MiniDice_code>().value;
            
        }
        //else Debug.Log("Rerolling the value");
           
        int result = Random.Range(1, val + 1);
        //Debug.Log("Skeleton moves for " + result);

        return result;
    }

    public void MoveRandomly(int value, GameObject chosen_dice)
    {
        // Animation
        GetComponent<Enemy_animation>().NewAnimation("move");
        
        // ___________

        value = MoveRandomlyPrep(value, chosen_dice);

        //if (bug_save > 4) GetComponent<Enemy_atributes>().Roll();

        bug_save++;
        int a = 0;
        int b = 0;
        switch (Random.Range(1, 3))                      // <<< Make logs go towards samurai
        {
            case 1:
                a = value;
                if (Samurai_stats.samurai_cell_x == x)
                {
                    b = a;
                    a = 0;
                }
                else if (Samurai_stats.samurai_cell_x < x) a *= -1;
                break;
            case 2:
                b = value;
                if (Samurai_stats.samurai_cell_y == y)
                {
                    a = b;
                    b = 0;
                }
                else if (Samurai_stats.samurai_cell_y < y) b *= -1;
                break;
      

        }

        // Checks if the cell right near the enemy is empty or not

        bool close_cell_check = true;
        int modifier = ((a + b) > 0) ? 1 : -1;

        if (a == 0 && GetComponent<Enemy_atributes>().x_coord + a < Battle_manager.cells.GetLength(0) && GetComponent<Enemy_atributes>().y_coord + b < Battle_manager.cells.GetLength(1) && GetComponent<Enemy_atributes>().x_coord + a >= 0 && GetComponent<Enemy_atributes>().y_coord + b >= 0)
        {
            close_cell_check = (Battle_manager.cells[GetComponent<Enemy_atributes>().x_coord, GetComponent<Enemy_atributes>().y_coord + modifier].tag == "cell_empty") ? true : false;
        }
        else if (b == 0 && GetComponent<Enemy_atributes>().x_coord + a < Battle_manager.cells.GetLength(0) && GetComponent<Enemy_atributes>().y_coord + b < Battle_manager.cells.GetLength(1) && GetComponent<Enemy_atributes>().x_coord + a >= 0 && GetComponent<Enemy_atributes>().y_coord + b >= 0)
        {
            close_cell_check = (Battle_manager.cells[GetComponent<Enemy_atributes>().x_coord + modifier, GetComponent<Enemy_atributes>().y_coord].tag == "cell_empty") ? true : false;
        }
        

        // ---------------------------------------------------------

        if (GetComponent<Enemy_atributes>().x_coord + a < Battle_manager.cells.GetLength(0) && GetComponent<Enemy_atributes>().y_coord + b < Battle_manager.cells.GetLength(1)
            && GetComponent<Enemy_atributes>().x_coord + a >= 0 && GetComponent<Enemy_atributes>().y_coord + b >= 0
            && close_cell_check
            //&& Battle_manager.cells[GetComponent<Enemy_atributes>().x_coord + (a % 100), GetComponent<Enemy_atributes>().y_coord + (b % 100)].tag == "cell_empty"
            //&& Battle_manager.cells[GetComponent<Enemy_atributes>().x_coord + a, GetComponent<Enemy_atributes>().y_coord + b].tag == "cell_empty"
            )
        {

            bug_save = 0;
            if (experiment)
            {
                for (int e = 1; e <= System.Math.Abs(a + b); e++)
                {
                    if (a == 0) //  && e != System.Math.Abs(a + b)
                    {
                        e = (b > 0) ? e : -e;
                        if (Battle_manager.cells[GetComponent<Enemy_atributes>().x_coord, GetComponent<Enemy_atributes>().y_coord + e].tag != "cell_empty")
                        {
                            /*
                            if (e == 1)
                            {
                                bug_save++;
                                MoveRandomly(value);
                                //return;
                            }*/
                            b = (b > 0) ? e - 1 : e + 1;
                            break;
                        }

                        e = (e < 0) ? -e : e;


                    }
                    else if (bug_save > 3) GetComponent<Enemy_atributes>().Roll();
                    else if (b == 0)
                    {
                        e = (a > 0) ? e : -e;
                        if (Battle_manager.cells[GetComponent<Enemy_atributes>().x_coord + e, GetComponent<Enemy_atributes>().y_coord].tag != "cell_empty")
                        {
                            /*
                            if (e == 1)
                            {
                                bug_save++;
                                MoveRandomly(value);
                                //return;
                            }*/
                            a = (a > 0) ? e - 1 : e + 1;
                            break;
                        }

                        e = (e < 0) ? -e : e;
                    }
                }
            }
            

            bug_save = 0;
            Battle_manager.cells[GetComponent<Enemy_atributes>().x_coord + a, GetComponent<Enemy_atributes>().y_coord + b].tag = "cell_occupied";
            Battle_manager.cells[GetComponent<Enemy_atributes>().x_coord, GetComponent<Enemy_atributes>().y_coord].tag = "cell_empty";
            GetComponent<Enemy_atributes>().x_coord += a;
            GetComponent<Enemy_atributes>().y_coord += b;
            GetComponent<Enemy_atributes>().Sprite.sortingOrder -= b;

            Battle_manager.Level.GetComponent<Animation_manager>().Animation_movement(this.gameObject, Battle_manager.cells[GetComponent<Enemy_atributes>().x_coord, GetComponent<Enemy_atributes>().y_coord].transform.position, 0.1f, "none");

            //Debug.Log("New dummy's coords are: " + GetComponent<Enemy_atributes>().x_coord + " and " + GetComponent<Enemy_atributes>().y_coord);
            //GetComponent<Enemy_atributes>().EndTurn(2);
        }
        else
        {
            MoveRandomly(value, chosen_dice);
        }
        //}
    }
    /*
    void ActualMovement(int horizontal, int vertical)
    {
        Battle_manager.cells[GetComponent<Enemy_atributes>().x_coord + horizontal, GetComponent<Enemy_atributes>().y_coord + vertical].tag = "cell_occupied";
        Battle_manager.cells[GetComponent<Enemy_atributes>().x_coord, GetComponent<Enemy_atributes>().y_coord].tag = "cell_empty";
        GetComponent<Enemy_atributes>().x_coord += horizontal;
        GetComponent<Enemy_atributes>().y_coord += vertical;
        GetComponent<Enemy_atributes>().Sprite.sortingOrder -= vertical;

        

        //Debug.Log("New dummy's coords are: " + GetComponent<Enemy_atributes>().x_coord + " and " + GetComponent<Enemy_atributes>().y_coord);
    }*/
}

/*
 *        /*
 *         /*
            if (Battle_manager.cells[GetComponent<Enemy_atributes>().x_coord + a, GetComponent<Enemy_atributes>().y_coord + b].tag == "cell_empty")
            {
                
                
                bug_save = 0;
                Battle_manager.cells[GetComponent<Enemy_atributes>().x_coord + a, GetComponent<Enemy_atributes>().y_coord + b].tag = "cell_occupied";
                Battle_manager.cells[GetComponent<Enemy_atributes>().x_coord, GetComponent<Enemy_atributes>().y_coord].tag = "cell_empty";
                GetComponent<Enemy_atributes>().x_coord += a;
                GetComponent<Enemy_atributes>().y_coord += b;
                GetComponent<Enemy_atributes>().Sprite.sortingOrder -= b;

                Battle_manager.Level.GetComponent<Animation_manager>().Animation_movement(this.gameObject, Battle_manager.cells[GetComponent<Enemy_atributes>().x_coord, GetComponent<Enemy_atributes>().y_coord].transform.position, 0.1f, "none");

                Debug.Log("New dummy's coords are: " + GetComponent<Enemy_atributes>().x_coord + " and " + GetComponent<Enemy_atributes>().y_coord);
            }
            else if (bug_save == 3) GetComponent<Enemy_atributes>().EndTurn();
            else
            {
                MoveRandomly(value);
            }
*--------------------------------------
                    if (e == b || e == -b)
                    {
                        Debug.Log("Animation has been executed");
                        Battle_manager.Level.GetComponent<Animation_manager>().Animation_movement(this.gameObject, Battle_manager.cells[GetComponent<Enemy_atributes>().x_coord, GetComponent<Enemy_atributes>().y_coord].transform.position, 0.1f, "none");
                        break;
                    }
                    else if (b > 0 && Battle_manager.cells[GetComponent<Enemy_atributes>().x_coord, GetComponent<Enemy_atributes>().y_coord + 1].tag == "cell_empty") ActualMovement(0, 1);
                    else if (b < 0 && Battle_manager.cells[GetComponent<Enemy_atributes>().x_coord, GetComponent<Enemy_atributes>().y_coord - 1].tag == "cell_empty") ActualMovement(0, -1);
                    else
                    {
                        Debug.Log("Animation has been executed");
                        Battle_manager.Level.GetComponent<Animation_manager>().Animation_movement(this.gameObject, Battle_manager.cells[GetComponent<Enemy_atributes>().x_coord, GetComponent<Enemy_atributes>().y_coord].transform.position, 0.1f, "none");
                        break;
                    }
                    
                     *** for b ***
                     *if (e == a || e == -a)
                    {
                        Debug.Log("Animation has been executed");
                        Battle_manager.Level.GetComponent<Animation_manager>().Animation_movement(this.gameObject, Battle_manager.cells[GetComponent<Enemy_atributes>().x_coord, GetComponent<Enemy_atributes>().y_coord].transform.position, 0.1f, "none");
                        break;
                    }
                    if (a > 0 && Battle_manager.cells[GetComponent<Enemy_atributes>().x_coord + 1, GetComponent<Enemy_atributes>().y_coord].tag == "cell_empty") ActualMovement(1, 0);
                    else if (a < 0 && Battle_manager.cells[GetComponent<Enemy_atributes>().x_coord - 1, GetComponent<Enemy_atributes>().y_coord].tag == "cell_empty") ActualMovement(-1, 0);
                    else
                    {
                        Debug.Log("Animation has been executed");
                        Battle_manager.Level.GetComponent<Animation_manager>().Animation_movement(this.gameObject, Battle_manager.cells[GetComponent<Enemy_atributes>().x_coord, GetComponent<Enemy_atributes>().y_coord].transform.position, 0.1f, "none");
                        break;
                    }
                     *
                     *
                     
*-------------------------------------
           if (a == 0)
           {
               for (int c = 1; System.Math.Abs(c) < System.Math.Abs(b); c++)
               {
                   if (c > 0 && b < 0) c *= -1;
                   if (Battle_manager.cells[GetComponent<Enemy_atributes>().x_coord, GetComponent<Enemy_atributes>().y_coord + c].tag != "cell_empty")
                   {
                       if (c == 0)
                       {

                           bug_save++;
                           MoveRandomly(value);
                           //return;
                       }
                       else
                       {
                           b = c;
                           break;

                       }

                   }
                   if (c < 0 && b < 0) c *= -1;
               }
           }
           else
           {
               for (int c = 1; System.Math.Abs(c) < System.Math.Abs(a); c++)
               {
                   if (c > 0 && a < 0) c *= -1;
                   if (Battle_manager.cells[GetComponent<Enemy_atributes>().x_coord + c, GetComponent<Enemy_atributes>().y_coord].tag != "cell_empty")
                   {
                       if (c == 0)
                       {
                           bug_save++;
                           MoveRandomly(value);
                           //return;
                       }
                       else
                       {
                           a = c;
                           break;

                       }

                   }
                   if (c < 0 && b < 0) c *= -1;
               }
           }
           bug_save = 0;
           Battle_manager.cells[GetComponent<Enemy_atributes>().x_coord + a, GetComponent<Enemy_atributes>().y_coord + b].tag = "cell_occupied";
           Battle_manager.cells[GetComponent<Enemy_atributes>().x_coord, GetComponent<Enemy_atributes>().y_coord].tag = "cell_empty";
           GetComponent<Enemy_atributes>().x_coord += a;
           GetComponent<Enemy_atributes>().y_coord += b;
           GetComponent<Enemy_atributes>().Sprite.sortingOrder -= b;

           Battle_manager.Level.GetComponent<Animation_manager>().Animation_movement(this.gameObject, Battle_manager.cells[GetComponent<Enemy_atributes>().x_coord, GetComponent<Enemy_atributes>().y_coord].transform.position, 0.1f, "none");

           Debug.Log("New dummy's coords are: " + GetComponent<Enemy_atributes>().x_coord + " and " + GetComponent<Enemy_atributes>().y_coord);*/