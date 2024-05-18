using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samurai_stat_loader : MonoBehaviour
{
    [SerializeField] GameObject[] dice;
    public List<GameObject> ghost_dice = new List<GameObject>();
    [SerializeField] GameObject[] katana_skills;
    [SerializeField] int attack;
    [SerializeField] int defence;
    // Start is called before the first frame update
    void Awake()
    {
        for (int a = 0; a < 6; a ++)
        {
            Samurai_stats.samurai_starting_dice[a] = dice[a];
            if (a < 4) Samurai_stats.katana_skills[a] = katana_skills[a];
        }
        for (int a = 0; a < ghost_dice.Count; a++)
        {
            Samurai_stats.samurai_starting_ghost_dice.Add(ghost_dice[a]);
        }

        Samurai_stats.samurai_attack = attack;
        Samurai_stats.samurai_defence = defence;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
