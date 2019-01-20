namespace Dialogue {
  public class Message {
    public string name;
    public string phrase;
    public float typeSpeed;
    // public ??? image;
    // public ??? fontColor;

    public Message(string speaker, string quote, float speed) {
      name = speaker;
      phrase = quote;
      typeSpeed = speed;
    }
  }
}
