
using UnityEngine;
using XFrame.Modules.Conditions;
using XFrame.Modules.Pools;

namespace Game.Test
{
    public class ConditionConst
    {
        public static int LEVEL = 1;

        public const int PERSISIT_HELPER = 0;
    }

    public class ConditionHelperTest : PoolObjectBase, IConditionHelper
    {
        public int Type => ConditionConst.PERSISIT_HELPER;

        public bool CheckFinish(IConditionGroupHandle group)
        {
            return group.GetData<bool>(group.Name + "finish");
        }

        public void MarkFinish(IConditionGroupHandle group)
        {
            group.SetData<bool>(group.Name + "finish", true);
        }
    }

    public class LevelCondition1Test : PoolObjectBase, IConditionCompare<int>
    {
        public int Target
        {
            get
            {
                Debug.LogWarning(ConditionConst.LEVEL);
                return ConditionConst.LEVEL;
            }
        }

        public bool CheckFinish(IConditionHandle info)
        {
            int level = info.GetData<int>(info.Group.Name);
            Debug.LogWarning($"CheckFinish {info.Param.IntValue} {level}");
            return info.Param.IntValue <= level;
        }

        public void OnEventTrigger(int param)
        {
            Debug.LogWarning($"OnEventTrigger {GetHashCode()} " + param);
        }

        public bool Check(IConditionHandle info, int param)
        {
            Debug.LogWarning("check " + param);
            info.SetData(info.Group.Name, param);
            return info.Param.IntValue <= param;
        }
    }
}
