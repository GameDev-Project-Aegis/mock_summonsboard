using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ally1 : MonoBehaviour
{
    public GameObject head1;
    public GameObject head2;
    public GameObject head3;
    public GameObject neck1;
    public GameObject neck2;
    public GameObject neck3;
    public GameObject body;
    public GameObject leg;
    public GameObject tail;
    public GameObject wing1;
    public GameObject wing2;
    public GameObject effect1;
    public GameObject effect2;

    public void Attack()
    {
        head1.GetComponent<Animator>().SetTrigger("attack");
        head2.GetComponent<Animator>().SetTrigger("attack");
        head3.GetComponent<Animator>().SetTrigger("attack");
        neck1.GetComponent<Animator>().SetTrigger("attack");
        neck2.GetComponent<Animator>().SetTrigger("attack");
        neck3.GetComponent<Animator>().SetTrigger("attack");
        body.GetComponent<Animator>().SetTrigger("attack");
        leg.GetComponent<Animator>().SetTrigger("attack");
        tail.GetComponent<Animator>().SetTrigger("attack");
        wing1.GetComponent<Animator>().SetTrigger("attack");
        wing2.GetComponent<Animator>().SetTrigger("attack");
        effect1.GetComponent<Animator>().SetTrigger("attack");
        effect2.GetComponent<Animator>().SetTrigger("attack");
    }
}
