using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveUIonHitTag : MonoBehaviour
{
    [SerializeField] GameObject Hint;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Hint.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(hideUI());
        }
    }
    IEnumerator hideUI()
    {
        yield return new WaitForSeconds(2f);
        Hint.GetComponent<Animator>().Play("FadeOut");
        Invoke("hideUiMethod", 0.5f);
        
    }

    void hideUiMethod()
    {
        Hint.SetActive(false);
    }
}
