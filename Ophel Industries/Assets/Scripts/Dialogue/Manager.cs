using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue {
  // TODO: make manager able to be loaded by controller
  public class Manager : MonoBehaviour {
    private Message[] messages;
    private int dialogueIndex;

    void Awake() {
      messages = new Message[3];
      messages[0] = new Message("Jim", "Hello, my name is Jim.", 0.15f);
      messages[1] = new Message("Jim #2", "Hi, my name is Jim too.", 0.05f);
      messages[2] = new Message("Jim #3", "Salutations", 0.3f);
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
