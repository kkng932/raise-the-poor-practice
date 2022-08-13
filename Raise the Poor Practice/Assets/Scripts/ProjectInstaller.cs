using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class ProjectInstaller : MonoBehaviour
{

    // Start is called before the first frame update
    void Awake()
    {

        var data = Resources.Load<GameData>("data");
        InjectContainer.Instance.Reset();
        InjectContainer.Instance.Regist(data);
        InjectContainer.Instance.Regist(new UserData());
        GameObject.DontDestroyOnLoad(this);

    }


}
