using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class text_script : MonoBehaviour
{
    string name;
    // Start is called before the first frame update
    void Start()
    {
        name = this.gameObject.name;
        switch (name)
        {
            case "attack_modifier":
                GetComponent<TMP_Text>().text = "+ " + Samurai_stats.samurai_attack.ToString();
                break;
        }
    }
}
