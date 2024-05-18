using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samurai_stats : MonoBehaviour
{
    public static int samurai_attack;
    public static int samurai_defence;
    public static int samurai_intellegence;
    public static string samurai_trinket;
    public static string samurai_light_weapon;
    public static string samurai_heavy_weapon;

    // Katana Skills

    public static GameObject[] katana_skills = new GameObject[4];
    //public static GameObject[] sword_skills = new GameObject[4];

    // Sword Skills
    public static bool samurai_alive;
    public static bool samurai_turn;
    public static bool samurai_facing_right;

    public static int samurai_cell_x;
    public static int samurai_cell_y;

    public static GameObject[] samurai_starting_dice = new GameObject[6];
    public static List<GameObject> samurai_starting_ghost_dice = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        Battle_manager.current_player = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
