using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Trinket : MonoBehaviour
{
    // Start is called before the first frame update


    // Update is called once per frame

    GameObject die_to_interact;

    public int max_trinket_charges;
    public int current_trinket_charges;
    public string trinket_name;
    public TMP_Text text;
    public GameObject glowing;

    void Start()
    {
        Recharge(0);
    }

    void Update()
    {
        
    }

    public void Recharge(int count)
    {
        if (count == 0)
        {
            current_trinket_charges = max_trinket_charges;
            text.text = current_trinket_charges.ToString();
        }
        
    }

    public void UseTrinket()
    {
        if (current_trinket_charges < 1)
        {
            //Battle_manager.current_die.GetComponent<Dice_code>().ReturnBack();
            die_to_interact.GetComponent<Dice_code>().ReturnBack();
            Debug.Log("Not enough charges");
            return;
        }
        current_trinket_charges--;
        text.text = current_trinket_charges.ToString();

        switch (trinket_name)
        {
            case "Hollow_bone":
                //Battle_manager.current_die.GetComponent<Dice_code>().rotating = 126;
                die_to_interact.GetComponent<Dice_code>().rotating = 126;
                //Debug.Log("Rerolled to " + Battle_manager.current_die.GetComponent<Dice_code>().value);
                break;
            case "Dragon_scale":
                die_to_interact.GetComponent<Dice_code>().value++;
                break;
            default:
                Debug.Log("Incorrect_trinket");
                break;
        }
        die_to_interact.GetComponent<Dice_code>().ReturnBack();
        die_to_interact = null;

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
        if (obj.gameObject.tag == "dice")
        {
            glowing.SetActive(false);
        }
    }

    void OnTriggerStay2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "dice" && Battle_manager.activation) //
        {
            if (obj.gameObject.GetComponent<Dice_code>().set) return;

            Debug.Log("Added to trinket");

            // ---------------------------------
            die_to_interact = obj.gameObject;

            UseTrinket();
            
            Battle_manager.activation = false;

            // ---------------------------------
        }
    }
}
