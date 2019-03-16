namespace Action {
  public interface IMoveable {

    bool isMoving { get; set; }
    bool affectedByPause { get; set; }

    void StartMovement();
    void StopMovement();
  }
}
