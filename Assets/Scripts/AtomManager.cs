using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomManager : MonoBehaviour
{
    public GameObject atomObj;
    public GameObject lineObj;  
    private float radius = 3.0f;
    private Vector3 MousePosition;
    public int atomNum;
    private List<GameObject> atomObjects = new List<GameObject>();
    private GameObject line;
    private GameObject newAtom;

    // Start is called before the first frame update
    void Start()
    {
        //Start with 6 atoms
        atomNum = 6;

        for (int n = 0; n < atomNum; n++) {
            GameObject atom = Instantiate(atomObj);
            atomObjects.Add(atom);
        }

        ArrangeAtoms();

        line = Instantiate(lineObj);
        line.transform.position = transform.position;
        line.SetActive(false);

        newAtom = Instantiate(atomObj);
    }

    // Update is called once per frame
    void Update()
    {
        MousePosition = Input.mousePosition;
        MousePosition = Camera.main.ScreenToWorldPoint(MousePosition);
        //Debug.Log(MousePosition);

        float mouseAngle = Mathf.Atan2(MousePosition.y, MousePosition.x);
        //Debug.Log(mouseAngle);
        if (mouseAngle < 0) {
            mouseAngle = (2* Mathf.PI - Mathf.Abs(mouseAngle));
        }
        //Debug.Log(mouseAngle);
        float angle = (2 * Mathf.PI / atomNum);

        int mouseBetween = (int) (mouseAngle / angle);
        Debug.Log(mouseBetween);

        if (Input.GetMouseButton(0)) {
            line.SetActive(true);
            line.transform.rotation = Quaternion.Euler(0, 0, (mouseBetween * angle + angle/2) * Mathf.Rad2Deg);
        }

        if (Input.GetMouseButtonUp(0)) {
            line.SetActive(false);

            atomObjects.Insert(mouseBetween + 1, newAtom);
            atomNum++;

            ArrangeAtoms();

            newAtom = Instantiate(atomObj);
        }
    }

    public void ArrangeAtoms() {
        for (int n = 0; n < atomNum; n++) {
            float angle = n * Mathf.PI * 2 / atomNum;
            atomObjects[n].transform.position = transform.position + (new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius);
        }
    }
}
