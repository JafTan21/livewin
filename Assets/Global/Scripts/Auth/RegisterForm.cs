using System;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class RegisterForm : MonoBehaviour
{
    [System.Serializable]
    private class RegisterResponse
    {
        public string status;

        public string Token;

        public string error;
    }






    [SerializeField] InputField fullname, email, username, phone, password, confrim_password;
    [SerializeField] Button submitButton;

    private void Awake()
    {
        submitButton.onClick.AddListener(Register);

        if (PlayerPrefs.HasKey("Token"))
        {
            SceneManager.LoadScene("LudoEntryScene");
        }
    }

    private void Register()
    {
        if (password.text != confrim_password.text)
        {
            PopUp.instance.Error("Password do not match");
            return;
        }
        PopUp.instance.ClearAll();

        PopUp.instance.Loading("Creating your account...");
        submitButton.interactable = false;
        Debug.Log("submiting");
        try
        {
            using (var wb = new WebClient())
            {

                // set header
                wb.Headers.Add("Accept", "application/json");

                // set form data
                var data = new NameValueCollection();
                data["name"] = fullname.text;
                data["email"] = email.text;
                data["phone"] = phone.text;
                data["password"] = password.text;
                data["username"] = username.text;
                data["token_name"] = Random.Range(100, 1000).ToString();

                var response = wb.UploadValues(Api.registerUrl, "POST", data);
                string responseInString = Encoding.UTF8.GetString(response);
                Debug.Log(responseInString);
                RegisterResponse user = JsonUtility.FromJson<RegisterResponse>(responseInString);
                if (user.status == "success")
                {
                    Debug.Log(user.Token);
                    PlayerPrefs.SetString("Token", user.Token);
                    PlayerPrefs.Save();
                    PopUp.instance.ClearAll();
                    PopUp.instance.Success("Registered successfully");
                    SceneManager.LoadScene("LudoEntryScene");

                }
                else
                {
                    PopUp.instance.ClearAll();

                    PopUp.instance.Error(user.error);
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
        if (email.text == ""
        || fullname.text == ""
        || password.text == ""
        || confrim_password.text == ""
        || phone.text == ""
        || username.text == ""
        )
        {
            submitButton.interactable = false;
        }

        else
        {
            submitButton.interactable = true;
        }
    }

}
