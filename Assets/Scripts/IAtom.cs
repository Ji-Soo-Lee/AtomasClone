using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAtom : MonoBehaviour
{
    public int atomID;
    public List<Color> atomColors = new List<Color>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetAtomID()
    {
        return atomID;
    }

    public void SetAtomID(int _atomID)
    {
        atomID = _atomID;
        // 0 as plus, -1 as minus

        if (atomID > 0) {
            gameObject.GetComponent<Renderer>().material.color = atomColors[atomID%10];

            string ID = atomID.ToString();
            gameObject.GetComponentInChildren<TextMesh>().text = ID;
        }

        else if (atomID == 0) {
            gameObject.GetComponent<Renderer>().material.color = atomColors[10];
            gameObject.GetComponentInChildren<TextMesh>().text = "+";
        }

        else if (atomID == -1) {
            gameObject.GetComponent<Renderer>().material.color = atomColors[11];
            gameObject.GetComponentInChildren<TextMesh>().text = "-";
        }

        else if (atomID == -2) {
            gameObject.GetComponent<Renderer>().material.color = atomColors[12];
        }

        else if (atomID == -3) {
            gameObject.GetComponent<Renderer>().material.color = atomColors[13];
            gameObject.GetComponentInChildren<TextMesh>().text = "+";
        }
    }
}
