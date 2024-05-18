using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Enemy_atributes : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        for (int a = 0; a < dice_prefabs.Length; a++)
        {
            GameObject new_die = Instantiate(dice_prefabs[0], transform.position, Quaternion.identity, Dicebar.transform);
            //new_die.transform.localScale += new Vector3(1f, 1f, 0f);
            new_die.GetComponent<MiniDice_code>().owner = this.gameObject;
            dice.Add(new_die);
            all_dice.Add(new_die);
        }
        ArrangeDice();

        dicebar_normal_size = Dicebar.transform.localScale;
        dicebar_upscaled_size = Dicebar.transform.localScale += new Vector3(0.5f, 0.5f, 0);
        Dicebar.transform.localScale = dicebar_normal_size;

        defence_modifier = Load_battle.defence_mod;
        resist_modifier = Load_battle.resist_mod;
    }

    public GameObject defence_modifier;
    public GameObject resist_modifier;
    [SerializeField] GameObject[] dice_prefabs;
    [SerializeField] GameObject Dicebar;
    public GameObject Glowing;
    
    public List<GameObject> dice = new List<GameObject>();
    public List<GameObject> all_dice = new List<GameObject>();
    public List<GameObject> used_dice = new List<GameObject>();

    Vector3 dicebar_normal_size;
    Vector3 dicebar_upscaled_size;
    public string name;
    bool turn = false;
    int highlighting = 0;
    int turn_end = 0;
    public int x_coord;
    public SpriteRenderer Sprite;
    public int y_coord;
    public int turn_timer = 0;
    [SerializeField] GameObject indicator;

    [SerializeField] Sprite[] symbol_sprite; // 0- move, 1- attack, 2- defence, 3- reroll
    [SerializeField] GameObject symbol;
    GameObject highlighted_die;

    public float dice_rolling_speed;

    // Stats

    public int ATK;
    public int DEF;
    public int INT;

    public int fire_resist;
    public int water_resist;
    public int poison_resist;
    public int wind_resist;
    public int dark_resist;
    public int lightning_resist;

    //_______
    // Update is called once per frame
    void FixedUpdate()
    {
        if (turn)
        {
            turn_timer++;
            if (turn_timer == 50)   // <--- time it takes in-between enemy turns
            {
                Enemy_movelist.Act(name, this.gameObject);
                turn = false;
                //GetComponent<Skeleton_foe>MoveRandomly(this.gameObject, 1);
            }
        }
        if (turn_end != 0)
        {
            turn_timer--;
            if (turn_timer < 1)
            {
                if (highlighted_die != null) highlighted_die.GetComponent<MiniDice_code>().Deactivate();
                EndDieHighlight();

                indicator.SetActive(false);
                GetComponent<Enemy_animation>().NewAnimation("idle");
                turn_timer = 0;
                Glowing.SetActive(false);
                ArrangeDice();
                Battle_manager.EndTurn(turn_end);
                turn_end = 0;
            }
            
        }
        else if (highlighting > 0 && highlighted_die != null)
        {
            symbol.transform.position = new Vector3(highlighted_die.transform.position.x, highlighted_die.transform.position.y + 0.6f, highlighted_die.transform.position.z);
            highlighted_die.transform.Translate(0f, 0.025f, 0f); 
            highlighting--;
        }
    }

    void Update()
    {
        //if (Battle_manager.skill_die == null) Glowing.SetActive(false);
    }
    // Implement coordinates

    public void Roll()
    {
        //int a = 0;
        while (used_dice.Count > 0)
        {
            used_dice[0].GetComponent<MiniDice_code>().Activate();
        }
        for (int a = 0; a < dice.Count; a++)
        {
            dice[a].GetComponent<MiniDice_code>().rolling = true;
        }

        EndTurn(2);
    }

    public void ArrangeDice()
    {
        if (all_dice.Count > 0) all_dice[0].transform.position = Dicebar.transform.position;

        for (int a = 1; a < all_dice.Count; a++)
        {
            all_dice[a].transform.position = new Vector3(all_dice[a - 1].transform.position.x + 0.5f, all_dice[a - 1].transform.position.y, all_dice[a - 1].transform.position.z);
            for (int b = 0; b <= a; b++)
            {
                all_dice[b].transform.Translate(-0.25f, 0f, 0f);
            }
        }

        for (int a = 0; a < used_dice.Count; a++)
        {
            used_dice[a].transform.Translate(0f, -0.1f, 0f);
        }
    }

    void OnMouseEnter()
    {
        if (Battle_manager.cells[x_coord, y_coord].GetComponent<Cell>().target_cell)
        {
            Battle_manager.cells[x_coord, y_coord].GetComponent<Cell>().Indicator.GetComponent<SpriteRenderer>().color = new Color(1f, 0.25f, 0.25f, 0.95f);
            Glowing.SetActive(true);
            /*
            defence_modifier.SetActive(true);
            defence_modifier.transform.position = new Vector3(transform.position.x + 0.8f, transform.position.y + 1f, transform.position.z);
            defence_modifier.GetComponent<gameobject_holder>().stuff.GetComponent<TMP_Text>().text = "+ " + DEF.ToString();

            
            int resist_value = Attack_manager.FindResist(this.gameObject, Battle_manager.skill_die);
            switch (resist_value)
            {
                case > 0:
                    resist_modifier.SetActive(true);
                    resist_modifier.transform.position = new Vector3(transform.position.x + 0.8f, transform.position.y + 0.5f, transform.position.z);
                    resist_modifier.GetComponent<gameobject_holder>().things[1].GetComponent<TMP_Text>().text = "+ " + Attack_manager.FindResist(this.gameObject, Battle_manager.skill_die).ToString();
                    resist_modifier.GetComponent<gameobject_holder>().things[0].GetComponent<Image>().sprite = Battle_manager.skill_die.GetComponent<Dice_code>().element_sprite;
                    break;
                case < 0:
                    resist_modifier.SetActive(true);
                    resist_modifier.transform.position = new Vector3(transform.position.x + 0.8f, transform.position.y + 0.5f, transform.position.z);
                    resist_value *= -1;
                    resist_modifier.GetComponent<gameobject_holder>().things[1].GetComponent<TMP_Text>().text = "- " + resist_value.ToString();
                    resist_modifier.GetComponent<gameobject_holder>().things[0].GetComponent<Image>().sprite = Battle_manager.skill_die.GetComponent<Dice_code>().element_sprite;
                    break;
            }*/

            
        }

        Dicebar.transform.localScale = dicebar_upscaled_size;
    }


    public void HighlightDice(GameObject Dice_to_highlight, string name)
    {
        //Dice_to_highlight.transform.position.x = Dicebar.transform.position.x;
        //Dice_to_highlight.transform.position.y = Dicebar.transform.position.y;

        highlighted_die = Dice_to_highlight;
        highlighting = 20;

        highlighted_die.transform.localScale += new Vector3(0.2f, 0.2f, 0f);
        highlighted_die.GetComponent<MiniDice_code>().glowing.SetActive(true);
        symbol.SetActive(true);
        //symbol.transform.position = new Vector3(highlighted_die.transform.position.x, highlighted_die.transform.position.y + 0.5f, highlighted_die.transform.position.z);
        

        switch (name)
        {
            case "move":
                symbol.GetComponent<SpriteRenderer>().sprite = symbol_sprite[0];
                highlighted_die.GetComponent<MiniDice_code>().glowing.GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.8f, 0.2f, 1f);
                break;
            case "attack":
                symbol.GetComponent<SpriteRenderer>().sprite = symbol_sprite[1];
                highlighted_die.GetComponent<MiniDice_code>().glowing.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f);
                break;
            case "block":
                symbol.GetComponent<SpriteRenderer>().sprite = symbol_sprite[2];
                highlighted_die.GetComponent<MiniDice_code>().glowing.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 1f, 1f);
                break;
        }

    }

    void OnMouseExit()
    {
        if (Battle_manager.cells[x_coord, y_coord].GetComponent<Cell>().target_cell)
        {
            if (Battle_manager.target_lock != this.gameObject)
            {
                Battle_manager.cells[x_coord, y_coord].GetComponent<Cell>().Indicator.GetComponent<SpriteRenderer>().color = new Color(1f, 0.25f, 0.25f, 0.5f);
                defence_modifier.SetActive(false);
                resist_modifier.SetActive(false);
            }
            Glowing.SetActive(false);
        }

         Dicebar.transform.localScale = dicebar_normal_size;
    }

    void OnMouseUp()
    {
        if (Battle_manager.cells[x_coord, y_coord].GetComponent<Cell>().target_cell)
        {
            /*
            if (Battle_manager.target_lock != this.gameObject && Battle_manager.target_lock != null)
            {
                //Battle_manager.target_lock.GetComponent<Enemy_atributes>().Glowing.SetActive(false);
                Battle_manager.cells[Battle_manager.target_lock.GetComponent<Enemy_atributes>().x_coord, Battle_manager.target_lock.GetComponent<Enemy_atributes>().y_coord].GetComponent<Cell>().Indicator.GetComponent<SpriteRenderer>().color = new Color(1f, 0.25f, 0.25f, 0.5f);
            }*/

            Battle_manager.target_lock = this.gameObject;

            //Battle_manager.current_player.GetComponent<Attack_manager>().SamuraiAttack(Battle_manager.skill_die, Battle_manager.target_lock);
            //Battle_manager.current_player.GetComponent<Attack_manager>().StartCoroutine(Attack(Battle_manager.skill_die, this.gameObject, "none", 0));
            StartCoroutine(Battle_manager.current_player.GetComponent<Attack_manager>().Attack(Battle_manager.skill_die, this.gameObject, "none", 0));
            
            // <Movement start here>
        }
    }

    public void StartTurn()
    {
        indicator.SetActive(true);
        turn = true;
        Debug.Log("Enemy's turn starts");
    }
    
    public void Death()
    {
        Battle_manager.characters.Remove(this.gameObject);
        Destroy(this.gameObject);

        Battle_manager.cells[x_coord, y_coord].tag = "cell_empty";
    }

    public void EndDieHighlight()
    {
        if (highlighted_die != null) highlighted_die.GetComponent<MiniDice_code>().glowing.SetActive(false);
        symbol.SetActive(false);
        //highlighted_die.transform.localScale -= new Vector3(0.2f, 0.2f, 0f);
        highlighted_die = null;
    }

    public void EndTurn(int action_point_cost)
    {
        Debug.Log("Enemy ends turn");
        defence_modifier.SetActive(false); 
       

        

        turn_end = action_point_cost;

    }

    //IEnumerator EndTurn(int ac)

   
}
