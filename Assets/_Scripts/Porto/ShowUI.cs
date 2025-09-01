using System.Collections;
using UnityEngine;

public class ShowUI : MonoBehaviour {

    public GameObject uiObject;
    
    void Start()
    {
        uiObject.SetActive(false);
    } 

    void OnTriggerEnter(Collider player)
    {
        {
            uiObject.SetActive(true);
            StartCoroutine(HideAfterDelay());
        }
    }

    IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(5);
        uiObject.SetActive(false); // Apenas desativa o UI após 5 segundos
    }
}
