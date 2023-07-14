using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAtom : MonoBehaviour
{
    public int atomID;
    // Start is called before the first frame update
    void Start()
    {
        atomID = Random.Range(0, 101);
        if (atomID%2 == 0)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }

        string ID = atomID.ToString();
        gameObject.GetComponentInChildren<TextMesh>().text = ID;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
