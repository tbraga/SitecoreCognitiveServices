﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.ProjectOxford.Vision.Contract;

namespace Sitecore.SharedSource.CognitiveServices.LaunchDemo.Models
{
    public class ComputerVisionResult
    {
        public AnalysisResult Result { get; set; }
        public string ImageUrl { get; set; }
    }
}