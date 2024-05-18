using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attack_manager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       dice_manager = GameObject.Find("Dice");
    }

    GameObject dice_manager;
    [SerializeField] GameObject target_dice_position;
    [SerializeField] GameObject samurai_dice_position;
    [SerializeField] float dice_upscale;
    bool samurai_dice_prep;
    bool target_dice_prep;
    bool dice_ready;
    bool slow_motion;
    int activation = 0;
    float filter_color = 1f;
    int slow_motion_timer = 0;
    GameObject samurai_dice;
    [SerializeField] GameObject Hit_filter;
    GameObject target_dice;
    public static GameObject target;

    int resist;

    // Update is called once per frame
    void FixedUpdate()
    {
        /*
        if (samurai_dice_prep)
        {
            samurai_dice.transform.position = Vector3.MoveTowards(samurai_dice.transform.position, samurai_dice_position.transform.position, 3f);
            if (samurai_dice.transform.position == samurai_dice_position.transform.position)
            {
                activation++;
                samurai_dice.GetComponent<Dice_code>().FixOn(samurai_dice.GetComponent<Dice_code>().manager.GetComponent<Dice_manager>().katana_dice_position);
                samurai_dice_prep = false;
            }
        }
        /*
        if (target_dice_prep)
        {
            target_dice.transform.position = Vector3.MoveTowards(target_dice.transform.position, target_dice_position.transform.position, 3f);
            if (target_dice.transform.position == target_dice_position.transform.position)
            {
                activation++;
                target_dice_prep = false;
            }
        }*/
        /*
        else if (activation == 1)
        {
            activation = 0;
            Battle_manager.cells[Battle_manager.move_cell_lock.GetComponent<Cell>().x, Battle_manager.move_cell_lock.GetComponent<Cell>().y].GetComponent<Cell>().Move();
        }
        else if (slow_motion)
        {
            slow_motion_timer++;
            if (slow_motion_timer == 4) Attack(samurai_dice.GetComponent<Dice_code>().value, target);
            if (slow_motion_timer == 9)
            {
                slow_motion_timer = 0;
                slow_motion = false;
                Time.timeScale = 1f;
            }
        }
        */
        if (Hit_filter.activeSelf )
        {
            Hit_filter.GetComponent<Image>().color = new Color(1f, 1f, 1f, filter_color);
            filter_color -= 0.05f;
            if (filter_color <= 0f) Hit_filter.SetActive(false);
        }
    }
    /*
    public static int FindResist(GameObject obj, GameObject die)
    {
        switch (die.GetComponent<Dice_code>().element)
        {
            case "fire":
                return obj.GetComponent<Enemy_atributes>().fire_resist;
                
            case "lightning":
                return obj.GetComponent<Enemy_atributes>().lightning_resist;
                
            case "water":
                return obj.GetComponent<Enemy_atributes>().water_resist;
                
            case "dark":
                return obj.GetComponent<Enemy_atributes>().dark_resist;
                
            case "wind":
                return obj.GetComponent<Enemy_atributes>().wind_resist;
                
            case "poison":
                return obj.GetComponent<Enemy_atributes>().poison_resist;
                
            default:
                return 0;
                

        }
    }

    void ActualAttack()
    {
        // Effects for attack if you hit or not and all the math behind it

        if (samurai_dice.GetComponent<Dice_code>().value + Samurai_stats.samurai_attack > target_dice.GetComponent<MiniDice_code>().value + target.GetComponent<Enemy_atributes>().DEF + resist)
        {
            Battle_manager.current_skill.GetComponent<Katana_skill>().Attack_options(samurai_dice, target, target_dice, "hit");

            Hit_filter.SetActive(true);
            filter_color = 1f;

            target_dice.GetComponent<MiniDice_code>().SelfDestruction();    
            
            // ***** Animation of the destruciton *******

            if (target.GetComponent<Enemy_atributes>().dice.Count < 1 && target.GetComponent<Enemy_atributes>().used_dice.Count < 1) target.GetComponent<Enemy_atributes>().Death();
        }
        else
        {
            //target_dice.transform.localScale -= new Vector3(dice_upscale, dice_upscale, 0);       Making the die small again

            // ***** Animation of a knockback *******
            
            target_dice.GetComponent<MiniDice_code>().Hit();

            Battle_manager.current_skill.GetComponent<Katana_skill>().Attack_options(samurai_dice, target, target_dice, "miss");

            if (target_dice.GetComponent< MiniDice_code>().value > 1)
            {
                target_dice.GetComponent<MiniDice_code>().value--;
                target_dice.GetComponent<SpriteRenderer>().sprite = target_dice.GetComponent<MiniDice_code>().die_face[target_dice.GetComponent<MiniDice_code>().value];
            }
        }

        Battle_manager.current_skill.GetComponent<Katana_skill>().Attack_options(samurai_dice, target, target_dice, "end");
    }*/

    void StartAttackAnimation(GameObject die)
    {
        bool close_combat = (Battle_manager.slash.GetComponent<Slash>().die_used_to_slash == null) ? false : true;

        if (close_combat)
        {
            GetComponent<Samurai_animation>().SamuraiAnimation("katana_1");
        }
        else
        {

        }
    }

    public GameObject PickRandomSamuraiDie()
    {
        GameObject dice_manager = GameObject.Find("Dice");

        GameObject dice_to_return;

        if (dice_manager.GetComponent<Dice_manager>().guard_dice.Count > 0)
        {
            dice_to_return = dice_manager.GetComponent<Dice_manager>().guard_dice[Random.Range(0, dice_manager.GetComponent<Dice_manager>().guard_dice.Count)];
        }
        else
        {
            dice_to_return = dice_manager.GetComponent<Dice_manager>().dice[Random.Range(0, dice_manager.GetComponent<Dice_manager>().dice.Count)];
            if (dice_to_return.GetComponent<Dice_code>().ghost) dice_to_return = PickRandomSamuraiDie();
        }

        return dice_to_return;
    }

    public void AttackOnSamurai(GameObject attacking_dice, GameObject Attacker)
    {
        int attacking_value = attacking_dice.GetComponent<MiniDice_code>().value;

        //GameObject die_under_attack = PickRandomSamuraiDie();

        GameObject die_under_attack = Battle_manager.above_samurai_dice;

       if (die_under_attack.GetComponent<Dice_code>().value > attacking_dice.GetComponent<MiniDice_code>().value)
        {
            die_under_attack.GetComponent<Dice_code>().Hit(attacking_value);
            Debug.Log("Die was hit");
        }
       else
        {
            die_under_attack.GetComponent<Dice_code>().Death();
            Debug.Log("Die was hit to death");
        }
        
        //attacking_dice.GetComponent<MiniDice_code>().Deactivate();

        //Attacker.GetComponent<Enemy_atributes>().EndTurn(2);
    }

    [SerializeField] GameObject Above_samurai_die_position;

    public void PutDieAboveSamurai(GameObject Above_samurai_die)
    {

        Above_samurai_die_position.transform.position = Battle_manager.characters[0].transform.position;
        Above_samurai_die_position.transform.Translate(-0.25f, 2f, 0f);
        Above_samurai_die.transform.position = Above_samurai_die_position.transform.position;
        Above_samurai_die.transform.localScale -= new Vector3(0.3f, 0.3f, 0);

        //Above_samurai_die.GetComponent<Image>().sprite = Battle_manager.skill_die.GetComponent<Image>().sprite;
        Battle_manager.above_samurai_dice = Above_samurai_die;
    }

    public IEnumerator Attack(GameObject attack_dice, GameObject target_character, string additional, int modifier)              // <---- Here is where the attack happens
    {
        Battle_manager.StopCellTarget();

        target = target_character;
        int attack_value = attack_dice.GetComponent<Dice_code>().value + modifier;

        // TO DO -> the thing that appears above samurai and imitates attack die

        PutDieAboveSamurai(Battle_manager.skill_die);
        

        dice_manager.GetComponent<Dice_manager>().dice_to_deactivate.Add(Battle_manager.skill_die);


        GameObject block_dice;

        if (attack_dice.GetComponent<Dice_code>().ranged) block_dice = PickingRandomDice(target_character);
        else
        {
            switch (attack_dice.GetComponent<Dice_code>().element)
            {
                case "fire":
                    block_dice = PickingHighestDice(target_character);
                    ShowAttackName(0);
                    break;
                case "dark":
                    block_dice = PickingLowestDice(target_character);
                    ShowAttackName(2);
                    break;
                case "wind":
                    block_dice = PickingRandomDice(target_character);
                    ShowAttackName(1);
                    break;
                default:
                    block_dice = PickingRandomDice(target_character);
                    break;

            }
        }

        

        block_dice.GetComponent<MiniDice_code>().owner.GetComponent<Enemy_atributes>().HighlightDice(block_dice, "block");

        GetComponent<Samurai_animation>().SamuraiAnimation("prepareAttack");

        yield return new WaitForSeconds(1f);

        StartAttackAnimation(Battle_manager.skill_die);

        if (attack_value >= block_dice.GetComponent<MiniDice_code>().value)
        {
            //Battle_manager.current_skill.GetComponent<Katana_skill>().Attack_options(samurai_dice, target, target_dice, "hit");

            //Hit_filter.SetActive(true);          <---- white flash for the whole screen
            filter_color = 1f;

            block_dice.GetComponent<MiniDice_code>().SelfDestruction();

            // ***** Animation of the destruciton *******

            if (target_character.GetComponent<Enemy_atributes>().dice.Count < 1 && target_character.GetComponent<Enemy_atributes>().used_dice.Count < 1) target_character.GetComponent<Enemy_atributes>().Death();
        }
        else
        {
            //target_dice.transform.localScale -= new Vector3(dice_upscale, dice_upscale, 0);       Making the die small again

            // ***** Animation of a knockback *******

            block_dice.GetComponent<MiniDice_code>().Hit(attack_value);

            //Battle_manager.current_skill.GetComponent<Katana_skill>().Attack_options(samurai_dice, target, target_dice, "miss");

            
        }
        block_dice.GetComponent<MiniDice_code>().owner.GetComponent<Enemy_atributes>().EndDieHighlight();


        yield return new WaitForSeconds(1f);

        //Battle_manager.skill_die.GetComponent<Dice_code>().ReturnBack();
        GetComponent<Samurai_animation>().SamuraiAnimation("idle");
        //Above_samurai_die.GetComponent<Image>().sprite = null;

        if (Battle_manager.skill_die.GetComponent<Dice_code>().element == "wind")
        {
            Battle_manager.current_player.GetComponent<Samurai>().EndTurn(1);
        }
        else Battle_manager.current_player.GetComponent<Samurai>().EndTurn(2);

    }

    
    
    public void ShowAttackName(int nr)
    {
        GetComponent<Samurai>().attack_name_box.GetComponent<Attack_name>().SpriteColor = 1.5f;
        GetComponent<Samurai>().attack_name_box.GetComponent<SpriteRenderer>().sprite = Battle_manager.Level.GetComponent<Storage>().sprites[nr];
    }

    List<GameObject> GettingDiceTogether(GameObject target)
    {
        List<GameObject> all_dice = new List<GameObject>();
        for (int a = 0; a < target.GetComponent<Enemy_atributes>().dice.Count; a++)
        {
            all_dice.Add(target.GetComponent<Enemy_atributes>().dice[a]);
        }
        for (int a = 0; a < target.GetComponent<Enemy_atributes>().used_dice.Count; a++)
        {
            all_dice.Add(target.GetComponent<Enemy_atributes>().used_dice[a]);
        }

        return all_dice;
    }

    GameObject PickingHighestDice(GameObject target)
    {
        List<GameObject> all_dice = GettingDiceTogether(target);

        GameObject chosen_die = null;

        for (int a = all_dice.Count - 1; a >= 0; a--)
        {
            if (chosen_die == null) chosen_die = all_dice[a];
            else if (chosen_die.GetComponent<MiniDice_code>().value < all_dice[a].GetComponent<MiniDice_code>().value) chosen_die = all_dice[a];
        }

        return chosen_die;
    }

    GameObject PickingLowestDice(GameObject target)
    {
        List<GameObject> all_dice = GettingDiceTogether(target);
        GameObject chosen_die = null;

        for (int a = all_dice.Count - 1; a >= 0; a--)
        {
            if (chosen_die == null) chosen_die = all_dice[a];
            else if (chosen_die.GetComponent<MiniDice_code>().value > all_dice[a].GetComponent<MiniDice_code>().value) chosen_die = all_dice[a];
        }

        return chosen_die;
    }


    GameObject PickingRandomDice(GameObject target)
    {
        List<GameObject> all_dice = GettingDiceTogether(target);

        int random_dice_nr = Random.Range(0, all_dice.Count);
        GameObject chosen_die = all_dice[random_dice_nr];

        while (all_dice.Count > 0)
        {
            all_dice.Remove(all_dice[0]);
        }

        return chosen_die;

    }
}
