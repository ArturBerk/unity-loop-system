# unity-loop-system
Simple and fast Update, FixedUpdate, LateUpdate callbacks alternative, based on PlayerLoop API

## Simple usage

Inherit **LoopBehaviour** and implement **IUpdateLoopElement** or **IFixedUpdateLoopElement** or **ILateUpdateLoopElement**

``` c#

class Looped : LoopBehaviour, IUpdateLoopElement, IFixedUpdateLoopElement, ILateUpdateLoopElement {
    
    public void OnUpdate() {
        // Update stuff
    }
    
    public void OnFixedUpdate() {
        // FixedUpdate stuff
    }
    
    public void OnLateUpdate() {
        // LateUpdate stuff
    }
    
}

```

## Advanced usage

``` c#
class Looped : MonoBehaviour {

    private Disposables subscription;

    private void OnEnable() {
        subscription += Loops.Update.Start(OnUpdate);
    }
    
    private void OnDisable() {
        subscription.Dispose();
    }
    
    private void OnUpdate() {
        // Update stuff
    }
}

```
