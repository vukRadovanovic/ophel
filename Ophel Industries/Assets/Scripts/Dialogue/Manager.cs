using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue {
  public class Manager : MonoBehaviour {
    private static Manager _instance;
    public static Manager Instance { get { return _instance; } }
    
    private Message[] messages;
    private int dialogueIndex;
    private Loader dialogueLoader;

    /**
     * Enforce singleton pattern and initialize instance variables.
     */
    void Awake() {
      // check if instance has been initialized
      if (_instance == null)  {
        _instance = this;
      }
      // destroy any other instances
      else if (_instance != this) {
        Destroy(gameObject);
      }
      
      // allow this gameobject to persist across scenes
      DontDestroyOnLoad(gameObject);

      dialogueIndex = 0;
      dialogueLoader = new Loader();
    }

    /**
     * Load a conversation into the manager using the dialogue from its JSON
     * file.
     */
    public void LoadConversation(string dialogueFile, int convoId) {
      string dFilePath = Loader.DIALOGUE_PATH + dialogueFile;

      // load the conversations from the dialogue file
      Dialogue.Conversation[] loadedConvos =
          dialogueLoader.NestedLoad<Dialogue.Conversation>(dFilePath);

      // find the desired conversation from the array of conversations
      Dialogue.Conversation convo = loadedConvos[convoId];
      
      if (convo.id != convoId) {
        throw new System.Exception("Invalid conversation retrieved");
      }

      messages = convo.messages;
      dialogueIndex = 0;
    }

    /**
     * Move to the next message in the conversation.
     */
    public void MoveNext() {
      dialogueIndex++;
    }

    /**
     * Retrieve the message that the manager is currently on.
     */
    public Message CurrentMessage() {
      return messages[dialogueIndex];
    }

    /**
     * Determine if the conversation has ended and all messages for the current
     * conversation have been displayed.
     */
    public bool IsFinished() {
      return dialogueIndex >= messages.Length;
    }
  }
}
