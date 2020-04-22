namespace Com.MarcusTS.SmartDI
{
   using System;
   using System.Collections.Generic;

   /// <summary>
   ///    Interface IConflictResolution
   /// </summary>
   public interface IConflictResolution
   {
      /// <summary>
      ///    Gets or sets the type of the master.
      /// </summary>
      /// <value>The type of the master.</value>
      Type MasterType { get; set; }

      /// <summary>
      ///    Gets or sets the type to cast with storage rule.
      /// </summary>
      /// <value>The type to cast with storage rule.</value>
      KeyValuePair<Type, IProvideCreatorAndStorageRule> TypeToCastWithStorageRule { get; set; }
   }

   /// <summary>
   ///    Class ConflictResolution.
   ///    Implements the <see cref="IConflictResolution" />
   ///    Implements the <see cref="Com.MarcusTS.SmartDI.IConflictResolution" />
   /// </summary>
   /// <seealso cref="Com.MarcusTS.SmartDI.IConflictResolution" />
   /// <seealso cref="IConflictResolution" />
   public class ConflictResolution : IConflictResolution
   {
      /// <summary>
      ///    Gets or sets the type of the master.
      /// </summary>
      /// <value>The type of the master.</value>
      public Type MasterType { get; set; }

      /// <summary>
      ///    Gets or sets the type to cast with storage rule.
      /// </summary>
      /// <value>The type to cast with storage rule.</value>
      public KeyValuePair<Type, IProvideCreatorAndStorageRule> TypeToCastWithStorageRule { get; set; }
   }
}