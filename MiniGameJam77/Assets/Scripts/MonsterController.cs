using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private int maxHealth;

    private int curHealth;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(){

        PlayHurtAnim();
    }

    private void PlayHurtAnim(){
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}
