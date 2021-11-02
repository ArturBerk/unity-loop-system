using UnityEngine;

namespace Abg
{
    public class LoopBehaviour : MonoBehaviour
    {
        private Disposables disposables;
        
        protected virtual void OnEnable()
        {
            if (this is IUpdateLoopElement updateLoopElement)
            {
                disposables += Loops.Update.Start(updateLoopElement.OnUpdate);
            }
            if (this is IFixedUpdateLoopElement fixedUpdateLoopElement)
            {
                disposables += Loops.Update.Start(fixedUpdateLoopElement.OnFixedUpdate);
            }
            if (this is ILateUpdateLoopElement lateUpdateLoopElement)
            {
                disposables += Loops.Update.Start(lateUpdateLoopElement.OnLateUpdate);
            }
        }

        protected virtual void OnDisable()
        {
            disposables.Dispose();
        } 
    }

    public interface IUpdateLoopElement
    {
        void OnUpdate();
    }

    public interface IFixedUpdateLoopElement
    {
        void OnFixedUpdate();
    }

    public interface ILateUpdateLoopElement
    {
        void OnLateUpdate();
    }
}