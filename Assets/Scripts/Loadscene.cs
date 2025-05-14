using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loadscene : MonoBehaviour
{
    // Start is called before the first frame update
     public void pindahscene(string name)
    {
        SceneManager.LoadScene(name);
    }
        
    
}
