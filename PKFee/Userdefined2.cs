using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;

namespace PKFee
{
    public class Userdefined2 : IFeeRule
    {
        
        public Userdefined2()
        {

        }
        public override decimal CalcFee()
        {
            //创建C#编译器实例
           // ICodeCompiler comp = (new CSharpCodeProvider().CreateCompiler());
            CSharpCodeProvider comp = new CSharpCodeProvider();
            //编译器的传入参数
            CompilerParameters cp = new CompilerParameters();
            cp.ReferencedAssemblies.Add("system.dll");                //添加程序集 system.dll 的引用
            cp.GenerateExecutable = false;                            //不生成可执行文件
            cp.GenerateInMemory = true;                                //在内存中运行
            CompilerResults cr = comp.CompileAssemblyFromSource(cp, FeeText);
            if (cr.Errors.HasErrors)                            //如果有错误
            {
                return -1;
            }
            Assembly a = cr.CompiledAssembly;                        //获取编译器实例的程序集
            object result = a.CreateInstance("PKFee.UserdefinedText");
            System.Reflection.MethodInfo methodinfo = result.GetType().GetMethod("CalcFee");
            return (decimal)methodinfo.Invoke(result, new object[] { ParkingBeginTime, ParkingEndTime });

            //UserdefinedText aa = new UserdefinedText();
            //return aa.CalcFee(ParkingBeginTime, ParkingEndTime);
            
        }
    }
}
