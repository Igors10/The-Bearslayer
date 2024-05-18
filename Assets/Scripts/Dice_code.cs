using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dice_code : MonoBehaviour
{
    public Sprite[] die_face;
    public GameObject manager;

    GameObject value_text_reference;
    GameObject value_text;
    
    TrailRenderer trail;

    // Objects to drop to

    public GameObject guard_drop;
    public GameObject move_drop;
    public GameObject ability_drop;
    public GameObject trinket_drop;
    public GameObject hint;

    public TMP_Text text;
    public TMP_Text number_text;
    public TMP_Text number10_text;
    [SerializeField] GameObject number1;
    [SerializeField] GameObject number;
    [SerializeField] GameObject number10;
    public GameObject move_particles;
    public int maxvalue;
    //public Vector3 default_position;
    public GameObject default_position;
    public GameObject skill_particles;
    public Sprite element_sprite;
    public int value;
    public string element;
    public bool interact;
    

    public bool ghost = false;

    // Animation variables
    GameObject hit_part;
    float rotating_speed;

    public bool ranged = false; // Indication that a ranged attack is being used
    bool taken = false;
    public bool set = false;
    public int rotating = 0;
    [SerializeField] bool player1;
    GameObject to_fix_on;

    Vector3 zoom_size;
    public Vector3 normal_size;
    // Start is called before the first frame update
    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        trail.enabled = false;
        normal_size = transform.localScale;
        zoom_size = transform.localScale + new Vector3(0.4f, 0.4f, 0f);

        if (number10 != null)number10_text = number10.GetComponent<TMP_Text>();

        default_position.transform.position = transform.position;

        rotating_speed = manager.GetComponent<Dice_manager>().dice_rotation_speed;

        value_text_reference = manager.GetComponent<Dice_manager>().text_value_reference;
        value_text = Instantiate(value_text_reference, transform.position, Quaternion.identity, this.gameObject.transform); //manager.GetComponent<Dice_manager>().text_values_block.transform
        value_text.transform.Translate(1.2f, 0.1f, 0f);
        text = value_text.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (taken)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
            transform.position = mousePosition;
        }
        else if (to_fix_on)
        {
            transform.position = to_fix_on.transform.position;
        }
    }

    void ChangeFace(bool flashy_number, int damage_value)
    {
        //number.GetComponent<Image>().sprite = die_face[value];
        if (value < 10)
        {
            if (number10 != null)
            {
                number10.SetActive(false);
                number1.SetActive(true);
            }
            number_text.text = value.ToString();
        }
        else
        {
            number10.SetActive(true);
            number1.SetActive(false);
            number10_text.text = value.ToString();
        }
        

        if (flashy_number) manager.GetComponent<Dice_manager>().damage_number.GetComponent<damage_number>().Show(this.gameObject, damage_value);
    }

    public void FixOn(GameObject to_fix)
    {
        to_fix_on = to_fix;
        set = true;
    }

    public void StopFixOn()
    {
        to_fix_on = null;
        set = false;
    }

    //int hp = enemy.GetComponent<code_for_the_enemy>.hp;

    void FixedUpdate()
    {
        // Rolling animation
        if (rotating > 0)
        {
            transform.Rotate(0f, 0f, rotating_speed, Space.Self);
            rotating--;
            if (rotating % 15 == 0 && rotating != 0) Roll();
            if (rotating < 60) rotating_speed /= 1.05f;
            if (rotating == 0)
            {
                transform.rotation = guard_drop.transform.rotation;
                rotating_speed = manager.GetComponent<Dice_manager>().dice_rotation_speed;
            }
        }

        // Being hit animation

        if(hit_part != null)
        {
            hit_part.transform.position = transform.position;
        }
    }

    public void Roll()
    {
        value = Random.Range(1, maxvalue + 1);
        ChangeFace(false, 0);
        if (ghost == false) GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        else GetComponent<Image>().color = new Color(0.4f, 1f, 0.6f, 0.8f);
    }

    bool IsGuard(GameObject dice_to_check)
    {
        
        for (int a = 0; a < manager.GetComponent<Dice_manager>().guard_dice.Count; a++)
        {
            if (dice_to_check == manager.GetComponent<Dice_manager>().guard_dice[a]) return true;
        }

        return false;
    }

    public void Hit(int damage)
    {
        if (IsGuard(this.gameObject))           // Maybe It's me or maybe sometimes it doesn't take it away from the array of guard dice
        {
            Battle_manager.characters[0].GetComponent<Samurai_animation>().SamuraiAnimation("block");
            Instantiate(manager.GetComponent<Dice_manager>().block_effect, new Vector3(Battle_manager.characters[0].GetComponent<Samurai>().Samurai_sprite.transform.position.x, Battle_manager.characters[0].GetComponent<Samurai>().Samurai_sprite.transform.position.y, Battle_manager.characters[0].GetComponent<Samurai>().Samurai_sprite.transform.position.z), Quaternion.identity);
            Deactivate();
        }
        else
        {
            //hit_part = Instantiate(manager.GetComponent<Dice_manager>().hit_particles, transform);
            //hit_part.GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingOrder = 20;

            Battle_manager.characters[0].GetComponent<Samurai_animation>().SamuraiAnimation("dodge");
            Battle_manager.Level.GetComponent<Animation_manager>().FlashEffect(Battle_manager.characters[0].GetComponent<Samurai>().Sprite, "white", 10);

            if (value > 1) value -= damage;
            if (value < 1) Death();
            ChangeFace(true, (damage * -1));
        }
        

    }

    public void MakeMeGhost()
    {
        manager.GetComponent<Dice_manager>().ghost_dice.Add(this.gameObject);
        
        ghost = true;
        //Roll();
        rotating = 126;
    }

    public void Death()
    {
        hit_part = Instantiate(manager.GetComponent<Dice_manager>().hit_particles, transform);
        manager.GetComponent<Dice_manager>().damage_number.GetComponent<damage_number>().Show(this.gameObject, -100);

        Battle_manager.characters[0].GetComponent<Samurai_animation>().SamuraiAnimation("hit");
        Battle_manager.Level.GetComponent<Animation_manager>().FlashEffect(Battle_manager.characters[0].GetComponent<Samurai>().Sprite, "red", 10);

        if (IsGuard(this.gameObject)) manager.GetComponent<Dice_manager>().guard_dice.Remove(this.gameObject);
        manager.GetComponent<Dice_manager>().guard_drop.GetComponent<Guard>().ArrangeGuardDice();
        manager.GetComponent<Dice_manager>().dice.Remove(this.gameObject);

        if (manager.GetComponent<Dice_manager>().dice.Count < 4 && manager.GetComponent<Dice_manager>().dice.Count > 0) manager.GetComponent<Dice_manager>().ReplaceDice(manager.GetComponent<Dice_manager>().ghost_dice_storage[manager.GetComponent<Dice_manager>().dice.Count - 1], this.gameObject, true);

        else if (manager.GetComponent<Dice_manager>().dice.Count == 0) Battle_manager.characters[0].GetComponent<Samurai>().Death(); // <-- Fake death, samrai just disappears from the screen;

        Destroy(this.gameObject);
    }

    public void Activate()
    {
        
        transform.position = default_position.transform.position;
        set = false;
    }

    void OnMouseEnter()
    {
        if (set || rotating > 0) return;
        transform.localScale = zoom_size;
        trail.enabled = true;
    }

    void OnMouseExit()
    {
        trail.enabled = false;
        if (set || rotating > 0) return;
        transform.localScale = normal_size;
        
    }

    void OnMouseDown()
    {
        if (set || rotating > 0) return;

        //Debug.Log("Clicked");
        taken = true;
        Camera_movement.freeze = true;
        //default_position = transform.position;
        Battle_manager.current_die = this.gameObject;
    }

    void OnMouseUp()
    {
        if (set || rotating > 0) return;

        transform.localScale = normal_size;

        if (interact && set == false)
        {
            Battle_manager.activation = true;
            /*
            switch (interact_with)
            {
                case "guard":
                    Debug.Log("Added to guard");
                    if (guard_drop.GetComponent<Guard>().guard_dice_count < 3)
                    {
                        set = true;
                        guard_drop.GetComponent<Guard>().AddToGuard();
                    }
                    break;
                case "move":
                    Debug.Log("Added to move");
                    break;
                case "ability":
                    Debug.Log("Added to ability");
                    break;
                case "trinket":
                    Debug.Log("Added to trinket");
                    trinket_drop.GetComponent<Trinket>().UseTrinket();
                    break;
                default:
                    break;

            }*/
        }
        else
        {
            ReturnBack();
        }


        taken = false;
        Camera_movement.freeze = false;
    }

    public void ReturnBack()
    {
        if (Battle_manager.target_lock != null && (this.gameObject == Battle_manager.skill_die || this.gameObject == move_drop.GetComponent<Move>().die_used_to_move)) Battle_manager.target_lock.GetComponent<Enemy_atributes>().Glowing.SetActive(false);

        if (IsGuard(this.gameObject))  // <<<< I did this check here as a way around a bug. 
        {
            manager.GetComponent<Dice_manager>().guard_dice.Remove(this.gameObject);
            manager.GetComponent<Dice_manager>().guard_drop.GetComponent<Guard>().ArrangeGuardDice();
        }
        else
        {
            GameObject capsule = GameObject.Find("capsule");
            capsule.GetComponent<Capsule_code>().Stop_skill_activation(this.gameObject);  // <<< this line crashed when the guard die was attacked
        }

        StopFixOn();
        transform.position = default_position.transform.position;
        Battle_manager.current_die = null;
        Battle_manager.slash.GetComponent<Slash>().die_used_to_slash = null;
        Battle_manager.ranged.GetComponent<Ranged>().die_used_to_shoot = null;

        text.text = " ";

        ranged = false;
        transform.localScale = normal_size;

        Battle_manager.activation = false;
        //skill_particles.SetActive(false);
    }

    public void Deactivate()
    {
        
        ReturnBack();
        set = true; //kurwa
        value /= 2;
        if (value == 0) value++;
        ChangeFace(false, 0);
        if (ghost == false) GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
        else GetComponent<Image>().color = new Color(0.4f, 1f, 0.6f, 0.4f);
    }

    void OnTriggerStay2D(Collider2D obj)
    {
        if (set) return;

        if (obj.gameObject.tag == "drop_to")
        {
            interact = true;
        }
        /*
        if (obj.gameObject == guard_drop)
        {
            Debug.Log("Hovering over guard");
            //hint.GetComponent<Hint>().Text("Put into guard", true);
            interact_with = "guard";
        }
        if (obj.gameObject == move_drop)
        {
            Debug.Log("Hovering over move");
            //hint.GetComponent<Hint>().Text("Move for " + value.ToString(), true);
            interact_with = "move";
        }
        if (obj.gameObject == ability_drop)
        {
            Debug.Log("Hovering over ability");
            //hint.GetComponent<Hint>().Text("Swing big sword for " + value.ToString(), true);
            interact_with = "ability";
        }
        if (obj.gameObject == trinket_drop)
        {
            Debug.Log("Hovering over trinket");
            //hint.GetComponent<Hint>().Text("Reroll", true);
            interact_with = "trinket";
        }*/
    }

    void OnTriggerExit2D(Collider2D obj)
    {
        //hint.GetComponent<Hint>().Text(".", false);
        interact = false;
    }
}
