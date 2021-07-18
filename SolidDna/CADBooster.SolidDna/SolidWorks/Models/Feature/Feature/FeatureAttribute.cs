﻿using SolidWorks.Interop.sldworks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Attribute feature
    /// </summary>
    public class FeatureAttribute : SolidDnaObject<IAttribute>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureAttribute(IAttribute model) : base(model)
        {

        }

        #endregion
    }
}
