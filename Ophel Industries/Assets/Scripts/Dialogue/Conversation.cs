namespace Dialogue {
  [System.Serializable]
  public class Conversation {
    public Message[] messages;
    public int id;
    
    public Conversation(int id_num, int size) {
      id = id_num;
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
