using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastController : MonoBehaviour
{
    [SerializeField] private float scale;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += new Vector3(Time.deltaTime,Time.deltaTime,Time.deltaTime) * scale;
    }
}
