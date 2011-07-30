#region Using statements
using UnityEngine;

#endregion

public class KongregateGameService : FistBumpGameService
{
    private bool m_IsGuest = false;
    private string m_Username = "";
    private int m_UserId = 0;
    private string m_Items = "";
    private string m_GameAuthToken = "";
    
    public bool IsGuest
    {
        get
        {
            CallAPIFunction("kongregate.services.isGuest()", "SetIsGuest");
            return m_IsGuest;
        }
    }

    public void SetIsGuest(object returnValue)
    {
        if (bool.TryParse(returnValue.ToString(), out m_IsGuest))
        {
            if (m_IsGuest)
                m_Username = "";
        }
    }

    public string Username
    {
        get
        {
            CallAPIFunction("kongregate.services.getUsername()", "SetUsername");
            return m_Username;
        }
    }
    public void SetUsername(object returnValue)
    {
        m_Username = returnValue.ToString();
    }

    public int UserId
    {
        get
        {
            CallAPIFunction("kongregate.services.getUserId()", "SetUserId");
            return m_UserId;
        }
    }

    void SetUserId(object returnValue)
    {
        if (int.TryParse(returnValue.ToString(), out m_UserId))
        {
            if (m_UserId == 0)
                m_Username = "";
        }
    }

    public string GameToken
    {
        get
        {
            return m_GameAuthToken;
        }
    }
      

    public string[] Items
    {
        get
        {
            CallAPIFunction("getUserItems()");
            return string.IsNullOrEmpty(m_Items) ? new string[0] : m_Items.Split(',');
        }
    }
    void SetUserItems(object returnValue)
    {
        m_Items = returnValue.ToString();
    }

    public void ShowSignIn()
    {
        CallAPIFunction("if(kongregate.services.isGuest()) kongregate.services.showSignInBox()");
    }

    public void SubmitStat(string statistic, int value)
    {
        CallAPIFunction(string.Format("kongregate.stats.submit('{0}', {1})", statistic, value));
    }

    public void PurchaseItem(string item)
    {
        Debug.Log("[Kongregate] Attempting purchase of " + item);
        CallAPIFunction(string.Format("purchaseItem('{0}')", item));
    }

    void CallAPIFunction(string functionCall)
    {
        CallAPIFunction(functionCall, null);
    }

    void CallAPIFunction(string functionCall, string callback)
    {
        if (APILoaded)
        {
            if (string.IsNullOrEmpty(callback))
            {
                Application.ExternalEval(functionCall);
            }
            else
            {
                Application.ExternalEval(string.Format(@"
					var value = {0};
					SendUnityMessage('{1}', String(value));
				", functionCall, callback));
            }
        }
    }

    public void LogMessage(string message)
    {
        Debug.Log(message);
    }

    void OnKongregateAPILoaded(string userInfo)
    {
        Debug.Log("[Kongregate] API Loaded");
        APILoaded = true;

        string[] userParams = userInfo.Split('|');
        int userID = int.Parse(userParams[0]);
        string userName = userParams[1];
        string gameToken = userParams[2];

        if (userID == 0)
        {
            SetIsGuest(true);
        }
        else
        {
            SetUserId(userParams[0]);
            SetUsername(userName);
            SetIsGuest(false);
            m_GameAuthToken = gameToken;
        }
    }


    public override void Initialize()
    {
        Application.ExternalEval(@"
            var kongregateObject = '" + gameObject.name + @"';

			function getUserItems()
			{
				kongregate.mtx.requestUserItemList(null, onUserItems);
			}

			function onUserItems(result)
			{
				if (result.success)
				{
					var items = '';
					for (var i = 0; i < result.data.length; i++)
					{
						items += result.data[i].identifier;
						if (i < result.data.length - 1)
							items += ',';
					}
					SendUnityMessage('SetUserItems', items);
				}
			}

			function purchaseItem(item)
			{
				kongregate.mtx.purchaseItems([item], onPurchaseResult);
				Log('Purchase sent...');				
			}

			function onPurchaseResult(result)
			{
				if (result.success)
				{
					Log('Purchase complete...');
					getUserItems();
				}
			}

			// Utility function to send data back to Unity
			function SendUnityMessage(functionName, message)
			{
				Log('Calling to: ' + functionName);
				var unity = kongregateUnitySupport.getUnityObject();
				unity.SendMessage(kongregateObject, functionName, message);
			}

			function Log(message)
			{
				var unity = kongregateUnitySupport.getUnityObject();
				unity.SendMessage(kongregateObject, 'LogMessage', '[Kongregate] ' + message);
			}

			if(typeof(kongregateUnitySupport) != 'undefined')
			{
  				kongregateUnitySupport.initAPI(kongregateObject, 'OnKongregateAPILoaded');
  			};
		");
    }
}