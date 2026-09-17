using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myrmidon.Core.Ecs {
    public interface IPartStore {
        void Remove(uint id);
        bool Contains(uint id);
    }
}
