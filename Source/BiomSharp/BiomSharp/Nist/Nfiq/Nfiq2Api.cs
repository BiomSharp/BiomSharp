// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using System.Runtime.InteropServices;

namespace BiomSharp.Nist.Nfiq
{
    internal class Nfiq2Api
    {
        [DllImport("Nfiq2Api.dll", EntryPoint = "Nfiq2Initialize", CallingConvention = CallingConvention.StdCall)]
        public static extern int Nfiq2Initialize([In][MarshalAs(UnmanagedType.LPStr)] string yaml);

        [DllImport("Nfiq2Api.dll", EntryPoint = "Nfiq2GetActionableFeedbackIds", CallingConvention = CallingConvention.StdCall)]
        public static extern int Nfiq2GetActionableFeedbackIds(
            ref int qActionableIdsCount, int qActionableIdsLen, IntPtr qActionableIds);

        [DllImport("Nfiq2Api.dll", EntryPoint = "Nfiq2GetQualityFeatureIds", CallingConvention = CallingConvention.StdCall)]
        public static extern int Nfiq2GetQualityFeatureIds(
            ref int qFeatureIdsCount, int qFeatureIdsLen, IntPtr qFeatureIds);

        [DllImport("Nfiq2Api.dll", EntryPoint = "Nfiq2GetVersion", CallingConvention = CallingConvention.StdCall)]
        public static extern void Nfiq2GetVersion(IntPtr version, IntPtr ocv);

        [DllImport("Nfiq2Api.dll", EntryPoint = "Nfiq2ComputeScore", CallingConvention = CallingConvention.StdCall)]
        public static extern int Nfiq2ComputeScore(ref int nfiq2Score, ref int nfiq2Error, int fpos, byte[] pixels, int width, int height, int ppi);

        [DllImport("Nfiq2Api.dll", EntryPoint = "Nfiq2ComputeScoreEx", CallingConvention = CallingConvention.StdCall)]
        public static extern int Nfiq2ComputeScore1(ref int nfiq2Score, ref int nfiq2Error, double[] qActionableVals, double[] qFeatureVals, int fpos, byte[] pixels, int width, int height, int ppi);

        [DllImport("Nfiq2Api.dll", EntryPoint = "InitNfiq2", CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr InitNfiq2(out IntPtr pCStrHash);

        [DllImport("Nfiq2Api.dll", EntryPoint = "GetNfiq2Version", CallingConvention = CallingConvention.StdCall)]
        private static extern void GetNfiq2Version(
            out int major, out int minor, out int patch, out IntPtr pOCV_Version_CStr);

        [DllImport("Nfiq2Api.dll", EntryPoint = "ComputeNfiq2Score", CallingConvention = CallingConvention.StdCall)]
        private static extern int ComputeNfiq2Score(int fpos, byte[] pixels, int size, int width, int height, int ppi);
    }
}
