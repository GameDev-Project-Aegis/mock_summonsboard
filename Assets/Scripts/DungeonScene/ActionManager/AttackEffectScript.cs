using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackEffectScript : MonoBehaviour
{
    public GameObject AttackEffect;
    // public GameObject CircleEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        // if (Input.GetKey(KeyCode.Return)) {
        //     normalAttack();
        //     // StartCoroutine(normalAttack());
        //     Debug.Log("enterキーが押されました");
        // }
    }

    public void normalAttack() {
        AttackEffect.GetComponent<Animator>().SetTrigger("NormalAttack1");
    }

    // IEnumerator normalAttack()
    // {
    //     AttackEffect.GetComponent<Animator>().SetTrigger("NormalAttack1");
        
    //     yield return new WaitForSeconds(0.1f);

    //     AttackEffect.GetComponent<Animator>().SetTrigger("NormalAttack2");
        
    //     yield return new WaitForSeconds(0.1f);

    //     AttackEffect.GetComponent<Animator>().SetTrigger("NormalAttack3");
    // }

    
}