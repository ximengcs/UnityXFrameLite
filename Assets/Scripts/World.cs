
using XFrame.Core;
using XFrameShare.Core.Network;

public class World
{
    private static NetworkModule s_Net;

    public static NetworkModule Net => s_Net ?? Entry.GetModule<NetworkModule>();
    private static MessageModule s_Message;

    public static MessageModule Message => s_Message ??= Entry.GetModule<MessageModule>();
}