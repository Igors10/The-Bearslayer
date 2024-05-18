using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guard : MonoBehaviour
{
    GameObject die_used;
    public int guard_dice_count = 0;
    [SerializeField] GameObject manager;



    [SerializeField] GameObject[] guard_dice_positions;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void AddToGuard()
    {
        die_used.GetComponent<Dice_code>().set = true;
        //die_used.GetComponent<Image>().sprite = die_used.GetComponent<Dice_code>().die_face[die_used.GetComponent<Dice_code>().maxvalue + 1];
        manager.GetComponent<Dice_manager>().guard_dice.Add(die_used);
        Battle_manager.current_die = null;
        //guard_dice_count++;

        ArrangeGuardDice();
    }

    public void ArrangeGuardDice()
    {
        switch (manager.GetComponent<Dice_manager>().guard_dice.Count)
        {
            case 1:
                manager.GetComponent<Dice_manager>().guard_dice[0].transform.position = transform.position;
                break;
            case 2:
                manager.GetComponent<Dice_manager>().guard_dice[0].transform.position = guard_dice_positions[1].transform.position;
                manager.GetComponent<Dice_manager>().guard_dice[1].transform.position = guard_dice_positions[2].transform.position;
                break;
            case 3:
                manager.GetComponent<Dice_manager>().guard_dice[0].transform.position = guard_dice_positions[0].transform.position;
                manager.GetComponent<Dice_manager>().guard_dice[1].transform.position = transform.position;
                manager.GetComponent<Dice_manager>().guard_dice[2].transform.position = guard_dice_positions[3].transform.position;
                break;

        }
    }

    void OnTriggerStay2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "dice" && Battle_manager.activation)
        {
            if (obj.gameObject.GetComponent<Dice_code>().set || obj.gameObject.GetComponent<Dice_code>().ghost) return;

            if (obj.gameObject == Battle_manager.move_die || obj.gameObject == Battle_manager.skill_die) obj.gameObject.GetComponent<Dice_code>().ReturnBack();

            Debug.Log("Added to guard");
            die_used = obj.gameObject;
            // ---------------------------------

            if (manager.GetComponent<Dice_manager>().guard_dice.Count < 3) AddToGuard();
            else
            {
                obj.gameObject.GetComponent<Dice_code>().ReturnBack();
            }
            Battle_manager.activation = false;

            // ---------------------------------
        }
    }
    
}
