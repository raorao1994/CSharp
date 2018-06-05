using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 动态编译
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            run();
        }
        void run()
        {
            CSharpCodeProvider objCSharpCodePrivoder = new CSharpCodeProvider();
            ICodeCompiler objICodeCompiler = objCSharpCodePrivoder.CreateCompiler();

            CompilerParameters objCompilerParameters = new CompilerParameters();

            //添加需要引用的dll
            objCompilerParameters.ReferencedAssemblies.Add("System.dll");
            objCompilerParameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");

            //是否生成可执行文件
            objCompilerParameters.GenerateExecutable = false;

            //是否生成在内存中
            objCompilerParameters.GenerateInMemory = true;

            //编译代码
            CompilerResults cr = objICodeCompiler.CompileAssemblyFromSource(objCompilerParameters, textBox1.Text);

            if (cr.Errors.HasErrors)
            {
                var msg = string.Join(Environment.NewLine, cr.Errors.Cast<CompilerError>().Select(err => err.ErrorText));
                MessageBox.Show(msg, "编译错误");
            }
            else
            {
                Assembly objAssembly = cr.CompiledAssembly;
                object objHelloWorld = objAssembly.CreateInstance("Test");
                MethodInfo objMI = objHelloWorld.GetType().GetMethod("Hello");
                objMI.Invoke(objHelloWorld, null);
            }
        }

        /// <summary>
        /// 动态编译并执行代码
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="SavePath">保存路径</param>
        /// <returns>返回输出内容</returns>
        public CompilerResults DebugRun(string code, string SavePath)
        {
            ICodeCompiler complier = new CSharpCodeProvider().CreateCompiler();
            //设置编译参数
            CompilerParameters paras = new CompilerParameters();
            //引入第三方dll
            paras.ReferencedAssemblies.Add("System.dll");
            //引入自定义dll
            paras.ReferencedAssemblies.Add(@"D:\自定义方法\自定义方法\bin\LogHelper.dll");
            //是否内存中生成输出 
            paras.GenerateInMemory = false;
            //是否生成可执行文件
            paras.GenerateExecutable = false;
            paras.OutputAssembly = SavePath + ".dll";

            //编译代码
            CompilerResults result = complier.CompileAssemblyFromSource(paras, code);

            return result;
        }
        /// <summary>
        /// 调用并执行dll文件
        /// </summary>
        /// <param name="dllpath"></param>
        /// <param name="namespaceStr"></param>
        /// <param name="funName"></param>
        /// <returns></returns>
        public string LoadDllAndRun(string dllpath,string namespaceStr,string funName,List<Object> parameters)
        {
            //dll的调用，采用反射方法
            Assembly assembly = Assembly.LoadFile(dllpath);
            Type AType = assembly.GetType(namespaceStr);
            MethodInfo method = AType.GetMethod(funName);
            var t = method.ReturnType.Name;
            //object[] parameters = new object[] { 传入参数 };
            var returnResult = Convert.ToString(method.Invoke(null, parameters));
            return returnResult;
            //returnResult则为自定义方法返回值。
        }
    }
}
