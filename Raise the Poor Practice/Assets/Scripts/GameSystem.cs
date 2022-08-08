using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    [Inject]
    GameData gameData;


    InjectObj injectObj = new InjectObj();

    // Start is called before the first frame update
    void Awake()
    {
        injectObj.Inject(this);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
