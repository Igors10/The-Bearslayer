using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Capsule_code : MonoBehaviour
{
    [SerializeField] GameObject sword_icon;
    [SerializeField] GameObject boot_icon;

    [SerializeField] GameObject move;
    [SerializeField] GameObject attack;
    [SerializeField] GameObject ranged_attack;
    [SerializeField] GameObject trinket;
    [SerializeField] GameObject attack_modifier;
    [SerializeField] GameObject resist_modifier;

    [SerializeField] GameObject move_skill_list;

    [SerializeField] Sprite default_sprite;
    [SerializeField] Sprite attack_sprite;
    [SerializeField] Sprite move_sprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void Stop_skill_activation(GameObject die)
    {
        if (die == move.GetComponent<Move>().die_used_to_move)
        {
            Recolor(0);
            Battle_manager.StopCellActivation();
            Battle_manager.StopCellTarget();
            Battle_manager.DeletePath();
            Battle_manager.move_die = null;
            move.GetComponent<Move>().glowing.SetActive(false);

            move.GetComponent<Move>().die_used_to_move = null;
            if (Battle_manager.skill_die != null)
            {
                Battle_manager.skill_die.GetComponent<Dice_code>().ReturnBack();
                Battle_manager.skill_die = null;
                attack_modifier.SetActive(false);
                resist_modifier.SetActive(false);
            }


        }
        if (die == Battle_manager.skill_die)
        {
            if (die.GetComponent<Dice_code>().ranged) ranged_attack.GetComponent<Ranged>().glowing.SetActive(false);
            else attack.GetComponent<Slash>().glowing.SetActive(false);
            Battle_manager.StopCellTarget();
            Battle_manager.skill_die = null;
            attack_modifier.SetActive(false);
            resist_modifier.SetActive(false);
        }
    }



    public void Recolor(int color) // 0 - default   1 - green   2 - blue
    {
        //trinket.SetActive(false);
        if (color == 1)
        {
            //GetComponent<Image>().sprite = move_sprite;
            //attack.SetActive(false);
            //boot_icon.transform.position = move.transform.position;
            //boot_icon.transform.Translate(0f, 1f, 0f);
            //move_skill_list.SetActive(true);
        }
        if (color == 2)
        {
            GetComponent<Image>().sprite = attack_sprite;
            move.SetActive(false);
            sword_icon.transform.position = attack.transform.position;
            sword_icon.transform.Translate(0f, -1f, 0f);
        }
        if (color == 0)
        {
            if (GetComponent<Image>().sprite == move_sprite) boot_icon.transform.position = move.transform.position;
            else if (GetComponent<Image>().sprite == attack_sprite) sword_icon.transform.position = attack.transform.position;

            GetComponent<Image>().sprite = default_sprite;

            trinket.SetActive(true);
            attack.SetActive(true);
            move.SetActive(true);
            move_skill_list.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
