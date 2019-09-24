using UnityEngine;
using System.Collections;

public class GameplayController : MonoBehaviour
{

    public void GoBack()
    {
        Application.LoadLevel("LevelMenu");
    }

}
