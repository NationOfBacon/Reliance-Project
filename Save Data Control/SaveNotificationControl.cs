using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveNotificationControl : MonoBehaviour
{
    private Animator anim;
    public Animator imageAnim;
    private Vector3 startingPos;

    private void Start()
    {
        anim = GetComponent<Animator>();
        startingPos = transform.position;
    }

    public void RunAnimation()
    {
        gameObject.SetActive(true);
        StartCoroutine(OnSaveGame());
    }

    public IEnumerator OnSaveGame()
    {
        imageAnim.Play("Save_Flash_Anim");
        yield return new WaitForSecondsRealtime(2);
        anim.Play("Save_Notification_End");
        yield return new WaitForSecondsRealtime(1);
        gameObject.SetActive(false);

        gameObject.transform.position = startingPos;

        yield return null;
    }
}
