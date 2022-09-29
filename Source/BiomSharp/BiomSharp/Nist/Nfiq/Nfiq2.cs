// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using System.Runtime.InteropServices;
using System.Text;
using BiomSharp.Imaging;

namespace BiomSharp.Nist.Nfiq
{
    public static class Nfiq2
    {
        public static bool Initialized { get; private set; }

        public static RandomForestModel? Model { get; private set; }

        private static List<string>? actionableIds = null;
        private static List<string>? featureIds = null;

        public static bool Initialize(string modelInfo, byte[] modelData)
        {
            if (!Initialized)
            {
                Model = new RandomForestModel(modelInfo);
                int error = Nfiq2Api.Nfiq2Initialize(Model.UnzipToYaml(modelData));
                if (error == 0)
                {
                    int idCount = 0;
                    byte[] idBuffer = new byte[2048];
                    var pinnedArray = GCHandle.Alloc(idBuffer, GCHandleType.Pinned);
                    IntPtr idPtr = pinnedArray.AddrOfPinnedObject();
                    error = Nfiq2Api.Nfiq2GetActionableFeedbackIds(ref idCount, 256, idPtr);
                    pinnedArray.Free();
                    if (error == 0)
                    {
                        actionableIds = Encoding.ASCII.GetString(idBuffer)
                            .TrimEnd((char)0)
                            .Split(new char[] { '\n' }).ToList();
                        pinnedArray = GCHandle.Alloc(idBuffer, GCHandleType.Pinned);
                        idPtr = pinnedArray.AddrOfPinnedObject();
                        error = Nfiq2Api.Nfiq2GetQualityFeatureIds(ref idCount, 2048, idPtr);
                        pinnedArray.Free();
                        if (error == 0)
                        {
                            featureIds = Encoding.ASCII.GetString(idBuffer)
                                .TrimEnd((char)0)
                                .Split(new char[] { '\n' }).ToList();
                        }
                    }
                }
                Initialized = error == 0;
            }
            return Initialized;
        }

        public static void GetVersion(out string libNfiq2Version, out string libOpenCvVersion)
        {
            byte[] libNfiq2 = new byte[20];
            byte[] libOpenCv = new byte[20];

            var pinnedLibNfiq2 = GCHandle.Alloc(libNfiq2, GCHandleType.Pinned);
            IntPtr libNfiq2Ptr = pinnedLibNfiq2.AddrOfPinnedObject();
            var pinnedLibOpenCv = GCHandle.Alloc(libOpenCv, GCHandleType.Pinned);
            IntPtr libOpenCvPtr = pinnedLibOpenCv.AddrOfPinnedObject();

            Nfiq2Api.Nfiq2GetVersion(libNfiq2Ptr, libOpenCvPtr);

            pinnedLibNfiq2.Free();
            pinnedLibOpenCv.Free();

            libNfiq2Version = Encoding.ASCII.GetString(libNfiq2).TrimEnd((char)0);
            libOpenCvVersion = Encoding.ASCII.GetString(libOpenCv).TrimEnd((char)0);
        }

        public static int ComputeScore(out Nfiq2Analysis? analysis, out int nfiq2Error,
            int fingerCode, SimpleBitmap rawImage)
        {
            int nfiq2Score = 0;
            nfiq2Error = -1;
            int error = Nfiq2Api.Nfiq2ComputeScore(ref nfiq2Score, ref nfiq2Error,
                fingerCode, rawImage.Pixels, rawImage.Width, rawImage.Height,
                rawImage.Resolution < 500 ? 500 : rawImage.Resolution);
            analysis = error == 0 ? new Nfiq2Analysis(nfiq2Score) : null;
            return error;
        }

        public static int ComputeAll(out Nfiq2Analysis? analysis, out int nfiq2Error,
            int fingerCode, SimpleBitmap rawImage)
        {
            int nfiq2Score = 0;
            nfiq2Error = -1;
            if (actionableIds != null && featureIds != null)
            {
                double[] actionableValues = new double[actionableIds.Count];
                double[] featureValues = new double[featureIds.Count];
                int error = Nfiq2Api.Nfiq2ComputeScore1(ref nfiq2Score, ref nfiq2Error,
                    actionableValues, featureValues,
                    fingerCode, rawImage.Pixels, rawImage.Width, rawImage.Height,
                    rawImage.Resolution < 500 ? 500 : rawImage.Resolution);
                if (error == 0)
                {
                    analysis = new Nfiq2Analysis(nfiq2Score, allMeasures: true);
                    analysis.SetActionableFeedback(actionableIds, actionableValues);
                    analysis.SetQualityFeatures(featureIds, featureValues);
                }
                else
                {
                    analysis = null;
                }
                return error;
            }
            throw new InvalidOperationException("NFIQ2 algorithm is not initialized");
        }
    }
}
