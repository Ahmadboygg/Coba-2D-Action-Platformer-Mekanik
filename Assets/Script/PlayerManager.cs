using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerBaseState playerBaseState;
    public PlayerCurrentState playerCurrentState = new PlayerCurrentState();
    [SerializeField] private bool isAttack;
    void Awake()
    {
        playerBaseState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
