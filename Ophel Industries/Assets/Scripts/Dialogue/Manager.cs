using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue {
  // TODO: make manager able to be loaded by controller
  // TODO: use singleton pattern
  public class Manager : MonoBehaviour {
    private Message[] messages;
    private int dialogueIndex;
    private Loader dialogueLoader;

    void Awake() {
      dialogueIndex = 0;
      dialogueLoader = new Loader();

      // FIXME: temporary testing for loader
      LoadConversation("Initial", 0);
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
        // TODO: raise error instead
        Debug.Log("ERROR: invalid conversation retrieved");
        return;
      }

      messages = convo.messages;
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
