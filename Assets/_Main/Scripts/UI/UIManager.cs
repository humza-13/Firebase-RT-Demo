﻿using Phoenix.Firebase.Managers;
using UnityEngine;
using TMPro;

namespace Phoenix.UI.Managers
{
    public class UIManager : MonoBehaviour
    {
        [Header("Login Elements")]
        [SerializeField] private TMP_InputField username;
        [SerializeField] private TMP_InputField password;
        [SerializeField] private TMP_Text loginErrorText;
        
        
        public async void Signin()
        {
            Debug.LogError("Loading...");
            await AuthManager._authInstance.CreateOrLoginUser(username.text, password.text);
            Debug.LogError("Process Completed");

        }
}
}