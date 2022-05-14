using _Application;

namespace _Util.GameState
{
    public class StateMachine : GameStateMachine
    {
        public override GameState DefaultState => BaseState.LoadState(DebugParameter.Data.Debug.UseDebugSkipScene, this);
    }
}