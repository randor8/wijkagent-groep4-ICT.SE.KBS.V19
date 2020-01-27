using System.Collections.Generic;
using WijkagentModels;

namespace WijkagentWPF
{
    public interface IFilter
    {
        List<Offence> ApplyOn(List<Offence> offences);
    }
}
