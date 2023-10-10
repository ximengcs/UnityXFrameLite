using System;
using UnityXFrame.Core;
using XFrame.Modules.Tasks;

namespace UnityXFrameLib.Tasks
{
    public static class TaskExt
    {
        public static ActionTask Invoke(Action handler)
        {
            ActionTask task = Global.Task.GetOrNew<ActionTask>().Add(handler);
            task.Start();
            return task;
        }

        public static ActionTask Invoke(this ActionTask task, Action handler)
        {
            task.Add(handler);
            task.Start();
            return task;
        }

        public static ActionTask NextFrame(Action handler)
        {
            ActionTask task = Global.Task.GetOrNew<ActionTask>().Add(handler, true);
            task.Start();
            return task;
        }

        public static ActionTask NextFrame(this ActionTask task, Action handler)
        {
            task.Add(handler, true);
            task.Start();
            return task;
        }

        public static ActionTask Delay(float delayTime, Action handler, bool nextFrame = false)
        {
            ActionTask task = Global.Task.GetOrNew<ActionTask>().Add(delayTime, handler, nextFrame);
            task.Start();
            return task;
        }

        public static ActionTask Delay(this ActionTask task, float delayTime, Action handler, bool nextFrame = false)
        {
            task.Add(delayTime, handler, nextFrame);
            task.Start();
            return task;
        }

        public static ActionTask Invoke(Func<bool> handler, bool nextFrame = false)
        {
            ActionTask task = Global.Task.GetOrNew<ActionTask>().Add(handler, nextFrame);
            task.Start();
            return task;
        }

        public static ActionTask Invoke(this ActionTask task, Func<bool> handler, bool nextFrame = false)
        {
            task.Add(handler, nextFrame);
            task.Start();
            return task;
        }

        public static ActionTask Invoke(Func<float> handler, bool nextFrame = false)
        {
            ActionTask task = Global.Task.GetOrNew<ActionTask>().Add(handler, nextFrame);
            task.Start();
            return task;
        }

        public static ActionTask Invoke(this ActionTask task, Func<float> handler, bool nextFrame = false)
        {
            task.Add(handler, nextFrame);
            task.Start();
            return task;
        }

        public static ActionTask Beat(float gapTime, Func<bool> handler, bool nextFrame = false)
        {
            ActionTask task = Global.Task.GetOrNew<ActionTask>().Add(gapTime, handler, nextFrame);
            task.Start();
            return task;
        }

        public static ActionTask Beat(this ActionTask task, float gapTime, Func<bool> handler, bool nextFrame = false)
        {
            task.Add(gapTime, handler, nextFrame);
            task.Start();
            return task;
        }
    }
}
