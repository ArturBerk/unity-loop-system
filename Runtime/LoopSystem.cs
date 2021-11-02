using System;
using System.Collections.Generic;
using UnityEngine.LowLevel;

namespace Abg
{
    public interface ILoopSystem
    {
        IDisposable Start(Action updatable);
    }

    internal class LoopSystem<TUpdateGroup> : ILoopSystem
    {
        private UpdatableSlot[] updatables = new UpdatableSlot[1000];
        private List<int> updatablesToRemove = new List<int>();
        private int count;

        private void OnUpdate()
        {
            foreach (var elementIndex in updatablesToRemove)
            {
                RemoveAtReal(elementIndex);
            }

            updatablesToRemove.Clear();

            for (int i = 0; i < count; i++)
            {
                updatables[i].Updatable();
            }
        }

        public IDisposable Start(Action updatable)
        {
            if (count == updatables.Length)
            {
                Array.Resize(ref updatables, (int) (count * 1.5f));
            }

            var updatableSlot = new UpdatableSlot(this, updatable, count);
            updatables[count] = updatableSlot;

            if (count == 0)
            {
                PlayerLoopExtensions.ModifyCurrentPlayerLoop((ref PlayerLoopSystem system) =>
                {
                    system.GetSystem<TUpdateGroup>().AddSystem<LoopSystem<TUpdateGroup>>(OnUpdate);
                });
            }

            ++count;

            return updatableSlot.Registration;
        }

        private void RemoveAtReal(int index)
        {
            var lastUpdatableIndex = count - 1;
            if (index == lastUpdatableIndex)
            {
                updatables[index] = default;
            }
            else
            {
                ref var lastUpdatable = ref updatables[lastUpdatableIndex];
                lastUpdatable.Registration.Index = index;
                updatables[index] = lastUpdatable;
                updatables[lastUpdatableIndex] = default;
            }

            --count;

            if (count == 0)
            {
                PlayerLoopExtensions.ModifyCurrentPlayerLoop((ref PlayerLoopSystem system) =>
                {
                    system.GetSystem<TUpdateGroup>().RemoveSystem<LoopSystem<TUpdateGroup>>(false);
                });
            }
        }

        private void RemoveAt(int index)
        {
            updatablesToRemove.Add(index);
        }

        private struct UpdatableSlot
        {
            public readonly Action Updatable;
            public readonly UpdatableRegistration Registration;

            public UpdatableSlot(LoopSystem<TUpdateGroup> loopSystem, Action updatable, int registration)
            {
                Updatable = updatable;
                Registration = new UpdatableRegistration(loopSystem, registration);
            }
        }

        private class UpdatableRegistration : IDisposable
        {
            private LoopSystem<TUpdateGroup> loopSystem;
            internal int Index;

            public UpdatableRegistration(LoopSystem<TUpdateGroup> loopSystem, int index)
            {
                this.loopSystem = loopSystem;
                Index = index;
            }

            public void Dispose()
            {
                if (Index < 0) return;
                loopSystem.RemoveAt(Index);
                Index = -1;
            }
        }
    }
}