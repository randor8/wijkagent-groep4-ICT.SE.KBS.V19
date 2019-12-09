using System;
using System.Collections.Generic;
using WijkagentModels;

namespace WijkagentWPF
{
    public interface IFilter : IEquatable<IFilter>
    {
        List<Offence> ApplyOn(List<Offence> offences);
    }
}
