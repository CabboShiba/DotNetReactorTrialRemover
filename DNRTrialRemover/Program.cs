using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
namespace DNRTrialRemover
{
    internal class Program
    {
        private static string File = null;
        static void Main(string[] args)
        {
            Console.Title = ".NET Reactor 6.9 14 Days Trial Remover by Cabbo - https://github.com/CabboShiba";
            try
            {
                File = args[0];
            }
            catch(Exception ex)
            {
                Log("Please use Drag&Drop. Press enter to leave...");
                Console.ReadLine();
                Process.GetCurrentProcess().Kill();
            }
            try
            {
                object[] parameters = null;
                var assembly = Assembly.LoadFile(Path.GetFullPath(File));
                var paraminfo = assembly.EntryPoint.GetParameters();
                parameters = new object[paraminfo.Length];
                Harmony patch = new Harmony("DotNetReactorTrialRemover6.9_https://github.com/CabboShiba");
                patch.PatchAll(Assembly.GetExecutingAssembly());
                assembly.EntryPoint.Invoke(null, parameters);
            }
            catch (Exception ex)
            {
                Log($"Could not load {File}\n{ex.Message}");
            }
            Console.ReadLine();
        }  
        
        [HarmonyPatch(typeof(System.Math), nameof(System.Math.Abs), new[] { typeof(int)})]
        class FixDate
        {
            static bool Prefix(ref int value)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                try
                {
                    if(value >= 14)
                    {
                        const int NewDate = 0;
                        value = NewDate;
                        Log("Succesfully modified date. New Date: " + value);
                    }
                }
                catch (Exception ex)
                {
                    Log($"Error during Patch:\n {ex.Message}");
                }
                Console.ResetColor();
                return false;
            }
        }
        public static void Log(string Data)
        {
            string Log = $"[{DateTime.Now} - TrialRemover] {Data}";
            Console.WriteLine(Log);
        }
    }
}
