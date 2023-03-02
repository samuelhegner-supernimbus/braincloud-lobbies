using System;
using System.Collections;
using System.Collections.Generic;
using BrainCloud;
using UnityEngine;

public class BrainCloudInterface : MonoBehaviour
{
    [SerializeField] private BCConfig bcConfig;
    private BrainCloudWrapper bc = null;
    
    public void Start()
    {
        bc = bcConfig.GetBrainCloud();
        bc.RTTService.EnableRTT(RTTConnectionType.WEBSOCKET, OnSuccess, OnFail);
        bc.RTTService.RegisterRTTLobbyCallback(LobbyCallback);

        AuthenticateAnonymous();
    }

    private void LobbyCallback(string jsonresponse)
    {
        Debug.Log(jsonresponse);
    }


    public void AuthenticateAnonymous()
    {
        bc.AuthenticateAnonymous(OnSuccess_Authenticate, OnError_Authenticate);
    }

    private void OnError_Authenticate(int status, int reasoncode, string jsonerror, object cbobject)
    {
        Debug.Log("Failed Auth");
    }

    private void OnSuccess_Authenticate(string jsonresponse, object cbobject)
    {
        Debug.Log("Authed");
        
        /*
         "algo": {
            "strategy": "ranged-absolute",
            "alignment": "center",
            "ranges": [
              5,
              7.5,
              10
            ]
          }
         */

        Dictionary<string, object> algo = new Dictionary<string, object>()
        {
            {"strategy", "ranged-absolute"},
            {"alignment", "center"},
            {"ranges", new []{5, 7.5, 10}}
        };

        bc.LobbyService.FindOrCreateLobby("basic"
            , 0
            , 3
            , algo
            , new Dictionary<string, object>()
            , 60
            , true
            , new Dictionary<string, object>()
            , ""
            , new Dictionary<string, object>()
            , new string[]{}
            , OnSuccess
            , OnFail
            );
    }

    private void OnFail(int status, int reasoncode, string jsonerror, object cbobject)
    {
        Debug.LogError(jsonerror);
    }

    private void OnSuccess(string jsonresponse, object cbobject)
    {
        Debug.Log(jsonresponse);
    }
}
