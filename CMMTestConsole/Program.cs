using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;

namespace CoverMyMeds.SAML.CMMTestConsole
{
    /// <summary>
    /// Basic console test harness for verifying system settings, etc...
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Test Console{0}", Environment.NewLine);

            List<string> MainMenuOptions = new List<string>();
            MainMenuOptions.Add("Validate Certificate for Issuing SAML Response");
            MainMenuOptions.Add("Validate Certificate for Verifying SAML Response");

            bool MenuLoop = true;
            while (MenuLoop)
            {
                //DisplayMenu();

                string MenuChoice = GetValidUserInputFromList("Menu", "x", MainMenuOptions);
                switch (MenuChoice.ToLower())
                {
                    case "1":
                        VerifyX509CertForIssuingSAML();
                        break;
                    case "x":
                        MenuLoop = false;
                        break;
                    //default:
                }
            }
            Console.WriteLine("Shutting down Console");
        }

        #region Certificate Testing and Admin

        // http://msdn.microsoft.com/en-us/library/bfsktky3%28v=vs.100%29.aspx

        private static void VerifyX509CertForIssuingSAML()
        {
            Console.WriteLine("Validating Certificate for Issuing SAML Response");
            Console.WriteLine("--------------------------------------");
            Console.Write("Enter Certificate Name: ");
            string CertSubjectName = Console.ReadLine();
            Console.WriteLine("Select Certificate Store Location");
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("1) CurrentUser");
            Console.WriteLine("2) LocalMachine");
            //StoreLocation.
        }

        #endregion

        #region Console functions

        private static string GetValidUserInputFromList(string UserPrompt, string ExitPrompt, List<string> ValidInputs)
        {
            string UserInput = string.Empty;
            while (true)
            {
                Console.WriteLine(UserPrompt);
                Console.WriteLine("--------------------------------------");

                for (int i = 0; i < ValidInputs.Count; i++)
                {
                    Console.WriteLine("{0}) {1}", i + 1, ValidInputs[i]);
                }

                Console.WriteLine("--------------------------------------");

                Console.Write("Submit choice (press {0} to exit):", ExitPrompt);

                UserInput = Console.ReadLine();

                if (UserInput == ExitPrompt)
                {
                    UserInput = string.Empty;
                    break;
                }
                int iUserInput;
                if (int.TryParse(UserInput, out iUserInput))
                {
                    if (iUserInput >= 1 && iUserInput <= ValidInputs.Count + 1)
                    {
                        UserInput = ValidInputs[iUserInput];
                        break;
                    }
                }
            }

            return UserInput;
        }

        //private static void DisplayMenu()
        //{
        //    Console.WriteLine("Select test function");
        //    Console.WriteLine("--------------------------------------");
        //    Console.WriteLine("1) Validate Certificate for Issuing SAML Response");
        //    Console.WriteLine("2) Validate Certificate for Verifying SAML Response");
        //    Console.WriteLine("--------------------------------------");
        //    Console.Write("Submit choice (press x to exit):");
        //}

        #endregion
    }
}
