﻿using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Sketch feature
    /// </summary>
    public class FeatureSketch : SolidDnaObject<ISketch>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSketch(ISketch model) : base(model)
        {

        }

        #endregion
    }
}
