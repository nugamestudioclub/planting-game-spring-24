using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class endUiScript : MonoBehaviour
{
    [SerializeField]
    TextMeshPro current;
    [SerializeField]
    TextMeshPro high;
    // Start is called before the first frame update
    void Start()
    {
        current.text = "You Survived " + GameManager.anyScore + " seconds";
        high.text = "Your high score is " + GameManager.highScore + "seconds";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
