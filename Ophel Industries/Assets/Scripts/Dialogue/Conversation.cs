namespace Dialogue {
  public class Conversation {
    public Message[] messages;

    public Conversation(int size) {
      messages = new Message[size];
    }

    public void Set(int index, Message message) {
      messages[index] = message;
    }

    public Message Get(int index) {
      return messages[index];
    } 
  }
}
