using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.Core;
using UnityEngine;

public class CloudSaveSample : MonoBehaviour
{
    [SerializeField] private TMP_InputField saveInputField;
    private string word;

    private void Start()
    {
        if (saveInputField != null)
        {
            saveInputField.onEndEdit.AddListener(OnSaveWord);
        }
    }

    public void OnSaveWord(string newWord)
    {
        word = newWord;
    }

    public async void SaveData()
    {
        var playerData = new Dictionary<string, object>{
          {"firstKeyName", word},
          {"secondKeyName", 123}
        };
        await CloudSaveService.Instance.Data.Player.SaveAsync(playerData);
        Debug.Log($"Saved data {string.Join(',', playerData)}");

    }
    public async void DeleteData()
    {
        await CloudSaveService.Instance.Data.Player.DeleteAsync("firstKeyName");
    }

    public async void LoadData()
    {
        var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> {
          "firstKeyName", "secondKeyName"
        });

        if (playerData.TryGetValue("firstKeyName", out var firstKey))
        {
            Debug.Log($"firstKeyName value: {firstKey.Value.GetAs<string>()}");
        }

        if (playerData.TryGetValue("secondKeyName", out var secondKey))
        {
            Debug.Log($"secondKey value: {secondKey.Value.GetAs<int>()}");
        }
    }
}