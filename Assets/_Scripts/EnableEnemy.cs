using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableEnemy : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    public float delay = 1.0f;

    // Start is called before the first frame update
    void Start()
    {   
        StartCoroutine(EnableNavMeshAgent());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // coroutine to wait before enabling ai
    IEnumerator EnableNavMeshAgent() {
        yield return new WaitForSeconds(delay);
        agent.enabled = true;
        StopCoroutine(EnableNavMeshAgent());
    }
}
