using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using Newtonsoft.Json.Linq;


public class Deposit : MonoBehaviour
{
    public InputField AmountInput, FromInput, ToInput, TransactionIDInput;
    public GameObject dropdown;
    public TMP_Dropdown _dropdown;
    public Button SubmitButton;
    Dictionary<string, string> NameNumbers;


    class DepositResponse
    {
        public string status;
        public string message;
    }

    void OnChange()
    {
        List<TMP_Dropdown.OptionData> menuOptions = _dropdown.options;
        string selected = (menuOptions[_dropdown.value].text);

        ToInput.text = NameNumbers[selected];

    }

    private void Awake()
    {
        NameNumbers = new Dictionary<string, string>();
        _dropdown = dropdown.GetComponent<TMP_Dropdown>();
        _dropdown.onValueChanged.AddListener(delegate { OnChange(); });

        SubmitButton.onClick.AddListener(Submit);

        // set payment methods
        try
        {
            using (var wb = new WebClient())
            {
                wb.Headers.Add("Authorization", "Bearer " + PlayerPrefs.GetString("Token", ""));
                wb.Headers.Add("Accept", "application/json");

                var data = new NameValueCollection();

                var response = wb.UploadValues(Api.baseApiUrl + "/get-payment-methods", "POST", data);
                string responseInString = Encoding.UTF8.GetString(response);

                JObject joResponse = JObject.Parse(responseInString);
                JArray arr = (JArray)joResponse["methods"];


                // Debug.Log(arr.Count);
                for (int i = 0; i < arr.Count; i++)
                {
                    var item = (JObject)arr[i];
                    //do something with item

                    string Name = item["name"].ToString();
                    string Number = item["number"].ToString();

                    NameNumbers.Add(Name, Number);

                    _dropdown.options.Add(new TMP_Dropdown.OptionData(Name));

                }

                OnChange();
            }

        }
        catch (WebException e)
        {
            PopUp.instance.ClearAll();
            PopUp.instance.Error(e.Message);
        }

    }

    private void Submit()
    {
        SubmitButton.interactable = false;
        PopUp.instance.Loading("Submitting...");


        List<TMP_Dropdown.OptionData> menuOptions = _dropdown.options;
        string selected = (menuOptions[_dropdown.value].text);

        try
        {
            using (var wb = new WebClient())
            {

                wb.Headers.Add("Authorization", "Bearer " + PlayerPrefs.GetString("Token", ""));
                wb.Headers.Add("Accept", "application/json");

                var data = new NameValueCollection();
                data["from"] = FromInput.text;
                data["to"] = ToInput.text;
                data["amount"] = AmountInput.text;
                data["accountType"] = selected;
                data["transactionNumber"] = TransactionIDInput.text;

                var response = wb.UploadValues(Api.GetUserDepositUrl(), "POST", data);
                string responseInString = Encoding.UTF8.GetString(response);
                Debug.Log(responseInString);

                DepositResponse deposit = JsonUtility.FromJson<DepositResponse>(responseInString);

                if (deposit.status == "error")
                {
                    PopUp.instance.ClearAll();
                    PopUp.instance.Error(deposit.message);
                }

                else
                {
                    PopUp.instance.ClearAll();
                    PopUp.instance.Success(deposit.message);
                }

            }

        }
        catch (WebException e)
        {
            PopUp.instance.ClearAll();
            PopUp.instance.Error(e.Message);
        }

        SubmitButton.interactable = true;
    }

}
