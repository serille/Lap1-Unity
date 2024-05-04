using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDisplayController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            this.gameObject.transform.GetChild(0).gameObject.SetActive(!this.gameObject.transform.GetChild(0).gameObject.activeSelf);
            this.gameObject.transform.GetChild(1).gameObject.SetActive(!this.gameObject.transform.GetChild(1).gameObject.activeSelf);
        }
    }
}
