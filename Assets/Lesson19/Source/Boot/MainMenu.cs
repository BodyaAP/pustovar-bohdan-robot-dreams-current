using MyLesson19.StateMachineSystem.SceneManagement;

namespace MyLesson19
{
    namespace StateMachineSystem.GameStateSystem
    {
        public class MainMenu : GameStateBase
        {
            public MainMenu(StateMachine stateMachine, byte stateId, ISceneManager sceneManager, Scenes scene) : base(
                stateMachine, stateId, sceneManager, scene)
            {
            }

            public override void Dispose()
            {
            }
        }
    }
}