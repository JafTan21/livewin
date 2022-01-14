// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

public class Api
{
    public static string baseUrl = "http://127.0.0.1:8000"; //
    public static string baseApiUrl = baseUrl + "/api";



    public static string registerUrl = baseApiUrl + "/register";
    public static string loginUrl = baseApiUrl + "/login";

    public static string getUserUrl = baseApiUrl + "/get-user";
    public static string GetUserDepositUrl()
    {
        return baseApiUrl + "/deposit";
    }

    public static string GetUserWithdrawUrl()
    {
        return baseUrl + "/withdraw";
    }
}
