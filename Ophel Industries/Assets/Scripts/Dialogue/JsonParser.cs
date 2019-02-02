using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonParser {

  public static T[] MultiDeserialize<T>(string jsonStr) {
    JsonContainer<T> container = GatherJsonItems<T>(jsonStr);
    Debug.Log(container);
    Debug.Log(container.jsonItems);
    return container.jsonItems;
  }

  public static string MultiSerialize<T>(T[] jsonItems) {
    return "Incomplete"; 
  }

  public static T Deserialize<T>(string jsonStr) {
    T jsonObj = JsonUtility.FromJson<T>(jsonStr);
    return jsonObj;
  }

  public static string Serialize<T>(T jsonObj) {
    string jsonStr = JsonUtility.ToJson(jsonObj);
    return jsonStr;
  }

  private static JsonContainer<T> GatherJsonItems<T>(string jsonStr) {
    return JsonUtility.FromJson<JsonContainer<T>>(jsonStr);
  }

  [System.Serializable]
  private class JsonContainer<T> {
    public T[] jsonItems;
  }
}
