using Serv.Logic;
using System;

namespace Serv
{
	class MainClass
	{
		public static void Main(string[] args)
		{


			DataMgr dataMgr = new DataMgr ();
			ServNet servNet = new ServNet();
			servNet.proto = new ProtocolBytes ();
			servNet.Start("127.0.0.1",1234);

            //��������ʵ��
            Scene scene = new Scene();

			while(true)
			{
				string str = Console.ReadLine();
				switch(str)
				{
				case "quit":
					servNet.Close();
					return;
				case "print":
					servNet.Print();
					break;
				}
			}

		}
	}
}
