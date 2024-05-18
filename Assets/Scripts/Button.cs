using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] string function = "none";
    [SerializeField] GameObject glowing;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        switch (function)
        {
            case "End_turn":
                Battle_manager.current_player.GetComponent<Samurai>().EndTurn(2);               
                glowing.SetActive(false);
                
                break;
            case "Damage_self":
                GameObject dice_manager = GameObject.Find("Dice");

                dice_manager.GetComponent<Dice_manager>().dice[0].GetComponent<Dice_code>().Death();

                break;
        }

    }

    void OnMouseEnter()
    {
        if (glowing != null)
        {
            glowing.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        if (glowing != null)
        {
            glowing.SetActive(false);
        }
    }
}
