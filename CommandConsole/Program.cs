using HornetInc.JsonAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandConsole
{
    class Program
    {
        //static SimpleManager simpleManager = new SimpleManager(new ApiManager("https://localhost:44349/api/Project1488/", 0), 100);
        static SimpleManager simpleManager = new SimpleManager(new ApiManager("http://hornet.1gb.ru/api/Project1488/", 0), 250);
        static void Main(string[] args)
        {
            simpleManager.NewAnswer += SimpleManager_NewAnswer;

            while (true)
            {
                simpleManager.NewCommand(Console.ReadLine());
            }
        }

        private static void SimpleManager_NewAnswer(string ans)
        {
            Console.WriteLine(ans);
        }
    }
}
