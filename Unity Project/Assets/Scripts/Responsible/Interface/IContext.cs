using System.Collections;

public interface IContext
{
    // 現在のステートのUpdate()を呼び出す
    void Update();

    // 現在のステートのFixedUpdate()を呼び出す
    void FixedUpdate();

    // 現在のステートを変更する
    void ChangeState(IState state);

    // 現在のステートを取得
    IState GetState();
}
