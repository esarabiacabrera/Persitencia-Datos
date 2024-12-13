using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DatosScene : MonoBehaviour
{

    public static DatosScene Instance;
    public String nombreUsuario;
    public int mejorPuntaje;
    private void Awake() {

        if(Instance!=null){
            Destroy(gameObject);
            return;
        }
        Instance=this;
        DontDestroyOnLoad(gameObject);
    }
}
