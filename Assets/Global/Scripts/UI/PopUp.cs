using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class PopUp : MonoBehaviour
{
    [SerializeField]
    private Sprite _success, _error, _warning, _loading;
    [SerializeField]
    private GameObject success, error, warning, loading;//prefabs
    public static PopUp instance;
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    public Notification Error(string message, bool _autoDelete = true)
    {
        return AddNotification(error, _error, message, _autoDelete);
    }
    public Notification Success(string message, bool _autoDelete = true)
    {
        return AddNotification(success, _success, message, _autoDelete);
    }
    public Notification Loading(string message, bool _autoDelete = true)
    {
        return AddNotification(loading, _loading, message, _autoDelete);
    }
    public Notification Warning(string message, bool _autoDelete = true)
    {
        return AddNotification(warning, _warning, message, _autoDelete);
    }

    private Notification AddNotification(GameObject prefab, Sprite icon, string message, bool _autoDlete = false)
    {

        GameObject g = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
        g.SetActive(true);

        Notification n = g.GetComponent<Notification>();
        n.icon.sprite = icon;
        n.message.text = message;
        n.AutoDelete = _autoDlete;

        Debug.Log(n.message.text + " : " + n.AutoDelete);

        return n;

    }


    public void ClearAll()
    {
        Notification[] notifications = (Notification[])GameObject.FindObjectsOfType<Notification>();
        for (int i = 0; i < notifications.Length; i++)
        {

            if (notifications[i] != null) notifications[i].Remove();
        }
    }


}
