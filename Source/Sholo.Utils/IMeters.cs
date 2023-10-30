using System;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;

namespace Sholo.Utils;

public interface IMeters
{
    Meter Meter { get; }

    TInstrument GetOrAddInstrument<TInstrument, T>(Func<Instrument<T>> valueFactory, [CallerMemberName] string name = null)
        where TInstrument : Instrument<T>
        where T : struct;
}
