using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCreater : MonoBehaviour
{
    private List<GameObject> uistageList = new List<GameObject>();

    [SerializeField]
    private UIStage stageScript;
    // Use this for initialization
    List<GameObject> childs = new List<GameObject>();
    void Start()
    {

    }

    private void OnEnable()
    {
        StartCoroutine(CreatUiStage());
    }


    IEnumerator CreatUiStage()
    {

        GameObject obj = null;
        yield return new WaitForSeconds(0);
        foreach (var key in GameInstance.GameDatabase.Stages.Keys)
        {
            obj = Instantiate(stageScript.gameObject);
            obj.transform.SetParent(transform, false);
            obj.GetComponent<UIStage>().data = GameInstance.GameDatabase.Stages[key] as NormalStage;
            obj.SetActive(true);
            childs.Add(obj);
            uistageList.Add(obj);
        }

    }


    private void OnDisable()
    {
        for (int i = 0; i < childs.Count; i++)
        {
            Destroy(childs[i]);
        }
        childs.Clear();
    }


}
