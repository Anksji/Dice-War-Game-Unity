using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseDBLogin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        string userId = Random.Range(1, 9999999) + "_id_" + Random.Range(1, 999999) + 
            Random.Range(1, 99999) + Random.Range(1, 99999);

        
        int isLoggedIn = PlayerPrefs.GetInt("is_logged_in", 0);

        Debug.Log("Firebase user is logged in user " + isLoggedIn);
        if (isLoggedIn.Equals(0))
        {
            //not logged in yet
            PlayerPrefs.SetString(FirebaseConstants.USER_ID, userId);
            PlayerPrefs.SetString(FirebaseConstants.USER_NAME, "Temp User");
            string email = userId + "@subgames.com";
            string password = "K!mJo%M00NkoJang@";
            Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

            auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("Firebase user CreateUserWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("Firebase user CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }

                // Firebase user has been created.
                Firebase.Auth.FirebaseUser newUser = task.Result;
                PlayerPrefs.SetInt("is_logged_in", 1);
                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
            });

        }
        else
        {
            Debug.Log("Firebase user is already logged in " + isLoggedIn);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
