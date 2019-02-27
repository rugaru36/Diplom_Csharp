using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom_main {
    class Pursuiter : Player {
        public override void calculateNextWantedPoint(double[] coordinates) {
            wantedPoint[0] = coordinates[0];
            wantedPoint[1] = coordinates[1];
        }
    }
}
