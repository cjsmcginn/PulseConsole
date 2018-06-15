using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PulseConsole.Services;

namespace PulseConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            RunMain();

        }
        /// <summary>
        /// Allows for continuation of program until user decides to exit
        /// </summary>
        static void RunMain()
        {
            var maxTries = 10;
            var tries = 0;
            var inputValue = default(int?);

            while (!inputValue.HasValue && tries < 10)
            {
                tries += 1;
                inputValue = GetInputAction();
                if (!inputValue.HasValue)
                    Console.Out.WriteLine("Input Value was not Recognized as a Valid Integer");
            }

            if (inputValue.GetValueOrDefault() == 0)
                Environment.Exit(0);
            else
            {
                var records = MemberService.LookupMembershipInfo(inputValue.GetValueOrDefault());
                var output = MemberService.GetMembershipInfoResponse(records);
                output.ForEach(o => Console.Out.WriteLine(o));
                RunMain();
            }

        }
        /// <summary>
        /// Handles user input
        /// </summary>
        /// <returns></returns>
        static int? GetInputAction()
        {
            int? result = default(int?);
            Console.Out.WriteLine("Please Enter a MemberID to Query Results or Enter 0 to Quit Application");
            var input = Console.In.ReadLine();
            var inputTest = 0;
            if (int.TryParse(input, out inputTest))
                result = inputTest;
            return result;
        }
    }
}
