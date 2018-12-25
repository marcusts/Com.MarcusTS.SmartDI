// *********************************************************************************
// Assembly         : Com.MarcusTS.SmartDI
// Author           : Stephen Marcus (Marcus Technical Services, Inc.)
// Created          : 05-05-2018
// Last Modified On : 12-24-2018
//
// <copyright file="CreatorAndStorageRule.cs" company="Marcus Technical Services, Inc.">
//     @2018 Marcus Technical Services, Inc.
// </copyright>
//
// MIT License
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// *********************************************************************************

namespace Com.MarcusTS.SmartDI
{
   using System;

   /// <summary>
   /// Interface IProvideCreatorAndStorageRule
   /// </summary>
   public interface IProvideCreatorAndStorageRule
   {
      #region Public Properties

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

      #endregion Public Properties
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
      #region Public Constructors

      /// <summary>
      /// Initializes a new instance of the <see cref="CreatorAndStorageRule" /> class.
      /// </summary>
      /// <param name="creator">The creator.</param>
      /// <param name="storageRule">The storage rule.</param>
      public CreatorAndStorageRule(Func<object> creator     = null,
                                   StorageRules storageRule = StorageRules.AnyAccessLevel)
      {
         ProvidedStorageRule = storageRule;

         ProvidedCreator = creator;
      }

      #endregion Public Constructors

      #region Public Properties

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

      #endregion Public Properties
   }
}