using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAtom : MonoBehaviour
{
    public int atomID;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake() {
        atomID = Random.Range(1, 101);
        
        if (atomID%8 == 0)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        else if (atomID%8 == 1)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
        else if (atomID%8 == 2)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }
        else if (atomID%8 == 3)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.magenta;
        }
        else if (atomID%8 == 4)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        }
        else if (atomID%8 == 5)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.cyan;
        }
        else if (atomID%8 == 6)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.black;
        }
        else if (atomID%8 == 7)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.gray;
        }

        string ID = atomID.ToString();
        gameObject.GetComponentInChildren<TextMesh>().text = ID;

    }

    public int GetAtomID()
    {
        return atomID;
    }
}
