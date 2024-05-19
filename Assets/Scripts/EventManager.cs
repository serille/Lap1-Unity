using UnityEngine.Events;

public static class EventManager
{
    public static UnityEvent musicVolumeChanged = new UnityEvent();
    public static UnityEvent sfxVolumeChanged = new UnityEvent();
    public static UnityEvent fliesVolumeChanged = new UnityEvent();
}
