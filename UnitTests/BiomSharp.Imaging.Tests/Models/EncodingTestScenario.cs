using BiomSharp.Imaging.Wsq;
using BiomSharp.Imaging.Wsq.Tree;

namespace BiomSharp.Imaging.Tests.Models;

public sealed record EncodingTestScenario(WsqFilterType FilterType, 
    Filter SharedFilter,
    float[] OriginalHi, 
    float[] OriginalLo, 
    EncodingTestCase[] Cases);
