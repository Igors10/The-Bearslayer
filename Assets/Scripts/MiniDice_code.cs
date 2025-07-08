using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniDice_code : MonoBehaviour
{
    public bool player;
    public GameObject owner;
    public int value;
    public bool active = true;
    public string element;
    public int max_value;
    public Sprite[] die_face;
    bool hit;
    float hit_speed = 0.1f;
    float hit_cap;
    [SerializeField] GameObject hit_particles;
    public bool rolling = false;
    float rolling_speed;
    Vector3 starting_scale;

    public GameObject glowing;
    
    [SerializeField] GameObject number;
    [SerializeField] GameObject dying_die;             // Add dying die

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale += new Vector3(0.2f, 0.2f, 0f);
        Roll();
        rolling_speed = owner.GetComponent<Enemy_atributes>().dice_rolling_speed;
        starting_scale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (hit)
        {
            transform.Translate(hit_speed, 0f, 0f);

            if (transform.position.x >= hit_cap && hit_speed > 0f)
            {
                hit_speed *= -1;
                hit_cap -= 0.6f;
                
            }
            else if (hit_speed < 0f && transform.position.x <= hit_cap)
            {
                hit = false;
                owner.GetComponent<Enemy_atributes>().ArrangeDice();
            }
        }
        else if (rolling)
        {
            transform.Rotate(0f, 0f, rolling_speed, Space.Self);

            rolling_speed--;
            if (rolling_speed % 15 == 0 && rolling_speed != 0) Roll();
            if (rolling_speed < 60) rolling_speed /= 1.05f;
            if (rolling_speed <= 0)
            {
                transform.rotation = owner.transform.rotation;
                rolling_speed = owner.GetComponent<Enemy_atributes>().dice_rolling_speed;
                rolling = false;
            }
        }
       
    }

    public void Deactivate()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
        value /= 2;
        if (value == 0) value = 1;
        number.GetComponent<SpriteRenderer>().sprite = die_face[value];
        active = false;
        transform.localScale = starting_scale;

        owner.GetComponent<Enemy_atributes>().dice.Remove(this.gameObject);
        owner.GetComponent<Enemy_atributes>().used_dice.Add(this.gameObject);
    }

    public void Activate()
    {
        active = true;
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        owner.GetComponent<Enemy_atributes>().used_dice.Remove(this.gameObject);
        owner.GetComponent<Enemy_atributes>().dice.Add(this.gameObject);
    }

    public void Hit(int damage_dealt)
    {
        // Soundeffect for dodging
        AudioManager.instance.PlaySFX("Dodge");

        if (value > 1)
        {
            value -= damage_dealt;
            number.GetComponent<SpriteRenderer>().sprite = die_face[value];
            Battle_manager.enemy_damage_number.GetComponent<damage_number>().Show(this.gameObject, (damage_dealt * -1));
        }

        hit = true;
        hit_speed = 0.1f;
        hit_cap = transform.position.x + 0.5f;
        hit_particles.GetComponent<ParticleSystem>().Play();
        Battle_manager.Level.GetComponent<Animation_manager>().FlashEffect(owner.GetComponent<Enemy_atributes>().Sprite, "white", 10);
    }

    public void SelfDestruction()
{
        // Soundeffect for being hit
        AudioManager.instance.PlaySFX("EnemyHit");

        Battle_manager.Level.GetComponent<Animation_manager>().FlashEffect(owner.GetComponent<Enemy_atributes>().Sprite, "red", 10);

        GameObject decoy = Instantiate(dying_die, transform.position, Quaternion.identity);
        decoy.GetComponent<SpriteRenderer>().sprite = die_face[max_value + 2];

        if (active) owner.GetComponent<Enemy_atributes>().dice.Remove(this.gameObject);
        else owner.GetComponent<Enemy_atributes>().used_dice.Remove(this.gameObject);
        owner.GetComponent<Enemy_atributes>().all_dice.Remove(this.gameObject);

        Destroy(this.gameObject);
    }

    public void Roll()
    {
        value = Random.Range(1, max_value + 1);
        number.GetComponent<SpriteRenderer>().sprite = die_face[value];
        //GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }
}
