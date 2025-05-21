using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Myrmidon.Core.Actors;

namespace Myrmidon.Core.UI {
    public interface IUIService {
        public void Refresh();
        public void CenterOnActor(Actor actor);
    }

    public class UIService : IUIService {

        public void Refresh() {
        }
        public void CenterOnActor(Actor actor) {

        }
        public void ShowMessage(string message) {

        }
    }

}
