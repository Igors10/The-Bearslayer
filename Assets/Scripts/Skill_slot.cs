using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_slot : MonoBehaviour
{
    GameObject die_used;
    GameObject skill;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject glowing;
    [SerializeField] GameObject skill_glow;
    public int skill_slot_nr;
    [SerializeField] GameObject modifier;

    // Start is called before the first frame update
    void Start()
    {
        skill = Instantiate(Samurai_stats.katana_skills[skill_slot_nr - 1], transform.position, Quaternion.identity, panel.transform);
        skill.transform.Translate(0f, 0.9f, 0f);

        skill.GetComponent<skill_visuals>().slot = this.gameObject;
        skill.GetComponent<skill_visuals>().glowing = skill_glow;
        skill.GetComponent<Katana_skill>().slot = this.gameObject;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UseSkill()
    {
        int attack_value = die_used.GetComponent<Dice_code>().value + Samurai_stats.samurai_attack;
        die_used.GetComponent<Dice_code>().text.text = attack_value.ToString();

        die_used.transform.position = transform.position;
        skill.GetComponent<Katana_skill>().Use(die_used);
        modifier.SetActive(true);
        modifier.transform.position = transform.position;
        modifier.transform.Translate(0f, -0.5f, 0f);

        if (die_used.GetComponent<Dice_code>().skill_particles != null) die_used.GetComponent<Dice_code>().skill_particles.SetActive(true);
        //die_used.GetComponent<Dice_code>().text.text = die_used.GetComponent<Dice_code>().value.ToString();
    }

    void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "dice")
        {
            glowing.transform.position = transform.position;
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
        //Debug.Log("Something is touching");
        if (obj.gameObject.tag == "dice" && Battle_manager.activation) //
        {
            if (obj.gameObject == Battle_manager.skill_die) return;
            if (obj.gameObject == Battle_manager.move_die)
            {
                Battle_manager.move_die.GetComponent<Dice_code>().ReturnBack();
                return;
            }
            else if (obj.gameObject.GetComponent<Dice_code>().element != skill.GetComponent<Katana_skill>().element && skill.GetComponent<Katana_skill>().element != "any")
            {
                obj.gameObject.transform.position = obj.gameObject.GetComponent<Dice_code>().default_position.transform.position; 
                return;
            }

            Debug.Log("Added " + obj.GetComponent<Dice_code>().value + " to skill slot");

            // ---------------------------------

            if (Battle_manager.skill_die != null)
            {
                Battle_manager.skill_die.GetComponent<Dice_code>().ReturnBack();
                Battle_manager.skill_die = null;

                //Battle_manager.StopCellTarget();
                skill.GetComponent<Katana_skill>().FindTarget();
            }

            die_used = obj.gameObject;
            

            UseSkill();

            Battle_manager.activation = false;

            // ---------------------------------
        }
    }
}
