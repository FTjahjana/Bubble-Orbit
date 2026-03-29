using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem.Utilities;



public class Master_UICanvas : MonoBehaviour
{
    [System.Serializable]
    public class MainGroup { public string name; public GameObject obj; public UIGroup script; public Animator anim;}
    public List<MainGroup> mainGroups; public Dictionary<string, MainGroup> d = new Dictionary<string, MainGroup>();

    void Awake()
    {
        foreach (MainGroup group in mainGroups){ d[group.name] = group; }
    }
    void Start()
    {
        foreach (MainGroup group in mainGroups)
        {
            if (group.script == null) group.obj.GetComponent<UIGroup>();
            if (group.anim == null) group.obj.GetComponent<Animator>();
            //if (group.audsrc == null) group.obj.GetComponent<UIGroup>();


            if (group.script != null) group.script.msUiRef = this;
        }
    }

}

public class UIGroup : MonoBehaviour
{
    [HideInInspector] public Master_UICanvas msUiRef;
}