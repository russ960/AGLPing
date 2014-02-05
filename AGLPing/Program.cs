using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Data.SqlClient;

namespace AGLPing
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Pass AGL name and number of desired executions.  Eg: AGLPing <servername> <exectution count>");
                //Console.ReadKey(true);
            }
            else
            {
                SqlConnection AGLConn = new SqlConnection("Server=" + args[0] + ";Database=master;Trusted_Connection=True;Connection Timeout=60;MultiSubnetFailover=true;");

                int runCount = 0;
                bool test = false;

                if (args.Length > 1)
                {
                    test = int.TryParse(args[1], out runCount);
                }

                //Console.WriteLine(test);
                if (test == false)
                    {
                        Console.WriteLine("Please include a number for execution count.");
                    }
                
                int i = 1;

                while (i <= runCount)
                {
                    i = i + 1;
                    
                    var start = DateTime.Now;
                    
                    try
                    {
                        AGLConn.Open();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                                        
                    SqlCommand payloadCommand = new SqlCommand("SELECT COUNT(*) FROM sys.all_objects", AGLConn);
                    payloadCommand.ExecuteNonQuery();

                    try
                    {
                        AGLConn.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }

                    var end = DateTime.Now;
                    
                    TimeSpan pingtime = end - start;

                    Console.WriteLine("Server: " + args[0] + ", Ping time (ms): " + pingtime.TotalMilliseconds);
                }
            }
        }
    }
}
