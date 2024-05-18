using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : MonoBehaviour
{
    public GameObject die_used_to_shoot;
    public GameObject glowing;

    // Start is called before the first frame update
    void Start()
    {
        Battle_manager.ranged = this.gameObject;
    }

    void FindTarget()
    {
        List<GameObject> cells_around = Battle_manager.GetCellsDiagonal(Samurai_stats.samurai_cell_x, Samurai_stats.samurai_cell_y, "all", false, false, 4);
        for (int a = 0; a < cells_around.Count; a++)
        {
            cells_around[a].GetComponent<Cell>().MakeTarget();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
        if (obj.gameObject.tag == "dice" && die_used_to_shoot == null)
        {
            glowing.SetActive(false);
        }
    }


    void OnTriggerStay2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "dice" && Battle_manager.activation)
        {
            Debug.Log("Added to ranged");
            glowing.SetActive(true);
            if (obj.gameObject == die_used_to_shoot) return;

            if (Battle_manager.move_die != null) Battle_manager.move_die.GetComponent<Dice_code>().ReturnBack();
            if (Battle_manager.skill_die != null) Battle_manager.skill_die.GetComponent<Dice_code>().ReturnBack();

            if (die_used_to_shoot != null)
            {
                die_used_to_shoot.GetComponent<Dice_code>().ReturnBack();
                die_used_to_shoot = null;

                Battle_manager.StopCellTarget();

            }

            Battle_manager.CheckDice(obj.gameObject);

            die_used_to_shoot = obj.gameObject;
            die_used_to_shoot.GetComponent<Dice_code>().ranged = true;
            Battle_manager.skill_die = obj.gameObject;
            die_used_to_shoot.transform.position = transform.position;                             // <<<< TO DO, make it so that the die spawns closer to the botom of the icon, like the rest of abilities do
            

            // ---------------------------------

            FindTarget();

            Battle_manager.activation = false;

            // ---------------------------------


        }
    }
}
