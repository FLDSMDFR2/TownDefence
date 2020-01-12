using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuilding : BaseDestructibleObj
{
    // Start is called before the first frame update
    void Start()
    {
        Health = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Die())
            return;

    }

    protected override bool Die()
    {
        if (Health <= 0f )
        {
            Destroy(gameObject);
            return true;
        }

        return false;
    }


}
