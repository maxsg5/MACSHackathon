using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCodeHere : MonoBehaviour
{
# int counter = 10;


    // Start is called before the first frame update
    void Start()
    {
        counter = int(input('enter a value: '))
        if counter > 10:
                print('value is greater than 10')
        else
        {
            print('counter is less than 10')
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
