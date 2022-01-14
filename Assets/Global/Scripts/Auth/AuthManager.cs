using System.Text;
using System.Collections.Specialized;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    public string Token;
    public int id;

    private class User
    {
        public int id;
        public string name;
        public string phone;
        public float balance;
        public string email;
        public string username;
        public string status, success, error;
    }



    public string username, Name;
    public float Balance;
    public static AuthManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);


        if (PlayerPrefs.HasKey("Token"))
        {
            Token = PlayerPrefs.GetString("Token");
            Debug.Log("fetching user. Token: " + PlayerPrefs.GetString("Token"));

            StartCoroutine(FetchUser());
        }
        else
        {
            Scene currentScene = SceneManager.GetActiveScene();
            if (currentScene.name != "LoginScene" &&
            currentScene.name != "RegisterScene")
            {

                Logout();
            }
        }
    }

    public void Logout()
    {
        // return;
        StopAllCoroutines();
        PopUp.instance.Error("Logging out");
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("LoginScene");
    }

    private IEnumerator FetchUser()
    {
        while (true)
        {
            try
            {

                using (var wb = new WebClient())
                {
                    // headers
                    wb.Headers.Add("Authorization", "Bearer " + PlayerPrefs.GetString("Token", ""));
                    wb.Headers.Add("Accept", "application/json");

                    var data = new NameValueCollection();

                    var response = wb.UploadValues(Api.getUserUrl, "POST", data);
                    string responseInString = Encoding.UTF8.GetString(response);
                    Debug.Log(responseInString);

                    User user = JsonUtility.FromJson<User>(responseInString);

                    if (user.status == "error")
                    {
                        PopUp.instance.ClearAll();
                        PopUp.instance.Error(user.error);
                        Logout();
                    }

                    else
                    {
                        PlayerPrefs.SetString("username", user.username);
                        PlayerPrefs.SetString("name", user.name);
                        PlayerPrefs.SetString("email", user.email);
                        PlayerPrefs.SetString("phone", user.phone);
                        PlayerPrefs.SetFloat("balance", (float)user.balance);
                        PlayerPrefs.Save();


                        id = user.id;
                        username = user.username;
                        Balance = (float)user.balance;
                        Name = user.name;
                        Debug.Log("player fetched and set to player-prefs");
                    }
                }


            }
            catch (WebException e)
            {
                PopUp.instance.Error(e.Message);
                Logout();
                // if (e.Status == WebExceptionStatus.ProtocolError)
                // {
                //     if (((HttpWebResponse)e.Response).StatusCode == HttpStatusCode.Unauthorized)
                //     {
                //         PopUp.instance.ClearAll();
                //         Logout();
                //     }
                // }
            }

            yield return new WaitForSeconds(60f);

        }
    }

}
