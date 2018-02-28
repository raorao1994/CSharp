using System;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 车载指令的窗户位置： 1(司机)， 2(副驾驶)， 3(司机后面)，4(副驾驶后面)，5(天窗)
    /// </summary>
    public enum CarInstructionPositionEnum
    {
        司机 = 1,
        副驾驶 = 2,
        司机后面 = 3,
        副驾驶后面 = 4,
        天窗 = 5
    }
}