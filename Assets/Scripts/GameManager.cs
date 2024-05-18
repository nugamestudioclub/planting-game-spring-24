using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    int startingFood;

    // Counted in seconds per food
    [SerializeField]
    private float foodDecrease;

    // Counted in seconds
    [SerializeField]
    private float tideCooldown;

    // Counted in seconds
    [SerializeField]
    private float forageCooldown;

    Timer foodTimer;
    Timer tideTimer;
    Timer forageTimer;

    int food;
    public bool CanForage = true;

    // Start is called before the first frame update
    void Start()
    {
        food = startingFood;
        foodTimer = Timer.Register(foodDecrease, () => decreaseFood(1));
        tideTimer = Timer.Register(tideCooldown, () => doFlood());
        forageTimer = Timer.Register(forageCooldown, () => CanForage = true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void decreaseFood(int val)
    {
        food = Mathf.Max(food - val, 0);
    }

    void doFlood()
    {
        // TODO
    }


}
