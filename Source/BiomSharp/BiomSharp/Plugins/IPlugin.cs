// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

namespace BiomSharp.Plugins
{
    public interface IPlugin<TId>
    {
        TId? Id { get; }
        string? Description { get; }
    }
}
