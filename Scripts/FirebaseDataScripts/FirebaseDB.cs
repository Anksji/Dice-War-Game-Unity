using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;

public class FirebaseDB : MonoBehaviour
{
    // Start is called before the first frame update
    
    FirebaseFirestore firestoreDb;
    string userId;
    string userName;
    void Start()
    {
        userId = PlayerPrefs.GetString(FirebaseConstants.USER_ID);
        userName = PlayerPrefs.GetString(FirebaseConstants.USER_NAME, "Temp User");
        //mDatabase = FirebaseDatabase.DefaultInstance;
        firestoreDb = FirebaseFirestore.DefaultInstance;

        checkNewOnlinePlayRequest();
         
        //updateScoreToServer();
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void addNewOnlinePlayRequest()
    {
        if (isOtherUserFound == true)
        {
            return;
        }
        
        DocumentReference docRef = firestoreDb.Collection("playRequest").Document(userId);
        Dictionary<string, object> user = new Dictionary<string, object>
{
        { "user_id", userId},
        { "userName", userName },
        { "is_request_open", true },
        { "request_time", FieldValue.ServerTimestamp},
};
        docRef.SetAsync(user).ContinueWithOnMainThread(task => {
            Debug.Log("Added data to the alovelace document in the users collection.");
        });

    }

    private bool isOtherUserFound = false;
    public void checkNewOnlinePlayRequest(){

        Firebase.Firestore.Query query = firestoreDb.Collection("playRequest")
            .WhereEqualTo("is_request_open",true)
            .WhereNotEqualTo("user_id", userId).Limit(1);

        ListenerRegistration listener = query.Listen(snapshot => {
            Debug.Log("showing document Callback received query snapshot." + snapshot);
            Debug.Log("showing document Current cities in California:");

            foreach(DocumentSnapshot documentSnapshot in snapshot.Documents)
            {
                Debug.Log("showing document data "+documentSnapshot);
                isOtherUserFound = true;
                firestoreDb.Collection("playRequest").Document(documentSnapshot.Id)
                .DeleteAsync();
                connectWithOtherUser(documentSnapshot);
            }
        });
        Invoke("addNewOnlinePlayRequest", 30f);
    }

    public void connectWithOtherUser(DocumentSnapshot documentSnapshot)
    {
        FirebaseDatabase firebase = FirebaseDatabase.GetInstance(FirebaseConstants.SERVER_URL);
        DatabaseReference mDatabase = firebase.GetReference("livegames/" +userId+"/other_user/"+documentSnapshot.Id);
        Dictionary<string, object> childUpdates = new Dictionary<string, object>();
        childUpdates["/castle_life/"] = 50;
        childUpdates["/is_won/"] = false;
        childUpdates["/is_game_running/"] = true;
        childUpdates["/dice_outcome/"] = 6;
        mDatabase.UpdateChildrenAsync(childUpdates);

        mDatabase = firebase.GetReference("livegames/" + documentSnapshot.Id+ "/other_user/" + userId);
        Dictionary<string, object> liveGameData = new Dictionary<string, object>();
        liveGameData["/castle_life/"] = 50;
        liveGameData["/is_won/"] = false;
        liveGameData["/is_game_running/"] = true;
        liveGameData["/dice_outcome/"] = 6;
        mDatabase.UpdateChildrenAsync(liveGameData);

    }

    public void updateScoreToServer()
    {
        WriteNewScore("rssszxcumal", 203321320);
        Debug.Log("updated to server");
    }

    private void WriteNewScore(string userId, int score)
    {

        // Create new entry at /user-scores/$userid/$scoreid and at
        // /leaderboard/$scoreid simultaneously
        FirebaseDatabase firebase = FirebaseDatabase.GetInstance(FirebaseConstants.SERVER_URL);
        DatabaseReference mDatabase = firebase.GetReference("scores/"+ Random.Range(1, 999999)+"_id_" + Random.Range(1, 99999));
        //string key = mDatabase.Child("scores").Push().Key;
        Dictionary<string, object> childUpdates = new Dictionary<string, object>();
        childUpdates["/scores/"] = score;
        childUpdates["/user-scores/"] = userId;
        mDatabase.UpdateChildrenAsync(childUpdates);
         
    }

}
