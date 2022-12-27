using CADBooster.SolidDna.SolidWorks.Models.Configuration;
using SolidWorks.Interop.sldworks;
using System.Collections.Generic;
using System.Linq;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks model configuration
    /// </summary>
    public class ModelConfiguration : SolidDnaObject<Configuration>
    {
        #region Public Properties

        /// <summary>
        /// The list of child configurations. Is an empty list whn this configuration does not have any children.
        /// </summary>
        public List<ModelConfiguration> Children
        {
            get
            {
                // Get an array of child configuration
                var children = (object[])UnsafeObject.GetChildren();

                // Return an empty list instead of null when there are no children.
                // Create a list of ModelConfigurations if there are children.
                return children == null ? new List<ModelConfiguration>() : children.Cast<Configuration>().Select(x => new ModelConfiguration(x)).ToList();
            }
        }

        /// <summary>
        /// Comments value of the configuration properties.
        /// </summary>
        public string Comment
        {
            get => UnsafeObject.Comment;
            set => UnsafeObject.Comment = value;
        }

        /// <summary>
        /// User-friendly description text.
        /// </summary>
        public string Description
        {
            get => UnsafeObject.Description;
            set => UnsafeObject.Description = value;
        }

        /// <summary>
        /// The ID number of this configuration. Is unique within this model and never changes.
        /// </summary>
        public int Id => UnsafeObject.GetID();

        /// <summary>
        /// Whether this configuration is a child configuration and has a parent.
        /// </summary>
        public bool IsDerived => UnsafeObject.IsDerived();

        /// <summary>
        /// The name of the configuration. The name is the main identifier and is used in most methods that have a configuration argument.
        /// </summary>
        public string Name
        {
            get => UnsafeObject.Name;
            set => UnsafeObject.Name = value;
        }

        /// <summary>
        /// The parent configuration of this configuration. Is null when this is not a derived configuration.
        /// </summary>
        public ModelConfiguration Parent => IsDerived ? new ModelConfiguration(UnsafeObject.GetParent()) : null;

        /// <summary>
        /// The type of configuration, for example standard, flat pattern or one of two weldment options.
        /// </summary>
        public ConfigurationTypes Type => (ConfigurationTypes)UnsafeObject.Type;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ModelConfiguration(Configuration model) : base(model)
        {

        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns a user-friendly string with configuration properties.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Configuration name: {Name}. Type: {Type}. Is derived: {IsDerived}";
        }

        #endregion
    }
}
