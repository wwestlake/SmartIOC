using BusinessLogicEngine;
using SmartIOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    interface IInjected
    {
        void SayHello();
    }

    [Injectable(typeof(IInjected), SmartIOC.LifeCycle.Instance)]
    class InjectMe : IInjected
    {

        public void SayHello()
        {
            Console.WriteLine("Hello from Inject Me");
        }
    }

    interface ITest
    {
        void SayHello();
    }

    abstract class ATest
    {
        public abstract void SayHello();
    }

    [Injectable(typeof(ITest), LifeCycle.Singleton)]
    class A : ITest
   {
        [Inject]
        private IInjected injectme;

        public void SayHello()
        {
            Console.WriteLine("Hello from ITest Implementation");
            injectme.SayHello();
        }
    }

    [Injectable(typeof(ATest), LifeCycle.Singleton)]
    class B : ATest
    {

        public override void SayHello()
        {
            Console.WriteLine("Hello from ATest Implementation");
        }
    }

    interface ITestClass
    {
        void Test();
        void ChangeState();
    }

    [Injectable(typeof(ITestClass), LifeCycle.Singleton)]
    class TestClass : ITestClass
    {
        ITest a;
        ATest b;
        bool called = false;

        public TestClass()
        {

        }
        public TestClass(ITest a, ATest b)
        {
            this.a = a;
            this.b = b;
        }

        public void Test()
        {
            if (called) Console.WriteLine("Hey second time!");
            Console.WriteLine("I'm here");
            a.SayHello();
            b.SayHello();
        }

　
　
　
        public void ChangeState()
        {
            called = true;
        }
    }

　
    class Program
    {
        static void Main(string[] args)
        {

            Scanner.Instance.Scan(Assembly.GetExecutingAssembly());

　
            var instance = Configurator.Resolve<ITestClass>();

            instance.Test();
            instance.ChangeState();

　
            var second = Configurator.Resolve<ITestClass>();
            second.Test();

　
　
            Console.ReadKey();
        }
    }
}
