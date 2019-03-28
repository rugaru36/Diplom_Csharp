using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom_main {
    class Pursuiter : Player {
        protected override void getNextWantedPoint(Player escaper) {
            double[] eCoordinates = escaper.getCoordinates();

            wantedPoint = new double[] {
                eCoordinates[0] + escaper.getSpeedVector()[0] * escaper.getRadius(),
                eCoordinates[1] + escaper.getSpeedVector()[1] * escaper.getRadius()
            };
        }
    }
}
