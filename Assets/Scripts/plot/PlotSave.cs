using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class PlotSave : MonoBehaviour
{
    public static PlotSave instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public static bool isClickHomePage;

}
