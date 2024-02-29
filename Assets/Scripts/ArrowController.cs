using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private GameObject planets;

    [SerializeField] private Transform target;

    public void SetPlanetId(int id)
    {
        target = planets.transform.GetChild(id).transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targ = target.position;
        targ.y = transform.position.y;

        Vector3 objectPos = transform.position;
        targ.x = targ.x - objectPos.x;
        targ.z = targ.z - objectPos.z;

        float angle = Mathf.Atan2(targ.x, targ.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
    }
}
