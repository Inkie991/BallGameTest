using UnityEngine;

public class GameEvent { }
    
    public static class Events
    {
        public static PlayerWinEvent PlayerWinEvent = new();
        public static PlayerLoseEvent PlayerLoseEvent = new();
        public static PathIsClearEvent PathClearEvent = new();
    }

    public class PlayerWinEvent : GameEvent
    {
    }

    public class PlayerLoseEvent : GameEvent
    {
    }

    public class PathIsClearEvent : GameEvent
    {
        public Vector3 FireballPos;
    }

