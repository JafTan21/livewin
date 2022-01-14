using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Notification : MonoBehaviour
{
    public bool AutoDelete;
    public static List<Notification> notifications = new List<Notification>();
    public Image icon;
    public TextMeshProUGUI message;

    private void Awake()
    {
        AutoDelete = true;
    }

    private void OnEnable()
    {
        gameObject.transform.localScale = Vector3.zero;
        gameObject.LeanScale(Vector3.one, 0.15f);

        notifications.Add(this);
        if (AutoDelete == true) StartCoroutine(Remove_ENUM());
    }

    public void Remove()
    {
        if (gameObject != null)
            gameObject
                .LeanScale(new Vector3(1.1f, 1.1f, 0), 0.1f)
                .setEaseInExpo()
                .setOnComplete(onComplete);
    }

    IEnumerator Remove_ENUM()
    {
        yield return new WaitForSeconds(3);
        if (gameObject != null)
            gameObject
                .LeanScale(new Vector3(1.1f, 1.1f, 0), 0.1f)
                .setEaseInExpo()
                .setOnComplete(onComplete);
    }

    void onComplete()
    {
        gameObject
            .LeanScale(Vector3.zero, 0.2f)
            .setOnComplete(DestroyNotification);
    }

    void DestroyNotification()
    {
        notifications.Remove(this);
        Destroy(gameObject);
    }
}