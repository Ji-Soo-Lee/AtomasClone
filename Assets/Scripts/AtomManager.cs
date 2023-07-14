using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomManager : MonoBehaviour
{
    [SerializeField]
    public GameObject atomObj;
    private Transform originPos;
    private float radius = 3.0f;
    public int atomNum;

    // Start is called before the first frame update
    void Start()
    {
        //Start with 6 atoms
        atomNum = 6;

        for (int n = 0; n < atomNum; n++) {
            float angle = n * Mathf.PI * 2 / atomNum;

            GameObject atom = Instantiate(atomObj);

            atom.transform.position = transform.position + (new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
