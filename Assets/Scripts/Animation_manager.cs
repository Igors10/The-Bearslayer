using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_manager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    GameObject current;
    Vector3 target_position;
    float animation_speed;
    string animation_type = "none";
    float water_turn = 0f;
    char water_turn_coord;
    char water_turn_sign;
    string add = "none";

    // Variables for flash
    SpriteRenderer sprite_to_flash = null;
    Material def;
    [SerializeField] Material white;
    [SerializeField] Material red;
    int flash_timer = 0;
    // -------------------
    
    public void FlashEffect(SpriteRenderer sprite, string color, int time)
    {
        sprite_to_flash = sprite;
        def = sprite_to_flash.material;

        switch (color)
        {
            case "red":
                sprite_to_flash.material = red;
                break;
            default:
                sprite_to_flash.material = white;
                break;
        }

        flash_timer = time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Flash animation code
        if (sprite_to_flash != null)
        {
            flash_timer--;
            if (flash_timer < 1)
            {
                sprite_to_flash.material = def;
                sprite_to_flash = null;
            }
        }
        // --------------------

        if (current != null)
        {
            switch (animation_type)
            {
                case "movement":
                    current.transform.position = Vector3.MoveTowards(current.transform.position, target_position, animation_speed);
                    if (current.transform.position == target_position)
                    {
                        //current = null;

                        EndAnimation();
                    }
                    else
                    {
                        if (water_turn >= 0f && Mathf.Abs(current.transform.position.x - target_position.x) < 2f && Mathf.Abs(current.transform.position.y - target_position.y) < 2f)
                        {
                            water_turn -= 0.005f;
                            if (water_turn_coord == 'x')
                            {
                                target_position.y = (water_turn_sign == '+') ? target_position.y + 0.005f : target_position.y - 0.0005f;
                            }
                            else
                            {
                                target_position.x = (water_turn_sign == '+') ? target_position.x + 0.005f : target_position.x - 0.0005f;
                            }
                            
                            
                        }
                    }
                    break;
                case "water_movement":

                    break;

            }

            
        }

        
    }

    public void Animation_movement(GameObject to_animate, Vector3 move_to, float speed, string additional)
    {
        StartAnimation();

        animation_type = "movement";

        switch (additional)
        {
            case "water_movement":
                add = additional;

                if (Mathf.Abs(to_animate.transform.position.x - move_to.x) > Mathf.Abs(to_animate.transform.position.y - move_to.y))
                {
                    water_turn_sign = (move_to.y > to_animate.transform.position.y) ? '+' : '-';
                    move_to.y = (move_to.y > to_animate.transform.position.y) ? move_to.y - 1.5f : move_to.y + 1.5f;
                    water_turn = 1.5f;
                    water_turn_coord = 'y';
                    

                }
                else
                {
                    water_turn_sign = (move_to.x > to_animate.transform.position.x) ? '+' : '-';
                    move_to.x = (move_to.x > to_animate.transform.position.x) ? move_to.x - 0.75f : move_to.x + 0.75f;
                    water_turn = 0.75f;
                    water_turn_coord = 'x';
                }
                break;
            default:
                break;
        }

        animation_speed = speed;
        current = to_animate;
        target_position = move_to;

    }

    void StartAnimation()
    {
        Battle_manager.ongoing_animation = true;
        if (Battle_manager.current_player != null)
        {
            if (Battle_manager.current_player.name == "Samurai")
            {
                //Battle_manager.current_player.GetComponent<Samurai>().UI.SetActive(false);
            }
        }
    }

    void EndAnimation()
    {
        Battle_manager.ongoing_animation = false;

        animation_type = "none";
        add = "none";

        if (Battle_manager.current_player != null)
        {
            if (current == Battle_manager.characters[0]) //Battle_manager.current_player.name == "Samurai"
            {
                current = null;
                Battle_manager.current_player.GetComponent<Samurai>().UI.SetActive(true);
                Battle_manager.current_player.GetComponent<Samurai>().EndTurn(1);
            }
            else
            {
                current = null;
                Battle_manager.current_player.GetComponent<Enemy_atributes>().EndTurn(2);
            }
        }
    }
}
