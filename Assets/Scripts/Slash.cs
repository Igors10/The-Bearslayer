using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{

    public GameObject die_used_to_slash;
    public GameObject glowing;

    // Start is called before the first frame update
    void Start()
    {
        Battle_manager.slash = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FindTarget()
    {
        List<GameObject> cells_around = Battle_manager.GetCellsAround(Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y);
        for (int a = 0; a < cells_around.Count; a++)
        {
            cells_around[a].GetComponent<Cell>().MakeTarget();
        }
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
        if (obj.gameObject.tag == "dice" && die_used_to_slash == null)
        {
            glowing.SetActive(false);
        }
    }
    

    void OnTriggerStay2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "dice" && Battle_manager.activation)
        {
            glowing.SetActive(true);
            if (obj.gameObject == die_used_to_slash) return;

            if (Battle_manager.move_die != null) Battle_manager.move_die.GetComponent<Dice_code>().ReturnBack();
            if (Battle_manager.skill_die != null) Battle_manager.skill_die.GetComponent<Dice_code>().ReturnBack();

            if (die_used_to_slash != null) 
            {
                die_used_to_slash.GetComponent<Dice_code>().ReturnBack();
                die_used_to_slash = null;

                Battle_manager.StopCellTarget();
               
            }

            Battle_manager.CheckDice(obj.gameObject);

            die_used_to_slash = obj.gameObject;
            Battle_manager.skill_die = obj.gameObject;
            die_used_to_slash.transform.position = transform.position;
            Debug.Log("Added to slash");

            // ---------------------------------

            FindTarget();

            Battle_manager.activation = false;

            // ---------------------------------


        }
    }
}
