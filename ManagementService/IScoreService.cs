﻿using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementService
{
    public interface IScoreService
    {
        bool UpdateScore(List<StudentScore> changedScores);
    }
}
