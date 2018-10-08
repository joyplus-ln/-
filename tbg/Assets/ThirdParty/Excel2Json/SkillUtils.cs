

using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class SkillUtils
{

    public static CustomSkill MakeCustomSkill(string skillId)
    {

        object ect = Assembly.Load("Assembly-CSharp").CreateInstance(skillId);//加载程序集，创建程序集里面的 命名空间.类型名 实例
        if (ect == null)
            Debug.LogError("没有这个技能" + skillId);
        return (CustomSkill)ect;//类型转换并返回

    }

    public static CustomBuff MakeCustomBuff(string BuffId)
    {

        object ect = Assembly.Load("Assembly-CSharp").CreateInstance(BuffId);//加载程序集，创建程序集里面的 命名空间.类型名 实例
        CustomBuff custombuff = (CustomBuff)ect;
        custombuff.guid = System.Guid.NewGuid().ToString();
        custombuff.Init();
        return custombuff;//类型转换并返回

    }
}
