using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BayAnim : MonoBehaviour
{
    // Ranges from
    // 0=L 1=U 2=R 3=D
    [SerializeField]
    int side;
    // Caches animator
    Animator cacheAnim;
    // Start is called before the first frame update
    void Start()
    {
        cacheAnim = gameObject.GetComponent<Animator>();
        cacheAnim.SetInteger("baySide", side);
    }

    // Update is called once per frame
    void Update()
    {
        if ((int)Time.realtimeSinceStartup % 2 != 0)
        {
            cacheAnim.SetInteger("bayState", 1);
        }
        else
        {
            cacheAnim.SetInteger("bayState", 2);
        }
    }
}
