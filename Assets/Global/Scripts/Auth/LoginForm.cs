using System.Net.Sockets;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginForm : MonoBehaviour
{
    [System.Serializable]
    private class LoginResponse
    {
        public string Token;
        public string error;
        public string status;
    }

    [SerializeField] InputField email, password;
    [SerializeField] Button submitButton;

    private void Awake()
    {
        submitButton.onClick.AddListener(Login);
        if (PlayerPrefs.HasKey("Token"))
        {
            SceneManager.LoadScene("LudoEntryScene");
        }
    }

    private void Login()
    {
        submitButton.interactable = false;
        PopUp.instance.Loading("Loggin in...");

        try
        {
            using (var wb = new WebClient())
            {

                var data = new NameValueCollection();
                data["email"] = email.text;
                data["password"] = password.text;

                var response = wb.UploadValues(Api.loginUrl, "POST", data);
                string responseInString = Encoding.UTF8.GetString(response);
                Debug.Log(responseInString);

                LoginResponse user = JsonUtility.FromJson<LoginResponse>(responseInString);

                if (user.status == "error")
                {
                    PopUp.instance.ClearAll();
                    PopUp.instance.Error(user.error);
                }

                else
                {
                    PlayerPrefs.SetString("Token", user.Token);
                    PlayerPrefs.Save();
                    PopUp.instance.ClearAll();
                    PopUp.instance.Success("Logged in");
                    SceneManager.LoadScene("LudoEntryScene");
                }

            }

        }
        catch (WebException e)
        {
            PopUp.instance.ClearAll();
            PopUp.instance.Error(e.Message);
        }

        submitButton.interactable = true;
    }

    private void Update()
    {
        if (email.text != "" && password.text != "")
        {
            submitButton.interactable = true;
        }
        else
        {
            submitButton.interactable = false;
        }
    }
}
