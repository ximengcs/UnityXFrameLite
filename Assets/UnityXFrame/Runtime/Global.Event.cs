using XFrame.Modules.Event;

public static partial class Global
{
    private static IEventSystem s_EventSystemInst;
    private static IEventSystem s_EventSystem
    {
        get
        {
            if (s_EventSystemInst == null)
                s_EventSystemInst = Event.NewSys();
            return s_EventSystemInst;
        }
    }

    public static void Listen(int eventId, XEventHandler handler)
    {
        s_EventSystem.Listen(eventId, handler);
    }

    public static void Listen(int eventId, XEventHandler2 handler)
    {
        s_EventSystem.Listen(eventId, handler);
    }

    public static void Trigger(int eventId)
    {
        s_EventSystem.Trigger(eventId);
    }

    public static void Trigger(XEvent e)
    {
        s_EventSystem.Trigger(e);
    }

    public static void TriggerNow(int eventId)
    {
        s_EventSystem.TriggerNow(eventId);
    }

    public static void TriggerNow(XEvent e)
    {
        s_EventSystem.TriggerNow(e);
    }

    public static void Unlisten(int eventId, XEventHandler handler)
    {
        s_EventSystem.Unlisten(eventId, handler);
    }

    public static void Unlisten(int eventId, XEventHandler2 handler)
    {
        s_EventSystem.Unlisten(eventId, handler);
    }

    public static void Unlisten(int eventId)
    {
        s_EventSystem.Unlisten(eventId);
    }

    public static void Unlisten()
    {
        s_EventSystem.Unlisten();
    }
}
