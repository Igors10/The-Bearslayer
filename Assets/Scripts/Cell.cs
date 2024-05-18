using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    // add coords
    public int x;
    public int y;

    bool ready_to_move = false;
    bool samurai = true;
    public GameObject Indicator;
    public GameObject die_used;
    public bool target_cell;
    public bool color_lock;

    GameObject dice_manager;

    public bool marked = false;

    public GameObject transparent_sprite;

    public void Ready_to_move(GameObject dice)
    {
        if (this.gameObject.tag == "cell_occupied" || this.gameObject.tag == "cell_obstacle") return;
        Battle_manager.activated_cells.Add(this.gameObject);
        ready_to_move = true;
        Indicator.SetActive(true);
        die_used = dice;
    }

    void Start()
    {
        dice_manager = GameObject.Find("Dice");
        //Indicator.GetComponent<SpriteRenderer>().color = new Color(0.5f, 1f, 0.5f, 1f);
        Indicator.GetComponent<SpriteRenderer>().color = new Color(0.5f, 1f, 0.5f, 0.5f);
        transparent_sprite = GameObject.Find("transparent_sprite");
    }

    void Update()
    {
    }

    public void MakeTarget()
    {
        Battle_manager.target_cells.Add(this.gameObject);
        Indicator.SetActive(true);
        target_cell = true;
        Indicator.GetComponent<SpriteRenderer>().color = new Color(1f, 0.25f, 0.25f, 0.5f);
    }

    public void StopActivation()
    {
        Indicator.GetComponent<SpriteRenderer>().color = new Color(0.5f, 1f, 0.5f, 0.5f);
        Battle_manager.activated_cells.Remove(this.gameObject);
        ready_to_move = false;
        Indicator.SetActive(false);
        die_used = null;
        transparent_sprite.SetActive(false);
    }

    public void StopTarget()
    {
        Indicator.GetComponent<SpriteRenderer>().color = new Color(0.5f, 1f, 0.5f, 0.5f);
        target_cell = false;
        Battle_manager.target_cells.Remove(this.gameObject);
        Indicator.SetActive(false);
    }

    void LateUpdate()
    {
        if (Battle_manager.stop_cell_activation)
        {
            Battle_manager.stop_cell_activation = false;
            Debug.Log("Activation_stopped");
        }
            
    }
    
    void OnMouseEnter()
    {
        if (ready_to_move == false) return;
        //Indicator.GetComponent<SpriteRenderer>().color = new Color(0.75f, 1f, 0.5f, 1f);
        Indicator.GetComponent<SpriteRenderer>().color = new Color(0.75f, 1f, 0.5f, 1f);
        //Indicator.transform.localScale += new Vector3(0.05f, 0.05f, 0f);

        if (Battle_manager.move_cell_lock == null)
        {
            transparent_sprite.SetActive(true);
            transparent_sprite.transform.position = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
        }

        
    }

    void OnMouseExit()
    {
        if (ready_to_move == false) return;
        //Indicator.GetComponent<SpriteRenderer>().color = new Color(0.5f, 1f, 0.5f, 1f);

        //Indicator.transform.localScale += new Vector3(-0.05f, -0.05f, 0f);
        if (Battle_manager.move_cell_lock != this.gameObject && color_lock == false) Indicator.GetComponent<SpriteRenderer>().color = new Color(0.5f, 1f, 0.5f, 0.5f);  //
        if (Battle_manager.move_cell_lock == null)
        {
            transparent_sprite.SetActive(false); 
            
        }
    }
    
    void OnMouseUp()
    {
        if (ready_to_move)
        {

            Move();

            /*
            if (Battle_manager.skill_die == null)
            {
                Move();
            }
            else
            {
                if (Battle_manager.move_cell_lock != null)
                {
                    Battle_manager.move_cell_lock.GetComponent<Cell>().Indicator.GetComponent<SpriteRenderer>().color = new Color(0.5f, 1f, 0.5f, 0.5f);
                }
                Battle_manager.move_cell_lock = this.gameObject;

                // Making a path
                Battle_manager.DeletePath();
                NewPath(Battle_manager.cells[Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y]);

                // Finding a target

                Battle_manager.current_skill.GetComponent<Katana_skill>().FindTarget();

                // Transparent sprite look
                transparent_sprite.transform.position = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
                //transparent_sprite.GetComponent<LineRenderer>().SetPosition(0, Battle_manager.cells[Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y].transform.position);
                //transparent_sprite.GetComponent<LineRenderer>().SetPosition(1, transform.position);
                
                // ----------- ------ ----
            }*/


        }
       

    }

    void FireBurst(GameObject target_cell)
    {
        // Make it work even if it's outside of the map

        // Notes for future- make defense less op (yes, remove the +2 bonus)

        Battle_manager.characters[0].GetComponent<Attack_manager>().Attack(die_used, Battle_manager.AccessCharacter(target_cell), "none", -2);

        dice_manager.GetComponent<Dice_manager>().dice_to_deactivate.Remove(die_used);
    }

    void FireBurstAnimation(int direction)
    {
        // animation
        GameObject FireBurst = Instantiate(die_used.GetComponent<Dice_code>().move_particles, Battle_manager.characters[0].transform.position, Quaternion.identity);
        FireBurst.GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingOrder = Battle_manager.current_player.GetComponent<Samurai>().Sprite.sortingOrder;
        FireBurst.transform.Rotate(0f, 90f * direction, 0f);

    }

    public void Move()
    {
        
        dice_manager.GetComponent<Dice_manager>().dice_to_deactivate.Add(die_used);

        if (die_used.GetComponent<Dice_code>().element == "fire")
        {
            // Fire burst 
            
            if (transform.position.y == Battle_manager.cells[Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y].transform.position.y && transform.position.x > Battle_manager.cells[Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y].transform.position.x
                && Battle_manager.cells[Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y].GetComponent<Cell>().x - 1 > 0)
            {
                FireBurstAnimation(1);
                if (Battle_manager.cells[Samurai_stats.samurai_cell_x - 1, Samurai_stats.samurai_cell_y].tag == "cell_occupied") FireBurst(Battle_manager.cells[Samurai_stats.samurai_cell_x - 1, Samurai_stats.samurai_cell_y]);
            }
            else if (transform.position.y == Battle_manager.cells[Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y].transform.position.y && transform.position.x < Battle_manager.cells[Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y].transform.position.x
                && Battle_manager.cells[Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y].GetComponent<Cell>().x + 1 < Battle_manager.cells.GetLength(0))
            {
                FireBurstAnimation(3);
                if (Battle_manager.cells[Samurai_stats.samurai_cell_x + 1, Samurai_stats.samurai_cell_y].tag == "cell_occupied") FireBurst(Battle_manager.cells[Samurai_stats.samurai_cell_x + 1, Samurai_stats.samurai_cell_y]);
            }
            else if (x == Samurai_stats.samurai_cell_x && transform.position.y > Battle_manager.cells[Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y].transform.position.y
                && Battle_manager.cells[Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y].GetComponent<Cell>().y - 1 > 0)
            {
                FireBurstAnimation(4);
                if (Battle_manager.cells[Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y - 1].tag == "cell_occupied") FireBurst(Battle_manager.cells[Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y - 1]);
            }
            else if (x == Samurai_stats.samurai_cell_x && transform.position.y < Battle_manager.cells[Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y].transform.position.y
                && Battle_manager.cells[Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y].GetComponent<Cell>().y + 1 < Battle_manager.cells.GetLength(1))
            {
                FireBurstAnimation(2);
                if (Battle_manager.cells[Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y + 1].tag == "cell_occupied") FireBurst(Battle_manager.cells[Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y + 1]);
            }

        }

        Battle_manager.move_cell_lock = null;

        

        Indicator.GetComponent<SpriteRenderer>().color = new Color(0.5f, 1f, 0.5f, 0.5f);
        //Indicator.transform.localScale += new Vector3(-0.05f, -0.05f, 0f);

        // Animation
        transparent_sprite.SetActive(false);

        //Battle_manager.current_player.transform.position = transform.position;               // < --- Moving the player

        if (die_used.GetComponent<Dice_code>().move_particles != null && die_used.GetComponent<Dice_code>().element != "fire")
        {
            GameObject ParticleBurst = Instantiate(die_used.GetComponent<Dice_code>().move_particles, Battle_manager.characters[0].transform.position, Quaternion.identity);
            ParticleBurst.GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingOrder = Battle_manager.current_player.GetComponent<Samurai>().Sprite.sortingOrder;
        }

        switch (die_used.GetComponent<Dice_code>().element)
        {
            case "wind":
                

                Battle_manager.characters[0].GetComponent<Samurai>().Samurai_sprite.GetComponent<TrailRenderer>().enabled = true;
                Battle_manager.characters[0].GetComponent<Samurai>().Samurai_sprite.GetComponent<TrailRenderer>().sortingOrder = Battle_manager.current_player.GetComponent<Samurai>().Sprite.sortingOrder - 1;
                Battle_manager.Level.GetComponent<Animation_manager>().Animation_movement(Battle_manager.current_player, transform.position, 0.4f, "none");  

                break;

            case "dark":
                Battle_manager.characters[0].GetComponent<Samurai>().shade.SetActive(true);
                Battle_manager.characters[0].GetComponent<Samurai>().Samurai_sprite.GetComponent<SpriteRenderer>().enabled = false;
                Battle_manager.Level.GetComponent<Animation_manager>().Animation_movement(Battle_manager.current_player, transform.position, 0.1f, "none");

                break;

            case "magic":
                Battle_manager.Level.GetComponent<Animation_manager>().Animation_movement(Battle_manager.current_player, transform.position, 2f, "none");
                break;

            case "water":

                //Battle_manager.Level.GetComponent<Animation_manager>().Animation_movement(Battle_manager.current_player, transform.position, 0.1f, "water_movement");
                Battle_manager.Level.GetComponent<Animation_manager>().Animation_movement(Battle_manager.current_player, transform.position, 0.1f, "none");

                break;

            default:
                Battle_manager.Level.GetComponent<Animation_manager>().Animation_movement(Battle_manager.current_player, transform.position, 0.1f, "none");
                break;

        
        }


        
        //Battle_manager.stop_cell_activation = true;
        Battle_manager.StopCellActivation();

        Battle_manager.current_player.GetComponent<Samurai_animation>().SamuraiAnimation("move");

        // -------------------------------------------

        // cell tags and samurai coords change and stuff

        Battle_manager.cells[Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y].tag = "cell_empty";

        if (transform.position.y == Battle_manager.cells[Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y].transform.position.y)
        {
            float difference = transform.position.x - Battle_manager.cells[Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y].transform.position.x;
            difference = difference / 1.5f;

            // Flipping the sprite

            if (transform.position.x - 3f < Samurai_stats.samurai_cell_x && Battle_manager.current_player.GetComponent<Samurai>().Sprite.flipX == false)
            {
                Battle_manager.current_player.GetComponent<Samurai>().Sprite.flipX = true;
            }

            if (transform.position.x - 3f > Samurai_stats.samurai_cell_x && Battle_manager.current_player.GetComponent<Samurai>().Sprite.flipX)
            {
                Battle_manager.current_player.GetComponent<Samurai>().Sprite.flipX = false;
            }

            // ---------------------

            //Samurai_stats.samurai_cell_x += (int)difference;


        }
        else
        {
            float difference = transform.position.y - Battle_manager.cells[Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y].transform.position.y;
            difference = difference / 0.5f;
            //Samurai_stats.samurai_cell_y += (int)difference;
            //Samurai_stats.samurai_cell_x += (int)difference * 0.75f;
            if (samurai) Battle_manager.current_player.GetComponent<Samurai>().Sprite.sortingOrder -= (int)difference;
        }

        // Setting new Samurai coordinates

        Samurai_stats.samurai_cell_y = y;
        Samurai_stats.samurai_cell_x = x;

        //---------------------------------

        Debug.Log("New Samurai coords are x: " + Samurai_stats.samurai_cell_x + " and y: " + Samurai_stats.samurai_cell_y);
        this.gameObject.tag = "cell_occupied";
        //Battle_manager.current_player.GetComponent<Samurai>().EndTurn();

        //Battle_manager.characters[0].GetComponent<Samurai>().EndTurn(1);
        // ------------------------------------------------

        // Dice deactivation
    }

    void NewPath(GameObject origin_cell)
    {
        int differnce;
        if (origin_cell.GetComponent<Cell>().x == x)
        {
            differnce = (origin_cell.GetComponent<Cell>().y - y > 0) ? +1 : -1;

            transparent_sprite.GetComponent<SpriteRenderer>().sortingOrder += Battle_manager.current_player.GetComponent<Samurai>().Sprite.sortingOrder + origin_cell.GetComponent<Cell>().y - y;

            for (int a = y; a != origin_cell.GetComponent<Cell>().y; a += differnce)
            {
                Battle_manager.path_cells.Add(Battle_manager.cells[x, a]);
                Battle_manager.cells[x, a].GetComponent<Cell>().Indicator.GetComponent<SpriteRenderer>().color = new Color(0.75f, 1f, 0.5f, 1f);
                Battle_manager.cells[x, a].GetComponent<Cell>().color_lock = true;
            }
        }
        else if (origin_cell.GetComponent<Cell>().y == y)
        {
            differnce = (origin_cell.GetComponent<Cell>().x - x > 0) ? +1 : -1;

            transparent_sprite.GetComponent<SpriteRenderer>().sortingOrder = Battle_manager.current_player.GetComponent<Samurai>().Sprite.sortingOrder;

            for (int a = x; a != origin_cell.GetComponent<Cell>().x; a += differnce)
            {
                Battle_manager.path_cells.Add(Battle_manager.cells[a, y]);
                Battle_manager.cells[a, y].GetComponent<Cell>().Indicator.GetComponent<SpriteRenderer>().color = new Color(0.75f, 1f, 0.5f, 1f);
                Battle_manager.cells[a, y].GetComponent<Cell>().color_lock = true;
            }
        }
        Battle_manager.path_cells.Add(this.gameObject);
        Battle_manager.path_cells.Add(origin_cell);
    }
}
