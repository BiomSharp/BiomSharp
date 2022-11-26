// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using BiomSharp.Nist.Nfiq;
using BiomStudio.Forms;
using BiomStudio.Properties;

namespace BiomStudio
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] _)
        {
            if (!Nfiq2.Initialize(Resources.ModelInfo, Resources.ModelData))
            {
                MessageBox.Show("NFIQ2 not initialized!");
            }
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}
