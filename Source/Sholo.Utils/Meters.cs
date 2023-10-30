using System;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using Sholo.Utils.Collections;

namespace Sholo.Utils;

[PublicAPI]
public class Meters : IMeters
{
    public Meter Meter { get; }
    private LazyConcurrentDictionaryThing<string, Instrument> Instruments { get; } = new();

    public TInstrument GetOrAddInstrument<TInstrument, T>(Func<Instrument<T>> valueFactory, [CallerMemberName] string name = null)
        where TInstrument : Instrument<T>
        where T : struct
    {
        var result = Instruments.GetOrAdd(name, valueFactory);
        return (TInstrument)result;
    }

    public Meters(string applicationName, [CanBeNull] string version)
    {
        Meter = new(applicationName, version);
    }
}
