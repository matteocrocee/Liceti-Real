using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.AI;

public class NewBehaviourScript : MonoBehaviour
{
    public Rigidbody rigidB;
    public int speed = 10;
    // Start is called before the first frame update
    void Start()
    {
        rigidB = GetComponent<Rigidbody>();
       
    }

    // Update is called once per frame
    void Update()
    {
       NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
    }
}
