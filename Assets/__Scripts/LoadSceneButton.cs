using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour{
    public Button loadSceneButton;

    void Start () {
        Button btn = loadSceneButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick () {
        SceneManager.LoadScene( "_Scene_0" );
    }
}
