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
    public List<GameObject> atomObjects = new List<GameObject>();
    private GameObject line;
    private GameObject newAtom;
    private int newIdx;

    // Start is called before the first frame update
    void Start()
    {
        //Start with 6 atoms
        atomNum = 6;

        for (int n = 0; n < atomNum; n++) {
            GameObject atom = Instantiate(atomObj);
            atom.GetComponent<IAtom>().SetAtomID(Random.Range(1, 2));
            atomObjects.Add(atom);
        }

        ArrangeAtoms();

        line = Instantiate(lineObj);
        line.transform.position = transform.position;
        line.SetActive(false);

        newAtom = Instantiate(atomObj);
        newAtom.GetComponent<IAtom>().SetAtomID(Random.Range(0, 2));
        newAtom.transform.position = transform.position;
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
        //Debug.Log(mouseBetween);

        if (Input.GetMouseButton(0)) {
            line.SetActive(true);
            line.transform.GetChild(0).GetComponent<Renderer>().material.color = newAtom.GetComponent<Renderer>().material.color;
            line.transform.rotation = Quaternion.Euler(0, 0, (mouseBetween * angle + angle/2) * Mathf.Rad2Deg);
        }

        if (Input.GetMouseButtonUp(0)) {
            line.SetActive(false);

            newIdx = mouseBetween + 1;
            atomObjects.Insert(newIdx, newAtom);
            Debug.Log(newIdx);
            atomNum++;

            ArrangeAtoms();

            newAtom = Instantiate(atomObj);
            newAtom.GetComponent<IAtom>().SetAtomID(Random.Range(0, 2));
        }
    }

    public void ArrangeAtoms() {
        if (newAtom != null) {
            if (newAtom.GetComponent<IAtom>().GetAtomID() == 0) {
                InsertPlusAtom();
            }

            else {
                InsertNormalAtom();
            }
        }

        for (int n = 0; n < atomNum; n++) {
            float angle = n * Mathf.PI * 2 / atomNum;
            atomObjects[n].transform.position = transform.position + (new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius);
        }
    }

    public void InsertPlusAtom() {
        FuseAtoms(newIdx);
    }

    public void InsertNormalAtom() {
        int frontAtom = (newIdx - 1 + atomNum) % atomNum;
        int backAtom = (newIdx + 1) % atomNum;

        if (atomObjects[backAtom].GetComponent<IAtom>().GetAtomID() == 0 ) {
            FuseAtoms(backAtom);
        }

        else if (atomObjects[frontAtom].GetComponent<IAtom>().GetAtomID() == 0) {
            FuseAtoms(frontAtom);
        }
    }

    public void FuseAtoms(int plusIdx) {
        int maxID = 0;
        int chainNum = 0;

        if (atomNum <= 2) {
            return;
        }

        int frontAtom = (plusIdx - 1 + atomNum) % atomNum;
        int backAtom = (plusIdx + 1) % atomNum;

        while (atomObjects[frontAtom].GetComponent<IAtom>().GetAtomID() == atomObjects[backAtom].GetComponent<IAtom>().GetAtomID() && atomObjects[frontAtom].GetComponent<IAtom>().GetAtomID() != 0) {
            maxID = Mathf.Max(atomObjects[frontAtom].GetComponent<IAtom>().GetAtomID(), maxID);

            List<int> idxList = new List<int>() {frontAtom, plusIdx, backAtom};
            idxList.Sort();

            GameObject fusionAtom = Instantiate(atomObj);
            chainNum++;
            fusionAtom.GetComponent<IAtom>().SetAtomID(maxID + chainNum);
            atomObjects.Insert(idxList[2] + 1, fusionAtom);
            atomNum++;
            
            Destroy(atomObjects[idxList[2]]);
            atomObjects.Remove(atomObjects[idxList[2]]);

            Destroy(atomObjects[idxList[1]]);
            atomObjects.Remove(atomObjects[idxList[1]]);

            Destroy(atomObjects[idxList[0]]);
            atomObjects.Remove(atomObjects[idxList[0]]);

            atomNum -= 3;

            plusIdx = (backAtom - 2 + atomNum) % atomNum;
            frontAtom = (plusIdx - 1 + atomNum) % atomNum;
            backAtom = (plusIdx + 1) % atomNum;

            if (atomNum <= 2) {
                break;
            }
        }        
    }
}
