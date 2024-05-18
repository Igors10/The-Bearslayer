using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_animation : MonoBehaviour
{
    public Sprite[] animation;
    [SerializeField] int idle_animation_speed;
    SpriteRenderer Sprite;
    bool facing_save = false;

    int timer;
    string current_animation;
    // Start is called before the first frame update
    void Start()
    {
        Sprite = GetComponent<Enemy_atributes>().Sprite;
    }

    void RotateTowardsTarget()
    {
        facing_save = (Sprite.flipX) ? false : true;
        Sprite.flipX = (Battle_manager.characters[0].transform.position.x < transform.position.x) ? false : true;
    }

    public void NewAnimation(string animation_name)
    {
        timer = 0;
        current_animation = animation_name;
        switch (animation_name)
        {
            case "idle":
                Sprite.sprite = (Sprite.sprite == animation[0]) ? animation[1] : animation[0];
                break;
            case "move":
                Sprite.sprite = animation[2];
                break;
            case "hit":
                Sprite.sprite = animation[4];
                break;
            case "attack":
                Sprite.sprite = animation[5];
                break;
            case "prepareAttack":
                Sprite.sprite = animation[3];
                RotateTowardsTarget();
                break;
            case "dodge":
                Sprite.sprite = animation[3];
                break;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer++;
        if (timer == idle_animation_speed && (Sprite.sprite == animation[0] || Sprite.sprite == animation[1]))
        {
            NewAnimation("idle");
        }   
    }
}
