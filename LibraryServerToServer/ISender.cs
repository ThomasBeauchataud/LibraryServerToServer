using System.Collections.Generic;

namespace LibraryServerToServer
{
    public interface ISender
    {
        Flux Send(Flux flux, List<object> parameters);
    }
}
