﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UMA
{
    public class UMAGeneratorStub : UMAGeneratorBase
    {
        public override void addDirtyUMA(UMAData umaToAdd)
        {
        }

        public override bool IsIdle()
        {
            return false;
        }

        public override int QueueSize()
        {
            return 0;
        }

        public override void removeUMA(UMAData umaToRemove)
        {
        }

        public override bool updatePending(UMAData umaToCheck)
        {
            return false;
        }

        public override bool updateProcessing(UMAData umaToCheck)
        {
            return false;
        }

        public override void Work()
        {
        }
    }
}