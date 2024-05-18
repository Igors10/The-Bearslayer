using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samurai_animation : MonoBehaviour
{
    // Animations

    public Sprite[] samurai_animation;

    int facing_save = 2;
    // 0 Idle
    // 1 Idle v2
    // 2 Movement
    // 3 Dodge
    // 4 Damage
    // 5 Preparing for movement
    // 6 Basic katana
    // 7 katana (1)
    // 8 katana (2)
    // 9 katana (3)
    // 10 Preparing sword 
    // 11 Basic sword
    // 12 sword (1)
    // 13 sword (2)
    // 14 sword (3)

    // Start is called before the first frame update
    void Start()
    {
        current_animation = "idle";
        Sprite = GetComponent<Samurai>().Sprite;
    }

    public string current_animation;
    int timer = 0;

    public int idle_anim_speed;
    
    SpriteRenderer Sprite;

    public void SamuraiAnimation(string name)
    {
        timer = 0;
        current_animation = name;

        switch (name)
        {
            case "move":
                Sprite.sprite = samurai_animation[2];
                if (facing_save != 2)
                {
                    Sprite.flipX = (facing_save == 0) ? true : false;
                    facing_save = 2;
                }
                break;
            case "idle":
                Sprite.sprite = samurai_animation[0];
                break;
            case "hit":
                Sprite.sprite = samurai_animation[4];
                break;
            case "dodge":
                Sprite.sprite = samurai_animation[3];
                break;
            case "prepareAttack":
                RotateTowardsTarget();
                Sprite.sprite = samurai_animation[8];
                break;
            case "katana_1":
                Sprite.sprite = samurai_animation[6];
                
                break;
            case "katana_2":
                Sprite.sprite = samurai_animation[7];
                
                break;
            case "katana_3":
                Sprite.sprite = samurai_animation[8];
                
                break;
        }


    }

    public void AttackAnimation(string weapon, int skill_slot_nr)
    {
        if (weapon == "katana")
        {
            Sprite.sprite = samurai_animation[5 + skill_slot_nr];
        }
    }

    public void Reset()
    {
        if (current_animation != "katana_1")
        {
            SamuraiAnimation("idle");
        }
        
    }

    void RotateTowardsTarget()
    {
        facing_save = (Sprite.flipX) ? 0 : 1;
        Sprite.flipX = (Attack_manager.target.transform.position.x > transform.position.x) ? false : true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (current_animation)
        {
            case "move":
                break;
            case "prepareAttack":
                break;
            case "katana_1":
                break;
                // IDLE ANIMATION

            default:
                timer++;
                if (timer == idle_anim_speed)
                {
                    Sprite.sprite = samurai_animation[1];
                }
                else if (timer == idle_anim_speed * 2)
                {
                    Sprite.sprite = samurai_animation[0];
                    timer = 0;
                }

                break;

        }

    }
}
