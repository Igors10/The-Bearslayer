using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice_manager : MonoBehaviour
{
    //[SerializeField] GameObject[] dice;
    public List<GameObject> dice = new List<GameObject>();
    public List<GameObject> guard_dice = new List<GameObject>();
    public List<GameObject> dice_to_deactivate = new List<GameObject>();
    public List<GameObject> ghost_dice_storage = new List<GameObject>();
    public List<GameObject> ghost_dice = new List<GameObject>();


    //public GameObject[] guard_dice;
    public GameObject[] default_positions;
    public GameObject text_value_reference;
    public GameObject text_values_block;
    public GameObject block_effect;

    //Things to assign to dice

    public float dice_rotation_speed;

    public GameObject guard_drop;
    [SerializeField] GameObject move_drop;
    [SerializeField] GameObject ability_drop;
    [SerializeField] GameObject trinket_drop;
    [SerializeField] GameObject hint;
    public GameObject katana_dice_position;
    public GameObject sword_dice_position_1;
    public GameObject sword_dice_position_2;

    public GameObject hit_particles;
    public GameObject damage_number;

    public void EndOfTurn()
    {
        //int b = dice_to_deactivate.Count;

        while (0 < dice_to_deactivate.Count)
        {
            dice_to_deactivate[0].GetComponent<Dice_code>().Deactivate();
            dice_to_deactivate.Remove(dice_to_deactivate[0]);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Battle_manager.dice_manager = this.gameObject;

        for (int a = 0; a < 6; a++)
        {
            GameObject new_dice = Instantiate(Samurai_stats.samurai_starting_dice[a], default_positions[a].transform.position, Quaternion.identity, this.gameObject.transform);
            dice.Add(new_dice);
        }
        for (int a = 0; a < 3; a++)
        {
            ghost_dice_storage.Add(Samurai_stats.samurai_starting_ghost_dice[a]);
        }
        //guard_dice = new GameObject[3];
        

        for (int a = 0; a < dice.Count; a++)
        {
            dice[a].GetComponent<Dice_code>().guard_drop = guard_drop;
            dice[a].GetComponent<Dice_code>().move_drop = move_drop;
            dice[a].GetComponent<Dice_code>().ability_drop = ability_drop;
            dice[a].GetComponent<Dice_code>().trinket_drop = trinket_drop;
            dice[a].GetComponent<Dice_code>().hint = hint;
            dice[a].GetComponent<Dice_code>().manager = this.gameObject;

            //default_positions
            default_positions[a].transform.position = dice[a].transform.position;
            dice[a].GetComponent<Dice_code>().default_position = default_positions[a];
        }

        RollDice();
    }

    public void ReplaceDice(GameObject Dice, GameObject Dice_to_replace, bool is_ghost)
    {
        GameObject new_Dice = Instantiate(Dice, Dice_to_replace.transform.position, Quaternion.identity, this.gameObject.transform);

        new_Dice.GetComponent<Dice_code>().guard_drop = guard_drop;
        new_Dice.GetComponent<Dice_code>().move_drop = move_drop;
        new_Dice.GetComponent<Dice_code>().ability_drop = ability_drop;
        new_Dice.GetComponent<Dice_code>().trinket_drop = trinket_drop;
        new_Dice.GetComponent<Dice_code>().hint = hint;
        new_Dice.GetComponent<Dice_code>().manager = this.gameObject;

        new_Dice.GetComponent<Dice_code>().default_position = Dice_to_replace.GetComponent<Dice_code>().default_position;

        if (is_ghost) new_Dice.GetComponent<Dice_code>().MakeMeGhost();
        else dice.Add(new_Dice);
    }

    bool CheckForGhost(GameObject die_to_check)
    {
        for (int a = 0; a < ghost_dice_storage.Count; a++)
        {
            if (die_to_check == ghost_dice_storage[a]) return true;
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) RollDice();
    }

    public void RollDice()
    {
        // Playing sound effect for rerolling
        AudioManager.instance.PlaySFX("Reroll");

        // removing guard dice
        //guard_drop.GetComponent<Guard>().guard_dice_count = 0;
        
        
        
            //if (guard_dice[a].GetComponent<Dice_code>().default_position != null) guard_dice[a].transform.position = guard_dice[a].GetComponent<Dice_code>().default_position;
            guard_dice.Clear();
        
        // --Restoring-trinket-charges-
        trinket_drop.GetComponent<Trinket>().Recharge(0);
        // ----Rolling-dice-----
        for (int a = 0; a < dice.Count; a++)
        {
            dice[a].GetComponent<Dice_code>().Activate();
            dice[a].GetComponent<Dice_code>().rotating = 126; //126
            //dice[a].GetComponent<Dice_code>().rotating = (a + 1) * 90;
            //dice[a].GetComponent<Dice_code>().Roll();
        }
        for (int a = 0; a < ghost_dice.Count; a++)
        {
            ghost_dice[a].GetComponent<Dice_code>().Activate();
            ghost_dice[a].GetComponent<Dice_code>().rotating = 126;
        }
    }

    
}
