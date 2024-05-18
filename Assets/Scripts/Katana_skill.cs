using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana_skill : MonoBehaviour
{
    public GameObject slot;
    //public GameObject die;
    [SerializeField] string skill_name;
    public string element;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Use(GameObject used_die)
    {
        //die = used_die;
        Battle_manager.skill_die = used_die;
        Battle_manager.current_skill = this.gameObject;

    }

    public void FindTarget()
    {
        Battle_manager.StopCellTarget();

        switch (skill_name)
        {
            default:
                DefaultAttack();
                break;

        }
    }

    void DefaultAttack()  // Targeting 
    {
        for (int a = 0; a < Battle_manager.path_cells.Count; a++)
        {
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (Battle_manager.path_cells[a].GetComponent<Cell>().x + x < Battle_manager.cells.GetLength(0) && Battle_manager.path_cells[a].GetComponent<Cell>().x + x >= 0 && Battle_manager.path_cells[a].GetComponent<Cell>().y + y < Battle_manager.cells.GetLength(1) && Battle_manager.path_cells[a].GetComponent<Cell>().y + y >= 0)
                    {
                        GameObject cell_to_check = Battle_manager.cells[Battle_manager.path_cells[a].GetComponent<Cell>().x + x, Battle_manager.path_cells[a].GetComponent<Cell>().y + y];
                        if (cell_to_check.tag == "cell_occupied" && cell_to_check.GetComponent<Cell>().target_cell == false)
                        {
                            if (cell_to_check.GetComponent<Cell>().x == Samurai_stats.samurai_cell_x && cell_to_check.GetComponent<Cell>().y == Samurai_stats.samurai_cell_y)
                            {

                            }
                            else cell_to_check.GetComponent<Cell>().MakeTarget();
                        }
                    }

                }
            }
        }
    }

    public void Attack_options(GameObject samurai_dice, GameObject target, GameObject target_dice, string phase) // Actual attacking lol
    {
        switch (phase)
        {
            case "start":
                Battle_manager.characters[0].GetComponent<Samurai_animation>().AttackAnimation("katana", slot.GetComponent<Skill_slot>().skill_slot_nr);
                break;
            
            case "miss":
                switch (skill_name)
                {
                    case "thunder_attack":
                        target_dice.GetComponent<MiniDice_code>().Deactivate();     // Add thunder animation to this attack (and animation to deactivated dice in general)
                        break;
                }
                break;
            case "hit":
                break;
            case "kill":
                break;
            case "end":
                break;
        }
    }
}
