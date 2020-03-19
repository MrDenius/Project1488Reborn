using HornetInc.JsonAPI.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace HornetInc.JsonAPI
{
    public class SimpleManager
    {
        ApiManager apiManager;
        int TimeCDms;

        public SimpleManager(ApiManager apiManager, int timeCDms = 100)
        {
            this.apiManager = apiManager;
            TimeCDms = timeCDms;

            new Thread(WaitAnswer).Start();
        }

        public delegate void DNewAnswer(string ans);
        public event DNewAnswer NewAnswer;

        public void NewCommand(Command com)
        {
            apiManager.AddNewCommand(com);
        }

        public void NewCommand(string SmartCom)
        {
            switch (SmartCom)
            {
                case "AllUsers":
                    AnswersHandle(apiManager.GetUsers());
                    break;
                case "AllUsersOnline":
                    AnswersHandle(apiManager.GetOnlineUsers());
                    break;
                default: 
                    apiManager.AddNewCommand(new Command(SmartCom));
                    break;
            }
        }
        bool WaitEnable = false;
        private void WaitAnswer()
        {
            Thread.CurrentThread.IsBackground = true;
            while (true)
            {
                while (WaitEnable)
                {
                    AnswersHandle(apiManager.GetNewAnswers());
                    Thread.Sleep(TimeCDms);
                }
                Thread.Sleep(1000);
            }
        }

        public void Start()
        {
            WaitEnable = true;
        }
        public void Stop()
        {
            WaitEnable = false;
        }

        private void AnswersHandle(string[] answers)
        {
            foreach (string ans in answers)
            {
                if (ans != null)
                {
                    NewAnswer(ans);
                }
            }
        }
    }
}
