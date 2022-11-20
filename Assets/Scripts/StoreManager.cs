using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using System;

public class StoreManager : MonoBehaviour
{
    private int PlayersMoney;
    public Dictionary<string, int> PlayersPowerUp;
    private Dictionary<string, List<string>> displayToId;
    public Text PlayersMoneyText;
    public Button BuyButton1;
    public Button BuyButton2;
    public Button BuyButton3;
    public Button BuyButton4;
    public Button BuyButton5;
    public Button BuyButton6;

    public void Start()
    {
        GetPlayersMoney();
        UpdateInventory();
    }
   
    public void Update()
    {
        
        try
        {
            int soldi = Convert.ToInt32(PlayersMoneyText.text);
            if (soldi < 50)
            {
                BuyButton1.interactable = false;
                BuyButton2.interactable = false;
                BuyButton3.interactable = false;
                BuyButton4.interactable = false;
                BuyButton5.interactable = false;
                BuyButton6.interactable = false;
            }
            else
            {
                if (soldi < 100)
                {
                    BuyButton2.interactable = false;
                    BuyButton4.interactable = false;
                    BuyButton6.interactable = false;
                }
                else
                {

                    BuyButton1.interactable = true;
                    BuyButton2.interactable = true;
                    BuyButton3.interactable = true;
                    BuyButton4.interactable = true;
                    BuyButton5.interactable = true;
                    BuyButton6.interactable = true;
                }
            }
        }
        catch (Exception e)
        {
           
        }
    }


    public void GetItemPrices()
    {
        GetCatalogItemsRequest request = new GetCatalogItemsRequest();
        request.CatalogVersion = "PlasticProof";
        PlayFabClientAPI.GetCatalogItems(request, result =>
         {
             List<CatalogItem> lista = result.Catalog;
             foreach (CatalogItem i in lista)
             {
                 uint costo = i.VirtualCurrencyPrices["MN"];
                 UnityEngine.Debug.Log("Nome:" + i.DisplayName + " Costo:" + costo);
             }
         }, OnPlayFabError);
    }

    public void OnPlayFabError(PlayFabError obj)
    {
        UnityEngine.Debug.Log("There is a problem!");
    }

    public void BuyItem(string item)
    {
        PurchaseItemRequest request = new PurchaseItemRequest();
        request.CatalogVersion = "PlasticProof";
        request.ItemId = item;
        request.VirtualCurrency = "MN";
        switch (item)
        {
            case "X2/5S": 
                request.Price = 50;
                break;

            case "X2/10S":
                request.Price = 100;
                break;

            case "SLOW/5S":
                request.Price = 50;
                break;

            case "SLOW/10S":
                request.Price = 100;
                break;

            case "CLEAN":
                request.Price = 50;
                break;

            case "XPCLEAN":
                request.Price = 100;
                break;
        }
        PlayFabClientAPI.PurchaseItem(request, result =>
         {
             UnityEngine.Debug.Log("Item purchased" + result.Items[0].DisplayName);
             int appoggio = int.Parse(PlayersMoneyText.text);
             PlayersMoneyText.text = (appoggio - request.Price).ToString();
             UpdateInventory();
         }, OnPlayFabError);


    }


    public void GetPlayersMoney()
    {

         Dictionary<string, int> virtualCurrency = new Dictionary<string, int>();
         PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),result =>
        {
           



            foreach (var pair in result.VirtualCurrency)

                   virtualCurrency.Add(pair.Key, pair.Value);
            

            
            PlayersMoney =virtualCurrency["MN"] - 10;
            PlayerPrefs.SetFloat("Money", (float)PlayersMoney);
            PlayersMoneyText.text = PlayersMoney.ToString();
        },OnPlayFabError);
}

    public void UpdateInventory()
    {
        GetUserInventoryRequest request = new GetUserInventoryRequest();
        PlayFabClientAPI.GetUserInventory(request, result =>
        {

            displayToId = new Dictionary<string, List<string>>();
            PlayersPowerUp = new Dictionary<string, int>();
            for (int i = 0; i < result.Inventory.Count; i++)
            {
                if (!(displayToId.ContainsKey(result.Inventory[i].DisplayName)))
                {
                    List<string> appoggio = new List<string>();
                    displayToId.Add(result.Inventory[i].DisplayName, appoggio);
                    PlayersPowerUp.Add(result.Inventory[i].DisplayName, 0);
                }
                displayToId[result.Inventory[i].DisplayName].Add(result.Inventory[i].ItemInstanceId);
                PlayersPowerUp[result.Inventory[i].DisplayName]++;
            }
            foreach (KeyValuePair<string, int> kvp in this.PlayersPowerUp)
            {
                UnityEngine.Debug.Log("Key = {" + kvp.Key + "}, Value = {" + kvp.Value + "} ");
                try
                {
                    GameObject.Find("Owned" + kvp.Key).GetComponent<Text>().text = kvp.Value.ToString();
                }
                catch (Exception e)
                {

                }
            }
                
          
        }, OnPlayFabError);
    }

    public void RemovePowerUp(string item)
    {
        ConsumeItemRequest request = new ConsumeItemRequest();
        request.ConsumeCount = 1;
        request.ItemInstanceId = displayToId[item][0];
        UnityEngine.Debug.Log(displayToId[item][0]);
        PlayFabClientAPI.ConsumeItem(request, result => {
            UpdateInventory();
        }, OnPlayFabError);

    }

    public Dictionary<string, int> GetPlayersPowerUp()
    {
        return this.PlayersPowerUp;
    }
}
