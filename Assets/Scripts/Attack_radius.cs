using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_radius : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D obj)
    {
        /*
        if (obj.gameObject.tag == "enemy" && obj.gameObject == Battle_manager.target_lock)
        {
            Debug.Log("<==========>");
            Debug.Log("   attack   ");
            Debug.Log("<==========>");

            Battle_manager.current_player.GetComponent<Attack_manager>().AttackStart();

            
        }*/
    }
}
