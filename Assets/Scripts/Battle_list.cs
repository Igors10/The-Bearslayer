using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_list : MonoBehaviour
{
    void Awake()
    {
        obstacle_coordinate = obstacle_coordinate_list;
        samurai_spawn = samurai_spawnpoint;
        enemy_list = enemy_l;
        enemy_types = enemy_t;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public static int battle_nr;

    

    public Vector2[] obstacle_coordinate_list;
    public static Vector2[] obstacle_coordinate;

    public Vector3[] enemy_l;
    public GameObject[] enemy_t;

    public static GameObject[] enemy_types;
    public static Vector3[] enemy_list;

    public Vector2 samurai_spawnpoint;
    public static Vector2 samurai_spawn;

    // Update is called once per frame
    void Update()
    {
        
    }
}
