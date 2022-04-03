using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnMcfawn : MonoBehaviour
{
    public GameObject aiwitnessBot;
    void Start(){
        Instantiate(aiwitnessBot, transform.position, Quaternion.identity);
    }

}
