using System;
using System.Collections.Generic;
using System.Text;
using WijkagentModels;

namespace WijkagentWPF
{
    public interface IFilter
    {
        List<Offence> Filter(List<Offence> offences);
    }
}
