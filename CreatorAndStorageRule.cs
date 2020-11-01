namespace Com.MarcusTS.SmartDI
{
   using System;

   /// <summary>
   ///    Interface IProvideCreatorAndStorageRule
   /// </summary>
   public interface IProvideCreatorAndStorageRule
   {
      /// <summary>
      ///    Gets or sets the provided creator.
      /// </summary>
      /// <value>The provided creator.</value>
      Func<object> ProvidedCreator { get; set; }

      /// <summary>
      ///    Gets or sets the provided storage rule.
      /// </summary>
      /// <value>The provided storage rule.</value>
      StorageRules ProvidedStorageRule { get; set; }
   }

   /// <summary>
   ///    Class CreatorAndStorageRule.
   ///    Implements the <see cref="IProvideCreatorAndStorageRule" />
   ///    Implements the <see cref="Com.MarcusTS.SmartDI.IProvideCreatorAndStorageRule" />
   /// </summary>
   /// <seealso cref="Com.MarcusTS.SmartDI.IProvideCreatorAndStorageRule" />
   /// <seealso cref="IProvideCreatorAndStorageRule" />
   public class CreatorAndStorageRule : IProvideCreatorAndStorageRule
   {
      /// <summary>
      ///    Initializes a new instance of the <see cref="CreatorAndStorageRule" /> class.
      /// </summary>
      /// <param name="creator">The creator.</param>
      /// <param name="storageRule">The storage rule.</param>
      public CreatorAndStorageRule
      (
         Func<object> creator = null,
         StorageRules storageRule = StorageRules.AnyAccessLevel
      )
      {
         ProvidedStorageRule = storageRule;

         ProvidedCreator = creator;
      }

      /// <summary>
      ///    Gets or sets the provided creator.
      /// </summary>
      /// <value>The provided creator.</value>
      public Func<object> ProvidedCreator { get; set; }

      /// <summary>
      ///    Gets or sets the provided storage rule.
      /// </summary>
      /// <value>The provided storage rule.</value>
      public StorageRules ProvidedStorageRule { get; set; }
   }
}
