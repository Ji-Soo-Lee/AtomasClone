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
    private bool isFromMinus = false;

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
        newAtom.GetComponent<IAtom>().SetAtomID(Random.Range(-1, 2));
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
            if (newAtom.GetComponent<IAtom>().GetAtomID() !=-1) {
                line.transform.rotation = Quaternion.Euler(0, 0, (mouseBetween * angle + angle/2) * Mathf.Rad2Deg);
            }

            else if (newAtom.GetComponent<IAtom>().GetAtomID() == -1) {
                line.transform.rotation = Quaternion.Euler(0, 0, (mouseBetween * angle) * Mathf.Rad2Deg);
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            line.SetActive(false);

            if (newAtom.GetComponent<IAtom>().GetAtomID() != -1) {
                newIdx = mouseBetween + 1;
                atomObjects.Insert(newIdx, newAtom);
                //Debug.Log(newIdx);
                atomNum++;

                ArrangeAtoms();

                isFromMinus = false;
                newAtom = Instantiate(atomObj);
                newAtom.GetComponent<IAtom>().SetAtomID(Random.Range(-1, 2));
            }

            else if (newAtom.GetComponent<IAtom>().GetAtomID() == -1) {
                Destroy(newAtom);

                isFromMinus = true;
                newAtom = Instantiate(atomObj);
                newAtom.GetComponent<IAtom>().SetAtomID(atomObjects[mouseBetween].GetComponent<IAtom>().GetAtomID());

                Destroy(atomObjects[mouseBetween]);
                atomObjects.Remove(atomObjects[mouseBetween]);
                atomNum--;

                if (atomNum > 0)
                    ArrangeAtoms();
            }
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
        if (atomNum > 2)
            FuseAtoms(newIdx);
    }

    public void InsertNormalAtom() {
        int frontIdx = (newIdx - 1 + atomNum) % atomNum;
        int backIdx = (newIdx + 1) % atomNum;

        if (atomNum > 2) {
            if (atomObjects[backIdx].GetComponent<IAtom>().GetAtomID() == 0 ) {
                Debug.Log("back");
                FuseAtoms(backIdx);
            }

            if (atomObjects[frontIdx].GetComponent<IAtom>().GetAtomID() == 0) {
                Debug.Log("front");
                FuseAtoms(frontIdx);
            }
        }
    }

    public void FuseAtoms(int plusIdx) {
        int maxID = 0;
        int chainNum = 0;

        int frontIdx = (plusIdx - 1 + atomNum) % atomNum;
        int backIdx = (plusIdx + 1) % atomNum;

        while (atomObjects[frontIdx].GetComponent<IAtom>().GetAtomID() == atomObjects[backIdx].GetComponent<IAtom>().GetAtomID() && atomObjects[frontIdx].GetComponent<IAtom>().GetAtomID() != 0) {
            maxID = Mathf.Max(atomObjects[frontIdx].GetComponent<IAtom>().GetAtomID(), maxID);

            List<int> idxList = new List<int>() {frontIdx, plusIdx, backIdx};
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

            plusIdx = (backIdx - 2 + atomNum) % atomNum;
            frontIdx = (plusIdx - 1 + atomNum) % atomNum;
            backIdx = (plusIdx + 1) % atomNum;

            if (atomNum <= 2) 
                break;
        }        

        if (chainNum > 0) {
            newIdx = plusIdx;
            InsertNormalAtom();
        }

    }
}
