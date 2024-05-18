using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll_button : MonoBehaviour
{
    [SerializeField] GameObject Dicemanager;
    [SerializeField] GameObject glowing;
    void OnMouseDown()
    {
        Dicemanager.GetComponent<Dice_manager>().RollDice();
        Battle_manager.current_player.GetComponent<Samurai>().EndTurn(2);
        glowing.SetActive(false);
    }

    void OnMouseEnter()
    {
       
        glowing.SetActive(true);
    }

    void OnMouseExit()
    {
        
        glowing.SetActive(false);
    }
}
