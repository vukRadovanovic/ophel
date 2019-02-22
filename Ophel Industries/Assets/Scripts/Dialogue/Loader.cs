using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader {

  public const string DIALOGUE_PATH = "Dialogue/";

  public Loader() {
  }

  /**
   * Deserialize nested JSON from the given JSON file and return an array of
   * the deserialized objects..
   */
  public T[] NestedLoad<T>(string jsonFile) {
    TextAsset jfile = Resources.Load(jsonFile) as TextAsset;
    string jsonText = jfile.text;
    T[] deserializedObjs = JsonParser.MultiDeserialize<T>(jsonText);
    return deserializedObjs;
  }
}
