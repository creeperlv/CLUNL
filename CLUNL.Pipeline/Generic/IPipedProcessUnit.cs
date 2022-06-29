using System.Collections.Generic;
using System.Text;

namespace CLUNL.Pipeline.Generic
{
    public interface IPipedProcessUnit<T>
    {
        T Process(T input);
    }
}
