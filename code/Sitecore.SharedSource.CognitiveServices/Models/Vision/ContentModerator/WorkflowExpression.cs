﻿
using Sitecore.SharedSource.CognitiveServices.Enums;

namespace Sitecore.SharedSource.CognitiveServices.Models.Vision.ContentModerator
{
    public class WorkflowExpression
    {
        public string Description { get; set; }
        public IExpression Expression { get; set; }
        public ContentModeratorReviewType Type { get; set; }
    }
}