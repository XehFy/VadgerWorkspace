﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VadgerWorkspace.Data
{
    public enum Stages : byte
    {
        starting = 0,
        SelectService = 1,
        SelectTown = 2,
        Descriptioning = 3,
        Waiting = 4,
        Chating = 5,
        Management = 6,
    }
}
