using UnityEngine;

namespace _Application
{
    public class Stage : MonoBehaviour
    {
        public static Stage Current { get; private set; }

        public StageId Id { get; private set; }

        private void Awake()
        {
            Current = this;

            Id = StageLoader.SceneNameToStageId(gameObject.scene.name);
        }
    }
}