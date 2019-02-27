using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom_main {
    class Pursuiter : Player {
        public override void calculateNextWantedPoint(Player escaper) {
            double[] eCoordinates = escaper.getCoordinates();

            wantedPoint[0] = eCoordinates[0];
            wantedPoint[1] = eCoordinates[1];

            return;
        }
    }
}
