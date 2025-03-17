using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public void TriggerParentFireballEvent()
    {
        PlayerMovement parent = GetComponentInParent<PlayerMovement>();
        if (parent != null)
        {
            parent.CallThrowFireballEvent();
        }
        else
        {
            Debug.LogError("PlayerMovement component not found on parent!");
        }
    }
}