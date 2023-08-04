using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageLoader : MonoBehaviour
{

    public List<string> Stages;

    public void LoadStage(int stage)
    {
        SceneManager.LoadScene(Stages[stage]);
    }


}
