#region License

// Copyright (c) 2019  Marcus Technical Services, Inc. <marcus@marcusts.com>
//
// This file, CreatorAndStorageRule.cs, is a part of a program called AccountViewMobile.
//
// AccountViewMobile is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Permission to use, copy, modify, and/or distribute this software
// for any purpose with or without fee is hereby granted, provided
// that the above copyright notice and this permission notice appear
// in all copies.
//
// AccountViewMobile is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For the complete GNU General Public License,
// see <http://www.gnu.org/licenses/>.

#endregion

namespace Com.MarcusTS.SmartDI
{
   using System;

   /// <summary>
   /// Interface IProvideCreatorAndStorageRule
   /// </summary>
   public interface IProvideCreatorAndStorageRule
   {
      /// <summary>
      /// Gets or sets the provided creator.
      /// </summary>
      /// <value>The provided creator.</value>
      Func<object> ProvidedCreator { get; set; }

      /// <summary>
      /// Gets or sets the provided storage rule.
      /// </summary>
      /// <value>The provided storage rule.</value>
      StorageRules ProvidedStorageRule { get; set; }
   }

   /// <summary>
   /// Class CreatorAndStorageRule.
   /// Implements the <see cref="IProvideCreatorAndStorageRule" />
   /// Implements the <see cref="Com.MarcusTS.SmartDI.IProvideCreatorAndStorageRule" />
   /// </summary>
   /// <seealso cref="Com.MarcusTS.SmartDI.IProvideCreatorAndStorageRule" />
   /// <seealso cref="IProvideCreatorAndStorageRule" />
   public class CreatorAndStorageRule : IProvideCreatorAndStorageRule
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="CreatorAndStorageRule" /> class.
      /// </summary>
      /// <param name="creator">The creator.</param>
      /// <param name="storageRule">The storage rule.</param>
      public CreatorAndStorageRule
      (
         Func<object> creator     = null,
         StorageRules storageRule = StorageRules.AnyAccessLevel
      )
      {
         ProvidedStorageRule = storageRule;

         ProvidedCreator = creator;
      }

      /// <summary>
      /// Gets or sets the provided creator.
      /// </summary>
      /// <value>The provided creator.</value>
      public Func<object> ProvidedCreator { get; set; }

      /// <summary>
      /// Gets or sets the provided storage rule.
      /// </summary>
      /// <value>The provided storage rule.</value>
      public StorageRules ProvidedStorageRule { get; set; }
   }
}