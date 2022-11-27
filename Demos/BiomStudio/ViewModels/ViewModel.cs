// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using System.ComponentModel;

namespace BiomStudio.ViewModels
{
    public abstract class ViewModel : IViewModel
    {
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ReadOnly { get; set; }
    }
}
