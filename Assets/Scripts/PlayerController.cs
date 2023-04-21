using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerCharacter player;
    [HideInInspector]
    public float h, v;
    [HideInInspector]
    public bool jump;

    public bool attack;
    void Start()
    {
        player = GetComponent<PlayerCharacter>();
    }

    
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        jump = Input.GetButtonDown("Jump");
        attack = Input.GetKeyDown(KeyCode.J);
    }
}
