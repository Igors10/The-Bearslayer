using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_movelist : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //static int bug_save = 0;

    public static void Act(string name, GameObject target)
    {
        if (name == "Skeleton")
        {
            target.GetComponent<Skeleton_foe>().Action();
        }
    }

    /*
    void StartMovement()
    {

    }

    public static void MoveRandomly(GameObject enemy, int value)
    {
        if (bug_save > 4)
        {
            EndTurn();

        }

        bug_save++;
        int a = 0;
        int b = 0;
        int d;
        switch (Random.Range(1, 5))
        {
            case 1:
                a = 1;
                break;
            case 2:
                a = -1;
                break;
            case 3:
                b = 1;
                break;
            case 4:
                b = -1;
                break;

        }

        if (enemy.GetComponent<Enemy_atributes>().x_coord + a < Battle_manager.cells.GetLength(0) && enemy.GetComponent<Enemy_atributes>().y_coord + b < Battle_manager.cells.GetLength(1) && enemy.GetComponent<Enemy_atributes>().x_coord + a > 0 && enemy.GetComponent<Enemy_atributes>().y_coord + b > 0)
        {
            //d = (a == 0) ? b : a;
            if (a == 0)
            {
                for (int c = 0; Math.Abs(c) < Math.Abs(b); c++)
                {
                    if (c > 0 && b < 0) c *= -1;
                    if (Battle_manager.cells[enemy.GetComponent<Enemy_atributes>().x_coord, enemy.GetComponent<Enemy_atributes>().y_coord + c].tag != "cell_empty")
                    {
                        if (c == 0)
                        {
                            
                            bug_save++;
                            MoveRandomly(enemy, value);
                            return;
                        }
                        else
                        {
                            b = c;
                            break;
                            
                        }
                        
                    }
                }
            }
            else
            {
                for (int c = 0; Math.Abs(c) < Math.Abs(a); c++)
                {
                    if (c > 0 && a < 0) c *= -1;
                    if (Battle_manager.cells[enemy.GetComponent<Enemy_atributes>().x_coord + c, enemy.GetComponent<Enemy_atributes>().y_coord].tag != "cell_empty")
                    {
                        if (c == 0)
                        {
                            bug_save++;
                            MoveRandomly(enemy, value);
                            return;
                        }
                        else
                        {
                            a = c;
                            break;

                        }

                    }
                }
            }
           
            //if (Battle_manager.cells[enemy.GetComponent<Enemy_atributes>().x_coord + a, enemy.GetComponent<Enemy_atributes>().y_coord + b].tag == "cell_empty")
            //{

                // Checking for obstacles 
                


                // ----------------------

                bug_save = 0;
                Battle_manager.cells[enemy.GetComponent<Enemy_atributes>().x_coord + a, enemy.GetComponent<Enemy_atributes>().y_coord + b].tag = "cell_occupied";
                Battle_manager.cells[enemy.GetComponent<Enemy_atributes>().x_coord, enemy.GetComponent<Enemy_atributes>().y_coord].tag = "cell_empty";
                enemy.GetComponent<Enemy_atributes>().x_coord += a;
                enemy.GetComponent<Enemy_atributes>().y_coord += b;
                enemy.GetComponent<Enemy_atributes>().Sprite.sortingOrder -= b;

                Battle_manager.Level.GetComponent<Animation_manager>().Animation_movement(enemy, Battle_manager.cells[enemy.GetComponent<Enemy_atributes>().x_coord, enemy.GetComponent<Enemy_atributes>().y_coord].transform.position, 0.1f, "none");

                Debug.Log("New dummy's coords are: " + enemy.GetComponent<Enemy_atributes>().x_coord + " and " + enemy.GetComponent<Enemy_atributes>().y_coord);
            //}
            else if (bug_save == 3) enemy.GetComponent<Enemy_atributes>().EndTurn();
            else
            {
                MoveRandomly(enemy, value);
            }
        }
        else
        {
            MoveRandomly(enemy, value);
        }
    }*/
}
