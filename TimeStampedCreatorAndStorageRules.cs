namespace Com.MarcusTS.SmartDI
{
   using System;
   using System.Collections.Generic;

   /// <summary>
   ///    Interface ITimeStampedCreatorAndStorageRules
   /// </summary>
   public interface ITimeStampedCreatorAndStorageRules
   {
      /// <summary>
      ///    Gets or sets the creators and storage rules.
      /// </summary>
      /// <value>The creators and storage rules.</value>
      IDictionary<Type, IProvideCreatorAndStorageRule> CreatorsAndStorageRules { get; set; }

      /// <summary>
      ///    Gets or sets the when added.
      /// </summary>
      /// <value>The when added.</value>
      DateTime WhenAdded { get; set; }
   }

   /// <summary>
   ///    Class TimeStampedCreatorAndStorageRules.
   ///    Implements the <see cref="ITimeStampedCreatorAndStorageRules" />
   ///    Implements the <see cref="Com.MarcusTS.SmartDI.ITimeStampedCreatorAndStorageRules" />
   /// </summary>
   /// <seealso cref="Com.MarcusTS.SmartDI.ITimeStampedCreatorAndStorageRules" />
   /// <seealso cref="ITimeStampedCreatorAndStorageRules" />
   public class TimeStampedCreatorAndStorageRules : ITimeStampedCreatorAndStorageRules
   {
      /// <summary>
      ///    Gets or sets the creators and storage rules.
      /// </summary>
      /// <value>The creators and storage rules.</value>
      public IDictionary<Type, IProvideCreatorAndStorageRule> CreatorsAndStorageRules { get; set; }

      /// <summary>
      ///    Gets or sets the when added.
      /// </summary>
      /// <value>The when added.</value>
      public DateTime WhenAdded { get; set; }
   }
}