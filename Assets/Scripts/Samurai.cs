using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samurai : MonoBehaviour
{
    public GameObject Samurai_sprite;
    public SpriteRenderer Sprite;
    public GameObject UI;
    public GameObject shade;
    public GameObject attack_name_box;
    public bool dead;

    int turn_end = 0; // It stores how many action points should go to battle_manager at the end of the turn
    [SerializeField] int turn_end_timer_cap;
    int turn_end_timer = 0;

    [SerializeField] GameObject indicator;

    


    // Start is called before the first frame update
    void Start()
    {
        Sprite.sortingOrder = 20 - (int)Battle_list.samurai_spawn.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (turn_end != 0)
        {
            turn_end_timer++;
            if (turn_end_timer == turn_end_timer_cap)
            {
                Battle_manager.EndTurn(turn_end);
                turn_end = 0;
                turn_end_timer = 0;
            }
            
        }
    }

    public void Death()
    {
        dead = true;

        //Add more stuff here 
    }

    public void StartTurn()
    {
        Camera.main.GetComponent<Camera_movement>().camera_on_samurai = true;

        Debug.Log("Samurai starts turn");
        indicator.SetActive(true);
        UI.SetActive(true);
    }

    public void EndTurn(int action_point_cost)
    {
        // Stopping cell activation
        //Battle_manager.stop_cell_activation = true;
        Samurai_sprite.GetComponent<SpriteRenderer>().enabled = true;
        shade.SetActive(false);
        Battle_manager.StopCellActivation();
        // Recoloring the capsule
        GameObject capsule = GameObject.Find("capsule");
        capsule.GetComponent<Capsule_code>().Recolor(0);
        // Deactivating dice
        GameObject dice_manager = GameObject.Find("Dice");
        dice_manager.GetComponent<Dice_manager>().EndOfTurn();
        // ----------------------

        if ((Battle_manager.action_points - action_point_cost) < 1)
        {
            Samurai_sprite.GetComponent<TrailRenderer>().enabled = false;
            indicator.SetActive(false);
            UI.SetActive(false);

            Debug.Log("Samurai ends turn");
        }

        turn_end = action_point_cost;
        

        
    }
}
