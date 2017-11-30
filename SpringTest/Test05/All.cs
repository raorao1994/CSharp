using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test05
{
    /// <summary>
    /// 抽象人类
    /// </summary>
    public abstract class Person
    {
        /// <summary>
        /// 使用工具劳动
        /// </summary>
        public abstract void Work();
    }

    public interface ITool
    {
        /// <summary>
        /// 使用工具
        /// </summary>
        void UseTool();
    }
    public class Spear : ITool
    {
        public void UseTool()
        {
            Console.WriteLine("使用长矛");
        }
    }
    public class PrimitivePerson : Person
    {
        /// <summary>
        /// 原始社会使用长矛打猎
        /// </summary>
        public override void Work()
        {
            //知道打猎使用的是长矛，并且制造长矛
            ITool tool = new Spear();
            tool.UseTool();
            Console.WriteLine("使用长矛打猎");
        }
    }
    public class Hoe : ITool
    {
        public void UseTool()
        {
            Console.WriteLine("使用锄头");
        }
    }
    public static class ToolFactory
    {
        /// <summary>
        /// 工厂制造工具
        /// </summary>
        /// <returns></returns>
        public static ITool CreateTool()
        {
            return new Hoe();  // 制造锄头
        }
    }
    public class EconomyPerson : Person
    {
        /// <summary>
        /// 经济社会使用锄头耕作
        /// </summary>
        public override void Work()
        {
            //不用知道什么工具，只需知道工厂能买到工具，而不自己制造工具，但仅由工厂制造锄头
            ITool tool = ToolFactory.CreateTool();
            tool.UseTool();
            Console.WriteLine("经济社会使用工具耕作");
        }
    }
    public class Computer : ITool
    {
        public void UseTool()
        {
            Console.WriteLine("使用电脑");
        }
    }
    public class ModernPerson : Person
    {
        /// <summary>
        /// 从外部获取工具
        /// </summary>
        public ITool Tool { get; set; }

        /// <summary>
        /// 现在人用不需要知道电脑是哪来的，直接拿来办公
        /// </summary>
        public override void Work()
        {
            //不知道使用什么工具和哪来的工具，只是机械化的办公
            Tool.UseTool();
            Console.WriteLine("使用工具办公");
        }
    }
}
