using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingBehaviour_ActivatorDeactivator : MonoBehaviour
{
    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactviate()
    {
        gameObject.SetActive(false);
    }
}
